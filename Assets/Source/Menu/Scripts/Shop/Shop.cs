using System;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(IAPManager))]
public class Shop : MonoBehaviour
{
    #region Constants
    public const float ChanceToGetLegendaryDailyOffer = 0.05f;
    public const int MinLegendaryDailyOfferAmount = 0;
    public const int MaxLegendaryDailyOfferAmount = 1;

    public const float ChanceToGetEpicDailyOffer = 0.2f;
    public const int MinEpicDailyOfferAmount = 0;
    public const int MaxEpicDailyOfferAmount = 1;

    public const float ChanceToGetRareDailyOffer = 0.3f;
    public const int MinRareDailyOfferAmount = 1;
    public const int MaxRareDailyOfferAmount = 3;

    public const float ChanceToGetDiamondsFreeOffer = 0.1f;
    public const float ChanceToGetCoinsFreeOffer = 0.5f;

    public const float ChanceToGetChestOffer = 0.5f;
    #endregion

    #region Serializable Offers
    [Serializable]
    public class GadgetOfferInfo
    {
        public Rare Rare;
        public int GoldCost;
        public int DiamondCost;
        public int[] GadgetsCountVariations;

        public int GetGadgetsCount(bool isOfferFree, System.Random random, out int cost, out ShopItem.Currency currency)
        {
            int index = 0;

            if (isOfferFree == false)
            {
                index = random.Next(GadgetsCountVariations.Length);
            }

            int gadgetsAmount = GadgetsCountVariations[index];

            if (DiamondCost > 0 && random.Next(1) < 0.5f)
            {
                currency = ShopItem.Currency.Diamond;
                cost = DiamondCost * gadgetsAmount;
            }
            else
            {
                currency = ShopItem.Currency.Coin;
                cost = GoldCost * gadgetsAmount;
            }

            return gadgetsAmount;
        }
    }

    [Serializable]
    public class ShopDailyOffer
    {
        public string Name;
        public int RewardAmount;
        public Sprite RewardSprite;
    }

    [Serializable]
    public class ShopPermanentOffer
    {
        public ShopOffer Offer;
        public int Price;
        public ShopItem.Currency Currency;
        public int RewardAmount;
        public Sprite RewardSprite;
    }

    [Serializable]
    public class ShopPermanentUSDOffer
    {
        public ShopOffer Offer;
        public float Price;
        public int RewardAmount;
        public Sprite RewardSprite;
    }

    [Serializable]
    public class ShopBattlePassOffer
    {
        public string Text;
        public ShopOffer Offer;
        public float Price;
    }

    [Serializable]
    public class ShopPersonalOffer
    {
        public int CoinsReward;
        public int DiamondsReward;
        public int TicketsReward;
        public ChestReward.ChestType ChestReward;
        public int ChestsCout;
        public Sprite RewardSprite;
        public float Price;
        public float LastPrice;
        [Range(0, 1f)] public float Discount;
    }

    [Serializable]
    public class ShopPermanentChestOffer
    {
        public string Name;
        public ShopOffer Offer;
        public int Price;
        public ShopItem.Currency Currency;
        public ChestReward.ChestType ChestType;
    }
    #endregion

    [Header("Timer")]
    [SerializeField] private TMP_Text _timer;
    [Header("Battle Pass")]
    [SerializeField] private GameObject _battlePassOfferLine;
    [SerializeField] private ShopBattlePassOffer _battlePassOfferInfo;
    [Header("Personal Offer")]
    [SerializeField] private GameObject _personalOfferLine;
    [SerializeField] private ShopOffer _personalOffer;
    [SerializeField] private ShopPersonalOffer _personalOfferInfo;
    [Header("Daily Offers")]
    [SerializeField] private ShopDailyOffer _diamondsDailyOffer;
    [SerializeField] private ShopDailyOffer _coinsDailyOffer;
    [SerializeField] private ShopOffer[] _dailyOffers;
    [SerializeField] private GadgetOfferInfo[] _gadgetOfferInfo;
    [SerializeField, Min(0)] private int _bigChestPriceInDiamonds;
    [SerializeField, Min(0)] private int _legendaryChestPriceInDiamonds;
    [Header("Permanent Offers")]
    [SerializeField] private ShopPermanentChestOffer[] _chestOffers;
    [SerializeField] private ShopPermanentUSDOffer[] _diamondsOffers;
    [SerializeField] private ShopPermanentUSDOffer[] _goldsOffers;
    [SerializeField] private ShopPermanentOffer[] _ticketsOffers;

    private IAPManager _iapManager;
    private GlobalSettings _globalSettings;
    private GadgetSettings _gadgetSettings;
    private System.Random _random;

    private GlobalSettings GlobalSettings
    {
        get
        {
            if (_globalSettings == null)
            {
                _globalSettings = GlobalSettings.Instance;
            }

            return _globalSettings;
        }
    }

    private GadgetSettings GadgetSettings
    {
        get
        {
            if (_gadgetSettings == null)
            {
                _gadgetSettings = GadgetSettings.Instance;
            }

            return _gadgetSettings;
        }
    }

