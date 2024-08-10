using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(IAPManager))]
public class Shop : MonoBehaviour
{
    #region Constants
    private const float ChanceToGetLegendaryDailyOffer = 0.05f;
    private const int MinLegendaryDailyOfferAmount = 0;
    private const int MaxLegendaryDailyOfferAmount = 1;

    private const float ChanceToGetEpicDailyOffer = 0.2f;
    private const int MinEpicDailyOfferAmount = 0;
    private const int MaxEpicDailyOfferAmount = 1;

    private const float ChanceToGetRareDailyOffer = 0.3f;
    private const int MinRareDailyOfferAmount = 1;
    private const int MaxRareDailyOfferAmount = 3;

    private const float ChanceToGetDiamondsFreeOffer = 0.1f;
    private const float ChanceToGetCoinsFreeOffer = 0.5f;

    private const float ChanceToGetChestOffer = 0.5f;
    #endregion

    #region Serializable Offers
    [Serializable]
    public class GadgetOfferInfo
    {
        public Rare Rare;
        public int GoldCost;
        public int DiamondCost;
        public int[] GadgetsCountVariations;

        public int GetGadgetsCount(bool isOfferFree, out int cost, out ShopItem.Currency currency)
        {
            int index = 0;

            if (isOfferFree == false)
            {
                index = UnityEngine.Random.Range(0, GadgetsCountVariations.Length);
            }

            int gadgetsAmount = GadgetsCountVariations[index];

            if (DiamondCost > 0 && UnityEngine.Random.value < 0.5f)
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
        public string Name;
        public ShopOffer Offer;
        public int Price;
        public ShopItem.Currency Currency;
        public int RewardAmount;
        public Sprite RewardSprite;
    }

    [Serializable]
    public class ShopPermanentChestOffer
    {
        public string Name;
        public ShopOffer Offer;
        public int Price;
        public ShopItem.Currency Currency;
        public BoxReward.ChestType ChestType;
    }
    #endregion

    [SerializeField] private ShopOffer _bigOfferPrefab;
    [Header("Daily Offers")]
    [SerializeField] private ShopDailyOffer _diamondsDailyOffer;
    [SerializeField] private ShopDailyOffer _coinsDailyOffer;
    [SerializeField] private ShopOffer[] _dailyOffers;
    [SerializeField] private GadgetOfferInfo[] _gadgetOfferInfo;
    [SerializeField, Min(0)] private int _bigChestPriceInDiamonds;
    [SerializeField, Min(0)] private int _legendaryChestPriceInDiamonds;
    [Header("Permanent Offers")]
    [SerializeField] private ShopPermanentChestOffer[] _chestOffers;
    [SerializeField] private ShopPermanentOffer[] _diamondsOffers;
    [SerializeField] private ShopPermanentOffer[] _goldsOffers;
    [SerializeField] private ShopPermanentOffer[] _ticketsOffers;

    private IAPManager _iapManager;
    private GlobalSettings _globalSettings;

    private void Awake()
    {
        _globalSettings = GlobalSettings.Instance;
        _iapManager = GetComponent<IAPManager>();
        SetOffers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetOffers();
        }
    }

    private void SetOffers()
    {
        SetDailyOffers();
        SetPermanentOffers();
    }

    private void SetDailyOffers()
    {
        GetDailyOfferRareness(out int rare, out int epic, out int legendary);
        int offerIndex = _dailyOffers.Length - 1;

        for (int i = 0; i < legendary; i++)
        {
            SetDailyOffer(Rare.Legendary, offerIndex);
            offerIndex--;
        }

        for (int i = 0; i < epic; i++)
        {
            SetDailyOffer(Rare.Epic, offerIndex);
            offerIndex--;
        }

        for (int i = 0; i < rare; i++)
        {
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
            offer.Offer.SetOffer(offer.Name, offer.Price, offer.Currency, _globalSettings.GetChestSprite(offer.ChestType), chestReward);
        }

        foreach (ShopPermanentOffer offer in _diamondsOffers)
        {
            DiamondsShopReward diamondsReward = new(offer.RewardAmount);
            offer.Offer.SetOffer(offer.Name, offer.Price, offer.Currency, offer.RewardSprite, diamondsReward);
        }

        foreach (ShopPermanentOffer offer in _goldsOffers)
        {
            GoldsShopReward goldsReward = new(offer.RewardAmount);
            offer.Offer.SetOffer(offer.Name, offer.Price, offer.Currency, offer.RewardSprite, goldsReward);
        }

        foreach (ShopPermanentOffer offer in _ticketsOffers)
        {
            TicketsShopReward ticketsReward = new(offer.RewardAmount);
            offer.Offer.SetADSOffer(offer.Name, offer.RewardSprite, ticketsReward);
        }
    }

