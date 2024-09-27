using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlayerData
{
    public static IReadOnlyList<Reward> Rewards => _rewards;
    public static IReadOnlyList<Reward> RunRewards => _runRewards;
    public static int PlayerPlace => _playerPlace;
    public static bool IsRewardsEmpty => _rewards.Count == 0;
    public static bool IsRunRewardsEmpty => _runRewards.Count == 0;
    public static IReadOnlyList<OpeningChest> OpeningChests => _openingChests;
    public static IReadOnlyList<ICompanyBiomInfoReadOnly> CompanyBiomInfos => _companyBiomInfos;
    // Consumables
    public static int Level => _level;
    public static int Coins => _coins;
    public static int Diamonds => _diamonds;
    public static int Tickets => _tickets;
    // Player
    public static IReadOnlyList<Gadget> PlayerGadgets => _playerGadgets;
    public static int Experience => _experience;
    public static int PlayerRace => _playerRace;
    public static int PlayerDive => _playerDive;
    public static int PlayerAscend => _playerAscend;
    public static int PlayerGlide => _playerGlide;
    public static int PassedTrainings => _passedTrainings;
    // Settings
    public static int FPS => _fps;
    public static bool IsMusicOn => _isMusicOn;
    public static bool IsSoundsOn => _isSoundsOn;
    public static bool IsHapticOn => _isHapticOn;
    // Shop
    public static DateTime LastTimeBattlePassBought => _lastTimeBattlePassBought;
    public static DateTime LastUpdateShopDay => _lastUpdateShopDay;
    public static int ShopRandomSeed => _shopRandomSeed;

    private static List<Reward> _rewards = new();
    private static List<Reward> _runRewards = new();
    private static int _playerPlace;
    private static OpeningChest[] _openingChests = new OpeningChest[4];
    private static CompanyBiomInfo[] _companyBiomInfos;
    // Consumables
    private static int _level;
    private static int _coins;
    private static int _diamonds;
    private static int _tickets;
    // Player
    private static List<Gadget> _playerGadgets = new();
    private static int _experience;
    private static int _playerRace;
    private static int _playerDive;
    private static int _playerAscend;
    private static int _playerGlide;
    private static int _passedTrainings;
    // Settings
    private static int _fps;
    private static bool _isMusicOn;
    private static bool _isSoundsOn;
    private static bool _isHapticOn;
    // Shop
    private static DateTime _lastTimeBattlePassBought;
    private static DateTime _lastUpdateShopDay;
    private static int _shopRandomSeed;

    public static event Action OnCoinsChanged;
    public static event Action OnDiamondsChanged;
    public static event Action OnTicketsChanged;
    public static event Action OnMusicChanged;
    public static event Action OnSoundsChanged;
    public static event Action OnHapticChanged;

    static PlayerData()
    {
        DataSaver.LoadData();
    }

    public static void LoadData(SaveData saveData)
    {
        if (saveData == null)
        {
            DataSaver.ResetData();
            return;
        }

        Debug.Log("Loading: " + saveData);

        if (saveData.Rewards != null)
        {
            foreach (RewardSaveInfo rewardSaveInfo in saveData.Rewards)
            {
                AddReward(rewardSaveInfo);
            }
        }

        _rewards = new List<Reward>();

        if (saveData.RunRewards != null)
        {
            foreach (RewardSaveInfo rewardSaveInfo in saveData.RunRewards)
            {
                AddReward(rewardSaveInfo);
            }
        }

        _runRewards = new List<Reward>();

        if (saveData.PlayerGadgets == null)
        {
            saveData.PlayerGadgets = new PlayerGadgetSaveInfo[0];
        }
        else
        {
            _playerGadgets = saveData.PlayerGadgets.Select(gadget => new Gadget(
                GadgetSettings.Instance.GetGadgetByName(gadget.GadgetName), gadget.Amount, gadget.Level)).ToList();
        }

        _openingChests = saveData.OpeningChests.Select(openingChest => new OpeningChest(openingChest)).ToArray();

        Biom[] bioms = GlobalSettings.Instance.Bioms.ToArray();
        _companyBiomInfos = new CompanyBiomInfo[bioms.Length];

        for (int i = 0; i < bioms.Length; i++)
        {
            _companyBiomInfos[i] = new(bioms[i]);
        }

        if (saveData.CompanyBiomInfos != null)
        {
            for (int i = 0; i < bioms.Length; i++)
            {
                CompanyBiomSaveInfo companyBiomSaveInfo = saveData.CompanyBiomInfos.FirstOrDefault(biom => biom.ID == bioms[i].ID);

                if (companyBiomSaveInfo != null)
                {
                    _companyBiomInfos[i].LoadSave(companyBiomSaveInfo);
                }
            }
        }

        _playerPlace = saveData.PlayerPlace;
        _experience = saveData.Experience;
        _level = saveData.Level;
        _coins = saveData.Coins;
        _diamonds = saveData.Diamonds;
        _tickets = saveData.Tickets;
        _playerRace = saveData.PlayerRace;
        _playerDive = saveData.PlayerDive;
        _playerAscend = saveData.PlayerAscend;
        _playerGlide = saveData.PlayerGlide;
        _passedTrainings = saveData.TrainingsPassed;
        _fps = saveData.FPS;
        _isMusicOn = saveData.IsMusicOn;
        _isSoundsOn = saveData.IsSoundsOn;
        _isHapticOn = saveData.IsHapticOn;
        _shopRandomSeed = saveData.ShopRandomSeed;

        if (saveData.LastTimeBattlePassBought == null)
        {
            _lastTimeBattlePassBought = DateTime.MinValue;
        }
        else
        {
            _lastTimeBattlePassBought = saveData.LastTimeBattlePassBought.DateTimeValue;
        }

        if (saveData.LastUpdateShopDay == null)
        {
            _lastUpdateShopDay = DateTime.MinValue;
        }
        else
        {
            _lastUpdateShopDay = saveData.LastUpdateShopDay.DateTimeValue;
        }

        OnCoinsChanged?.Invoke();
        OnDiamondsChanged?.Invoke();
        OnTicketsChanged?.Invoke();
    }

    public static void SetPlayerPlace(int place)
    {
        _playerPlace = place;
    }

    public static bool TryToUpdateShop(out TimeSpan remainingTime)
    {
        DateTime nextUpdate = _lastUpdateShopDay.Date.AddDays(1);

        if (nextUpdate <= DateTime.Now)
        {
            _lastUpdateShopDay = DateTime.Today;
            remainingTime = TimeSpan.Zero;
            _shopRandomSeed = GetShopSeed();
            DataSaver.SaveData();
            return true;
        }

        remainingTime = nextUpdate - DateTime.Now;
        return false;
    }

    public static void BuyBattlePass()
    {
        _lastTimeBattlePassBought = DateTime.Now;
        DataSaver.SaveData();
    }

    public static void AddOpeningChest(ChestReward.ChestType chestType)
    {
        for (int i = 0; i < _openingChests.Length; i++)
        {
            if (_openingChests[i] == null)
            {
                _openingChests[i] = new OpeningChest(chestType);
                break;
            }
        }

        DataSaver.SaveData();
    }

    public static void SetMusic(bool isMusicOn)
    {
        _isMusicOn = isMusicOn;
        OnMusicChanged?.Invoke();
        DataSaver.SaveData();
    }

    public static void SetSounds(bool isSoundsOn)
    {
        _isSoundsOn = isSoundsOn;
        OnSoundsChanged?.Invoke();
        DataSaver.SaveData();
    }

    public static void SetHaptic(bool isHapticOn)
    {
        _isHapticOn = isHapticOn;
        OnHapticChanged?.Invoke();
        DataSaver.SaveData();
    }

    public static void SetFPS(int fps)
    {
        _fps = fps;
        Application.targetFrameRate = fps;

#if UNITY_EDITOR
        Application.targetFrameRate = 10000;
#endif

        DataSaver.SaveData();
    }

    public static void TrainingPassed()
    {
        _passedTrainings++;
        DataSaver.SaveData();
    }

    public static bool TryToBuy(ShopItem shopItem)
    {
        if (TryToBuyWithCoins(shopItem)) return true;
        else if (TryToBuyWithDiamonds(shopItem)) return true;
        else if (TryToBuyWithReal(shopItem)) return true;
        else if (TryToBuyWithADS(shopItem)) return true;

        return false;
    }

    public static bool TryToSpendCoins(int coins)
    {
        if (_coins >= coins)
        {
            _coins -= coins;
            OnCoinsChanged?.Invoke();
            DataSaver.SaveData();
            return true;
        }

        return false;
    }

    public static bool TryToSpendTicket()
    {
        if (_tickets > 0)
        {
            _tickets--;
            OnTicketsChanged?.Invoke();
            DataSaver.SaveData();
            return true;
        }

        return false;
    }

    public static void AddCoins(int coins)
    {
        _coins += coins;
        OnCoinsChanged?.Invoke();
        DataSaver.SaveData();
    }

    public static void AddDiamonds(int diamonds)
    {
        _diamonds += diamonds;
        OnDiamondsChanged?.Invoke();
        DataSaver.SaveData();
    }

    public static void AddTickets(int tickets)
    {
        _tickets += tickets;
        OnTicketsChanged?.Invoke();
        DataSaver.SaveData();
    }

    public static void AddRewards(List<Reward> rewards)
    {
        _rewards.AddRange(rewards);
        SortRewards(_rewards);

        DataSaver.SaveData();
    }

    public static void AddRunRewards(List<Reward> rewards, int placement)
    {
        SetPlayerPlace(placement);
        _runRewards.AddRange(rewards);
        SortRewards(_runRewards);

        DataSaver.SaveData();
    }

    public static void AddReward(Reward reward)
    {
        _rewards.Add(reward);
        SortRewards(_rewards);

        DataSaver.SaveData();
    }

    public static void AddRunReward(Reward reward, int placement)
    {
        SetPlayerPlace(placement);
        _runRewards.Add(reward);
        SortRewards(_runRewards);

        DataSaver.SaveData();
    }

    public static List<Reward> GetRewards()
    {
        List<Reward> rewards = _rewards.ToList();
        _rewards.Clear();
        return rewards;
    }

    public static List<Reward> GetRunRewards()
    {
        List<Reward> rewards = _runRewards.ToList();
        _runRewards.Clear();
        return rewards;
    }

    public static void AddCompanyStars(int id, int stars)
    {
        CompanyBiomInfo companyBiomInfo = _companyBiomInfos.First(biom => biom.ID == id);
        AddRewards(companyBiomInfo.AddStars(stars));
    }

    private static void AddReward(RewardSaveInfo rewardSaveInfo)
    {
        if (rewardSaveInfo is ChestSaveInfo chestRewardSaveInfo)
        {
            foreach (GadgetSaveInfo gadgetSaveInfo in chestRewardSaveInfo.GadgetRewards)
            {
                AddGadget(gadgetSaveInfo);
            }

            foreach (CharacteristicSaveInfo characteristicRewardSaveInfo in chestRewardSaveInfo.CharacteristicRewards)
            {
                AddCharacteristic(characteristicRewardSaveInfo);
            }

            if (chestRewardSaveInfo.CoinsReward != null)
            {
                AddCoins(chestRewardSaveInfo.CoinsReward.Amount);
            }

            if (chestRewardSaveInfo.DiamondsReward != null)
            {
                AddDiamonds(chestRewardSaveInfo.DiamondsReward.Amount);
            }
        }
        else if (rewardSaveInfo is GadgetSaveInfo gadgetSaveInfo)
        {
            AddGadget(gadgetSaveInfo);
        }
        else if (rewardSaveInfo is CharacteristicSaveInfo characteristicRewardSaveInfo)
        {
            AddCharacteristic(characteristicRewardSaveInfo);
        }
        else if (rewardSaveInfo is CoinsSaveInfo coinsRewardSaveInfo)
        {
            AddCoins(coinsRewardSaveInfo.Amount);
        }
        else if (rewardSaveInfo is DiamondsSaveInfo diamondsRewardSaveInfo)
        {
            AddDiamonds(diamondsRewardSaveInfo.Amount);
        }
    }

    public static void AddExperience(int experience)
    {
        _experience += experience;

        int experienceToLevelUp = GlobalSettings.Instance.XPToLevelUp;

        while (_experience >= experienceToLevelUp)
        {
            _experience -= experienceToLevelUp;
            _level++;
        }

        DataSaver.SaveData();
    }

    public static void AddGadget(Gadget gadget)
    {
        Gadget playerGadget = _playerGadgets.Find(g => g.ScriptableObject == gadget.ScriptableObject);

        if (playerGadget == null)
        {
            _playerGadgets.Add(new Gadget(gadget, GadgetSettings.Instance.GetStartGadgetLevel(gadget)));

            if (_playerGadgets.Count > 1)
            {
                _playerGadgets.Sort((g1, g2) => g1.ScriptableObject.Rare.CompareTo(g2.ScriptableObject.Rare));
            }
        }
        else
        {
            playerGadget.AddAmount(gadget.GetAmount());
        }

        DataSaver.SaveData();
    }

    public static void AddCharacteristic(CharacteristicType characteristicType, int amount)
    {
        switch (characteristicType)
        {
            case CharacteristicType.Climb:
                _playerAscend += amount;
                break;
            case CharacteristicType.Swim:
                _playerDive += amount;
                break;
            case CharacteristicType.Fly:
                _playerGlide += amount;
                break;
            case CharacteristicType.Run:
                _playerRace += amount;
                break;
        }

        DataSaver.SaveData();
    }

    public static int GetUpgradeAmount(ChunkType chunkType)
    {
        switch (chunkType)
        {
            case ChunkType.Ground:
                return PlayerRace;
            case ChunkType.Water:
                return PlayerDive;
            case ChunkType.Wall:
                return PlayerAscend;
            case ChunkType.Fly:
                return PlayerGlide;
            default:
                return 0;
        }
    }

    private static void SortRewards(List<Reward> rewards)
    {
        int coins = 0;
        List<CoinsReward> coinsRewards = rewards.Select(r => r as CoinsReward).Where(r => r != null).ToList();
        coinsRewards.ForEach(r => coins += r.Amount);
        CoinsReward coinsReward = new(coins);

        int diamonds = 0;
        List<DiamondsReward> diamondsRewards = rewards.Select(r => r as DiamondsReward).Where(r => r != null).ToList();
        diamondsRewards.ForEach(r => diamonds += r.Amount);
        DiamondsReward diamondsReward = new(diamonds);

        List<CharacteristicReward> characteristicRewards = rewards.Select(r => r as CharacteristicReward).Where(r => r != null).ToList();

        List<GadgetReward> gadgetRewards = rewards.Select(r => r as GadgetReward).Where(r => r != null).ToList();
        if (gadgetRewards.Count > 1) gadgetRewards.Sort((r1, r2) => r1.ScriptableObject.Rare.CompareTo(r2.ScriptableObject.Rare));

        List<ChestReward> chestRewards = rewards.Select(r => r as ChestReward).Where(r => r != null).ToList();
        if (chestRewards.Count > 1) chestRewards.Sort((r1, r2) => r1.Type.CompareTo(r2.Type));

        rewards.Clear();
        
        if (coins > 0) rewards.Add(coinsReward);
        if (diamonds > 0) rewards.Add(diamondsReward);
        if (characteristicRewards != null && characteristicRewards.Count > 0) rewards.AddRange(characteristicRewards);
        if (gadgetRewards != null && gadgetRewards.Count > 0) rewards.AddRange(gadgetRewards);
        if (chestRewards != null && chestRewards.Count > 0) rewards.AddRange(chestRewards);
    }

    private static void AddGadget(GadgetSaveInfo gadgetSaveInfo)
    {
        Gadget gadget = new(GadgetSettings.Instance.GetGadgetByName(gadgetSaveInfo.GadgetName),
            gadgetSaveInfo.Amount);

        AddGadget(gadget);
    }

    private static void AddCharacteristic(CharacteristicSaveInfo characteristicType)
    {
        CharacteristicType type = (CharacteristicType)characteristicType.TypeInt;
        AddCharacteristic(type, characteristicType.Amount);
    }

    private static bool TryToBuyWithCoins(ShopItem shopItem)
    {
        if (shopItem.CurrencyType == ShopItem.Currency.Coin && _coins >= shopItem.Price)
        {
            _coins -= shopItem.Price;

            foreach (ShopReward reward in shopItem.Rewards)
            {
                reward.ApplyReward();
            }

            OnCoinsChanged?.Invoke();
            DataSaver.SaveData();

            return true;
        }

        return false;
    }

    private static bool TryToBuyWithDiamonds(ShopItem shopItem)
    {
        if (shopItem.CurrencyType == ShopItem.Currency.Diamond && _diamonds >= shopItem.Price)
        {
            _diamonds -= shopItem.Price;

            foreach (ShopReward reward in shopItem.Rewards)
            {
                reward.ApplyReward();
            }

            OnDiamondsChanged?.Invoke();
            DataSaver.SaveData();

            return true;
        }

        return false;
    }

    private static bool TryToBuyWithReal(ShopItem shopItem)
    {
        if (shopItem.CurrencyType == ShopItem.Currency.USD)
        {
            foreach (ShopReward reward in shopItem.Rewards)
            {
                reward.ApplyReward();
            }

            DataSaver.SaveData();

            return true;
        }

        return false;
    }

    private static bool TryToBuyWithADS(ShopItem shopItem)
    {
        if (shopItem.CurrencyType == ShopItem.Currency.ADS)
        {
            foreach (ShopReward reward in shopItem.Rewards)
            {
                if (reward is TicketsShopReward)
                {
                    if (_tickets < GlobalSettings.Instance.MaxTickets)
                    {
                        reward.ApplyReward();
                        DataSaver.SaveData();

                        return true;
                    }
                }
                else
                {
                    reward.ApplyReward();
                    DataSaver.SaveData();

                    return true;
                }
            }
        }

        return false;
    }

    private static int GetShopSeed() => (int)DateTime.Now.Ticks;
}
