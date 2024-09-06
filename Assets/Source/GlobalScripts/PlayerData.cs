using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlayerData
{
    public static IReadOnlyCollection<ChestReward> BoxRewardQueue => _boxRewardQueue;
    public static bool IsRewardQueueEmpty => _boxRewardQueue.Count == 0;
    public static IReadOnlyList<OpeningChest> OpeningChests => _openingChests;
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

    private static Queue<ChestReward> _boxRewardQueue = new();
    private static OpeningChest[] _openingChests = new OpeningChest[4];
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
        GlobalSettings globalSettings = GlobalSettings.Instance;

        if (saveData.BoxRewardQueue == null)
        {
            _boxRewardQueue = new Queue<ChestReward>();
        }
        else
        {
            _boxRewardQueue = new Queue<ChestReward>(saveData.BoxRewardQueue.Select(chestReward => new ChestReward(
                (ChestReward.ChestType)chestReward.ChestTypeInt,
                chestReward.GadgetRewards.Select(gadget => new GadgetReward(globalSettings.GetGadgetByName(gadget.GadgetName), gadget.Amount)).ToList(),
                chestReward.CharacteristicRewards.Select(sharacteristicReward => new CharacteristicReward((CharacteristicType)sharacteristicReward.TypeInt, sharacteristicReward.Amount)).ToList(),
                chestReward.CoinsReward)));
        }

        if (saveData.PlayerGadgets == null)
        {
            saveData.PlayerGadgets = new PlayerGadgetSaveInfo[0];

        }
        else
        {
            _playerGadgets = saveData.PlayerGadgets.Select(gadget => new Gadget(
                globalSettings.GetGadgetByName(gadget.GadgetName), gadget.Amount, gadget.Level)).ToList();
        }

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

    public static void AddReward(ChestReward reward)
    {
        _boxRewardQueue.Enqueue(reward);

        DataSaver.SaveData();
    }

    public static ChestReward GetReward()
    {
        if (_boxRewardQueue.Count > 0)
        {
            return _boxRewardQueue.Dequeue();
        }

        return null;
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
            _playerGadgets.Add(new Gadget(gadget, GlobalSettings.Instance.GetStartGadgetLevel(gadget)));

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