    private void GetDailyOfferRareness(out int rare, out int epic, out int legendary)
    {
        legendary = MinLegendaryDailyOfferAmount;
        epic = MinEpicDailyOfferAmount;
        rare = MinRareDailyOfferAmount;

        for (int i = 0; i < MaxLegendaryDailyOfferAmount - MinLegendaryDailyOfferAmount; i++)
        {
            if (UnityEngine.Random.value < ChanceToGetLegendaryDailyOffer)
            {
                legendary++;
            }
        }

        for (int i = 0; i < MaxEpicDailyOfferAmount - MinEpicDailyOfferAmount; i++)
        {
            if (UnityEngine.Random.value < ChanceToGetEpicDailyOffer)
            {
                epic++;
            }
        }

        for (int i = 0; i < MaxRareDailyOfferAmount - MinRareDailyOfferAmount; i++)
        {
            if (UnityEngine.Random.value < ChanceToGetRareDailyOffer)
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
        if (UnityEngine.Random.value < ChanceToGetDiamondsFreeOffer)
        {
            DiamondsShopReward reward = new(_diamondsDailyOffer.RewardAmount);
            _dailyOffers[offerIndex].SetFreeOffer(_diamondsDailyOffer.Name, _diamondsDailyOffer.RewardSprite, reward);
        }
        else if (UnityEngine.Random.value < ChanceToGetCoinsFreeOffer)
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
        if (UnityEngine.Random.value < ChanceToGetChestOffer)
        {
            SetChestOffer(offerIndex, 0, ShopItem.Currency.ADS, BoxReward.ChestType.Small);
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
            if (UnityEngine.Random.value < ChanceToGetChestOffer)
            {
                SetChestOffer(offerIndex, _bigChestPriceInDiamonds, ShopItem.Currency.Diamond, BoxReward.ChestType.Big);
            }
            else
            {
                SetGadgetOffer(offerIndex, rare);
            }
        }
        else
        {
            if (UnityEngine.Random.value < ChanceToGetChestOffer)
            {
                SetChestOffer(offerIndex, _legendaryChestPriceInDiamonds, ShopItem.Currency.Diamond, BoxReward.ChestType.Legendary);
            }
            else
            {
                SetGadgetOffer(offerIndex, rare);
            }
        }
    }

    private void SetFreeGadgetOffer(int offerIndex)
    {
        GadgetScriptableObject gadget = _globalSettings.GetRandomGadget(Rare.Common);
        GadgetOfferInfo gadgetOfferInfo = _gadgetOfferInfo.First(g => g.Rare == gadget.Rare);
        int gadgetsAmount = gadgetOfferInfo.GetGadgetsCount(isOfferFree: true, out _, out _);
        GadgetShopReward gadgetShopReward = new(gadget, gadgetsAmount);

        _dailyOffers[offerIndex].SetFreeOffer(gadget.Name, gadget.Sprite, gadgetShopReward);
    }

    private void SetADSGadgetOffer(int offerIndex)
    {
        GadgetScriptableObject gadget = _globalSettings.GetRandomGadget(Rare.Common);
        GadgetOfferInfo gadgetOfferInfo = _gadgetOfferInfo.First(g => g.Rare == gadget.Rare);
        int gadgetsAmount = gadgetOfferInfo.GetGadgetsCount(isOfferFree: true, out _, out _);
        GadgetShopReward gadgetShopReward = new(gadget, gadgetsAmount);

        _dailyOffers[offerIndex].SetADSOffer(gadget.Name, gadget.Sprite, gadgetShopReward, isInfinityToSell: false);
    }

    private void SetGadgetOffer(int offerIndex, Rare rare)
    {
        GadgetScriptableObject gadget = _globalSettings.GetRandomGadget(rare);
        GadgetOfferInfo gadgetOfferInfo = _gadgetOfferInfo.First(g => g.Rare == gadget.Rare);
        int gadgetsAmount = gadgetOfferInfo.GetGadgetsCount(isOfferFree: false, out int price, out ShopItem.Currency currency);
        GadgetShopReward gadgetShopReward = new(gadget, gadgetsAmount);

        _dailyOffers[offerIndex].SetOffer(gadget.Name, price, currency, gadget.Sprite, gadgetShopReward, isInfinityToSell: false);
    }
    
    private void SetChestOffer(int offerIndex, int price, ShopItem.Currency currency, BoxReward.ChestType chestType)
    {
        Sprite chestSprite = _globalSettings.GetChestSprite(chestType);
        ChestShopReward chestShopReward = new(chestType);
        _dailyOffers[offerIndex].SetOffer($"{chestType} Chest", price, currency, chestSprite, chestShopReward, isInfinityToSell: false);
    }
}