    private void Awake()
    {
        _iapManager = GetComponent<IAPManager>();
    }

    private void Start()
    {
        UpdateOffers();
    }

    private void Update()
    {
        UpdateTimer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateOffers();
        }
    }

    private void UpdateTimer()
    {
        if (PlayerData.TryToUpdateShop(out TimeSpan timeToNextDay))
        {
            UpdateOffers();
        }

        _timer.text = timeToNextDay.ToString(@"hh\:mm\:ss");
    }

    private void UpdateOffers()
    {
        SetBattlePassOffer();
        SetPersonalOffer();
        SetPermanentOffers();
        SetDailyOffers();
    }

    private void SetPersonalOffer()
    {
        ShopReward[] rewards = new ShopReward[]
        {
            new GoldsShopReward(_personalOfferInfo.CoinsReward),
            new DiamondsShopReward(_personalOfferInfo.DiamondsReward),
            new TicketsShopReward(_personalOfferInfo.TicketsReward),
            new ChestShopReward(_personalOfferInfo.ChestReward, _personalOfferInfo.ChestsCout)
        };

        _personalOffer.SetPersonalOffer(
            _personalOfferInfo.Discount, _personalOfferInfo.Price, _personalOfferInfo.LastPrice,
            _personalOfferInfo.RewardSprite, rewards);
    }

    private void SetBattlePassOffer()
    {
        if (PlayerData.LastTimeBattlePassBought.AddDays(30) < DateTime.Now)
        {
            _battlePassOfferLine.SetActive(true);
            _battlePassOfferInfo.Offer.gameObject.SetActive(true);
            _battlePassOfferInfo.Offer.SetOffer(
                _battlePassOfferInfo.Text, _battlePassOfferInfo.Price,
                null, new BattlePassReward(), isInfinityToSell: false);
        }
        else
        {
            _battlePassOfferLine.SetActive(false);
            _battlePassOfferInfo.Offer.gameObject.SetActive(false);
        }
    }

    private void SetDailyOffers()
    {
        _random = new System.Random(PlayerData.ShopRandomSeed);

        GetDailyOfferRareness(out int rare, out int epic, out int legendary);
        int offerIndex = _dailyOffers.Length - 1;

        for (int i = 0; i < legendary; i++)
        {
            if (offerIndex < 0) break;

            SetDailyOffer(Rare.Legendary, offerIndex);
            offerIndex--;
        }

        for (int i = 0; i < epic; i++)
        {
            if (offerIndex < 0) break;

            SetDailyOffer(Rare.Epic, offerIndex);
            offerIndex--;
        }

        for (int i = 0; i < rare; i++)
        {
            if (offerIndex < 0) break;

            SetDailyOffer(Rare.Rare, offerIndex);
            offerIndex--;
        }

        for (; offerIndex >= 0; offerIndex--)
        {
            SetDailyOffer(Rare.Common, offerIndex);
        }
    }

    private void SetPermanentOffers()
    {
        foreach (ShopPermanentChestOffer offer in _chestOffers)
        {
            ChestShopReward chestReward = new(offer.ChestType);
            offer.Offer.SetOffer(offer.Name, offer.Price, offer.Currency, GlobalSettings.GetChestSprite(offer.ChestType), chestReward);
        }

        foreach (ShopPermanentUSDOffer offer in _diamondsOffers)
        {
            DiamondsShopReward diamondsReward = new(offer.RewardAmount);
            offer.Offer.SetOffer(string.Empty, offer.Price, offer.RewardSprite, diamondsReward);
        }

        foreach (ShopPermanentUSDOffer offer in _goldsOffers)
        {
            GoldsShopReward goldsReward = new(offer.RewardAmount);
            offer.Offer.SetOffer(string.Empty, offer.Price, offer.RewardSprite, goldsReward);
        }

        foreach (ShopPermanentOffer offer in _ticketsOffers)
        {
            TicketsShopReward ticketsReward = new(offer.RewardAmount);
            offer.Offer.SetOffer(string.Empty, offer.Price, offer.Currency, offer.RewardSprite, ticketsReward);
        }
    }

    private void GetDailyOfferRareness(out int rare, out int epic, out int legendary)
    {
        legendary = MinLegendaryDailyOfferAmount;
        epic = MinEpicDailyOfferAmount;
        rare = MinRareDailyOfferAmount;

        for (int i = 0; i < MaxLegendaryDailyOfferAmount - MinLegendaryDailyOfferAmount; i++)
        {
            if (_random.Next(1) < ChanceToGetLegendaryDailyOffer)
            {
                legendary++;
            }
        }

        for (int i = 0; i < MaxEpicDailyOfferAmount - MinEpicDailyOfferAmount; i++)
        {
            if (_random.Next(1) < ChanceToGetEpicDailyOffer)
            {
                epic++;
            }
        }

        for (int i = 0; i < MaxRareDailyOfferAmount - MinRareDailyOfferAmount; i++)
        {
            if (_random.Next(1) < ChanceToGetRareDailyOffer)
            {
                rare++;
            }
        }
    }

    private void SetDailyOffer(Rare rare, int offerIndex)
    {
        if (offerIndex == 0)
        {
            SetFreeOffer(offerIndex);
        }
        else if (offerIndex == 1)
        {
            SetADSOffer(offerIndex);
        }
        else
        {
            SetRareDailyOffer(rare, offerIndex);
        }
    }

    private void SetFreeOffer(int offerIndex)
    {
        if (_random.Next(1) < ChanceToGetDiamondsFreeOffer)
        {
            DiamondsShopReward reward = new(_diamondsDailyOffer.RewardAmount);
            _dailyOffers[offerIndex].SetFreeOffer(_diamondsDailyOffer.Name, _diamondsDailyOffer.RewardSprite, reward);
        }
        else if (_random.Next(1) < ChanceToGetCoinsFreeOffer)
        {
            GoldsShopReward reward = new(_coinsDailyOffer.RewardAmount);
            _dailyOffers[offerIndex].SetFreeOffer(_coinsDailyOffer.Name, _coinsDailyOffer.RewardSprite, reward);
        }
        else
        {
            SetFreeGadgetOffer(offerIndex);
        }
    }

    private void SetADSOffer(int offerIndex)
    {
        if (_random.Next(1) < ChanceToGetChestOffer)
        {
            SetChestOffer(offerIndex, 0, ShopItem.Currency.ADS, ChestReward.ChestType.Small);
        }
        else
        {
            SetADSGadgetOffer(offerIndex);
        }
    }

    private void SetRareDailyOffer(Rare rare, int offerIndex)
    {
        if (rare == Rare.Common || rare == Rare.Rare)
        {
            SetGadgetOffer(offerIndex, rare);
        }
        else if (rare == Rare.Epic)
        {
            if (_random.Next(1) < ChanceToGetChestOffer)
            {
                SetChestOffer(offerIndex, _bigChestPriceInDiamonds, ShopItem.Currency.Diamond, ChestReward.ChestType.Big);
            }
            else
            {
                SetGadgetOffer(offerIndex, rare);
            }
        }
        else
        {
            if (_random.Next(1) < ChanceToGetChestOffer)
            {
                SetChestOffer(offerIndex, _legendaryChestPriceInDiamonds, ShopItem.Currency.Diamond, ChestReward.ChestType.Legendary);
            }
            else
            {
                SetGadgetOffer(offerIndex, rare);
            }
        }
    }

    private void SetFreeGadgetOffer(int offerIndex)
    {
        GadgetScriptableObject gadget = GadgetSettings.GetRandomGadget(Rare.Common, _random);
        GadgetOfferInfo gadgetOfferInfo = _gadgetOfferInfo.First(g => g.Rare == gadget.Rare);
        int gadgetsAmount = gadgetOfferInfo.GetGadgetsCount(isOfferFree: true, _random, out _, out _);
        GadgetShopReward gadgetShopReward = new(gadget, gadgetsAmount);

        _dailyOffers[offerIndex].SetFreeOffer(gadget.Name, gadget.BigSprite, gadgetShopReward);
    }

    private void SetADSGadgetOffer(int offerIndex)
    {
        GadgetScriptableObject gadget = GadgetSettings.GetRandomGadget(Rare.Common, _random);
        GadgetOfferInfo gadgetOfferInfo = _gadgetOfferInfo.First(g => g.Rare == gadget.Rare);
        int gadgetsAmount = gadgetOfferInfo.GetGadgetsCount(isOfferFree: true, _random, out _, out _);
        GadgetShopReward gadgetShopReward = new(gadget, gadgetsAmount);

        _dailyOffers[offerIndex].SetADSOffer(gadget.Name, gadget.BigSprite, gadgetShopReward, isInfinityToSell: false);
    }

    private void SetGadgetOffer(int offerIndex, Rare rare)
    {
        GadgetScriptableObject gadget = GadgetSettings.GetRandomGadget(rare, _random);
        GadgetOfferInfo gadgetOfferInfo = _gadgetOfferInfo.First(g => g.Rare == gadget.Rare);
        int gadgetsAmount = gadgetOfferInfo.GetGadgetsCount(isOfferFree: false, _random, out int price, out ShopItem.Currency currency);
        GadgetShopReward gadgetShopReward = new(gadget, gadgetsAmount);

        _dailyOffers[offerIndex].SetOffer(gadget.Name, price, currency, gadget.BigSprite, gadgetShopReward, isInfinityToSell: false);
    }

    private void SetChestOffer(int offerIndex, int price, ShopItem.Currency currency, ChestReward.ChestType chestType)
    {
        Sprite chestSprite = GlobalSettings.GetChestSprite(chestType);
        ChestShopReward chestShopReward = new(chestType);
        _dailyOffers[offerIndex].SetOffer($"{chestType} Chest", price, currency, chestSprite, chestShopReward, isInfinityToSell: false);
    }
}
