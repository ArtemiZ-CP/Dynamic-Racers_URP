using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static IReadOnlyCollection<BoxReward> BoxRewardQueue => _boxRewardQueue;
    public static IReadOnlyCollection<BagReward> BagRewardQueue => _bagRewardQueue;
    public static IReadOnlyList<Gadget> PlayerGadgets => _playerGadgets;
    public static bool IsRewardQueueEmpty => _boxRewardQueue.Count == 0 && _bagRewardQueue.Count == 0;
    public static int Experience => _experience;
    public static int Level => _level;
    public static int Coins => _coins;
    public static int Diamonds => _diamonds;
    public static int Tickets => _tickets;
    public static int PlayerRace => _playerRace;
    public static int PlayerDive => _playerDive;
    public static int PlayerAscend => _playerAscend;
    public static int PlayerGlide => _playerGlide;
    public static int PassedTrainings => _passedTrainings;
    public static int FPS => _fps;
    public static bool IsMusicOn => _isMusicOn;
    public static bool IsSoundsOn => _isSoundsOn;
    public static bool IsHapticOn => _isHapticOn;

    private static Queue<BoxReward> _boxRewardQueue = new();
    private static Queue<BagReward> _bagRewardQueue = new();
    private static List<Gadget> _playerGadgets = new();
    private static int _experience;
    private static int _level;
    private static int _coins;
    private static int _diamonds;
    private static int _tickets;
    private static int _playerRace;
    private static int _playerDive;
    private static int _playerAscend;
    private static int _playerGlide;
    private static int _passedTrainings;
    private static int _fps;
    private static bool _isMusicOn;
    private static bool _isSoundsOn;
    private static bool _isHapticOn;

    private static bool _unfreezeFPS = false;

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

    public static void UnfreezeFPS()
    {
        Application.targetFrameRate = 1000;
        _unfreezeFPS = true;
    }

    public static void LoadData(SaveData saveData)
    {
        if (saveData == null)
        {
            SetMusic(true);
            SetSounds(true);
            SetHaptic(true);

            SetFPS(GlobalSettings.Instance.MinFPS);

            return;
        }

        _playerGadgets = saveData.PlayerGadgets;
        _bagRewardQueue = saveData.BagRewardQueue;
        _boxRewardQueue = saveData.BoxRewardQueue;
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
        _isMusicOn = saveData.IsMusicOn;
        _isSoundsOn = saveData.IsSoundsOn;

        OnCoinsChanged?.Invoke();
        OnDiamondsChanged?.Invoke();
        OnTicketsChanged?.Invoke();
    }

    public static void SetMusic(bool isMusicOn)
    {
        _isMusicOn = isMusicOn;
        OnMusicChanged?.Invoke();
    }

    public static void SetSounds(bool isSoundsOn)
    {
        _isSoundsOn = isSoundsOn;
        OnSoundsChanged?.Invoke();
    }

    public static void SetHaptic(bool isHapticOn)
    {
        _isHapticOn = isHapticOn;
        OnHapticChanged?.Invoke();
    }

    public static void SetFPS(int fps)
    {
        if (_unfreezeFPS) return;

        _fps = fps;
        Application.targetFrameRate = fps;
    }

    public static void TrainingPassed()
    {
        _passedTrainings++;
    }

    public static bool TryToBuy(ShopItem shopItem)
    {
        if (TryToBuyWithCoins(shopItem)) return true;
        else if (TryToBuyWithDiamonds(shopItem)) return true;
        else if (TryToBuyWithReal(shopItem)) return true;
        else if (TryToBuyWithADS(shopItem)) return true;

        return false;
    }

    public static bool TryToSpendTicket()
    {
        if (_tickets > 0)
        {
            _tickets--;
            OnTicketsChanged?.Invoke();
            return true;
        }

        return false;
    }

    public static void AddCoins(int coins)
    {
        _coins += coins;
        OnCoinsChanged?.Invoke();
    }

    public static void AddDiamonds(int diamonds)
    {
        _diamonds += diamonds;
        OnDiamondsChanged?.Invoke();
    }

    public static void AddTickets(int tickets)
    {
        _tickets += tickets;
        OnTicketsChanged?.Invoke();
    }

    public static void AddReward(RewardContainer reward)
    {
        if (reward is BoxReward boxReward)
        {
            _boxRewardQueue.Enqueue(boxReward);
        }
        else if (reward is BagReward bagReward)
        {
            _bagRewardQueue.Enqueue(bagReward);
        }
    }

    public static RewardContainer GetReward()
    {
        if (_boxRewardQueue.Count > 0)
        {
            return _boxRewardQueue.Dequeue();
        }

        if (_bagRewardQueue.Count > 0)
        {
            return _bagRewardQueue.Dequeue();
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
    }

    public static void AddGadget(Gadget gadget)
    {
        Gadget playerGadget = _playerGadgets.Find(g => g.GadgetScriptableObject == gadget.GadgetScriptableObject);

        if (playerGadget == null)
        {
            _playerGadgets.Add(new Gadget(gadget));
            _playerGadgets.Sort((g1, g2) => g1.GadgetScriptableObject.Rare.CompareTo(g2.GadgetScriptableObject.Rare));
        }
        else
        {
            playerGadget.AddAmount(gadget.GetAmount());
        }
    }

    public static void AddCharacteristic(CharacteristicType characteristicType, int amount)
    {
        switch (characteristicType)
        {
            case CharacteristicType.Ascend:
                _playerAscend += amount;
                break;
            case CharacteristicType.Dive:
                _playerDive += amount;
                break;
            case CharacteristicType.Glide:
                _playerGlide += amount;
                break;
            case CharacteristicType.Race:
                _playerRace += amount;
                break;
        }
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
            shopItem.Reward.ApplyReward();
            OnCoinsChanged?.Invoke();

            return true;
        }

        return false;
    }

    private static bool TryToBuyWithDiamonds(ShopItem shopItem)
    {
        if (shopItem.CurrencyType == ShopItem.Currency.Diamond && _diamonds >= shopItem.Price)
        {
            _diamonds -= shopItem.Price;
            shopItem.Reward.ApplyReward();
            OnDiamondsChanged?.Invoke();

            return true;
        }

        return false;
    }

    private static bool TryToBuyWithReal(ShopItem shopItem)
    {
        if (shopItem.CurrencyType == ShopItem.Currency.Real)
        {
            shopItem.Reward.ApplyReward();

            return true;
        }

        return false;
    }

    private static bool TryToBuyWithADS(ShopItem shopItem)
    {
        if (shopItem.CurrencyType == ShopItem.Currency.ADS)
        {
            if (shopItem.Reward is TicketsShopReward)
            {
                if (_tickets < GlobalSettings.Instance.MaxTickets)
                {
                    shopItem.Reward.ApplyReward();

                    return true;
                }
            }
            else
            {
                shopItem.Reward.ApplyReward();

                return true;
            }
        }

        return false;
    }
}
