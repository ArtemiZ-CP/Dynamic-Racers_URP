using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopOffer : MonoBehaviour
{
    private const string DiamondIcon = "<sprite=\"Diamond\" index=0>";
    private const string CoinIcon = "<sprite=\"Coin\" index=0>";
    private const string RubIcon = "<sprite=\"Rub\" index=0>";
    private const string ADSIcon = "<sprite=\"ADS\" index=0>";

    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private TMP_Text _lastPriceText;
    [SerializeField] private TMP_Text _saleText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Image _offerImage;
    [SerializeField] private GameObject[] _soldoutImages;
    [SerializeField] private GameObject[] _imagesToHideOnSoldout;

    private Button _button;
    private ShopItem _item;
    private bool _isInfinityToSell;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClickHandler);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClickHandler);
    }

    public void SetPersonalOffer(float salePersantage, float price, float lastPrice, Sprite sprite, ShopReward[] shopRewards)
    {
        ActiveOffer();
        _item = new ShopItem(price, shopRewards);

        _offerImage.sprite = sprite;

        _nameText.text = $"{(int)(salePersantage * 100)}%\nSale";

        SetPriceText(ShopItem.Currency.USD, lastPrice, _lastPriceText);
        SetPriceText(ShopItem.Currency.USD, price, _priceText);
    }

    public void SetFreeOffer(string name, Sprite sprite, ShopReward shopReward)
    {
        SetOffer(name, 0, ShopItem.Currency.Coin, sprite, shopReward, isInfinityToSell: false);
    }

    public void SetADSOffer(string name, Sprite sprite, ShopReward shopReward, bool isInfinityToSell = true)
    {
        SetOffer(name, 0, ShopItem.Currency.ADS, sprite, shopReward, isInfinityToSell);
    }

    public void SetOffer(string name, float price, Sprite sprite, ShopReward shopReward, bool isInfinityToSell = true)
    {
        ActiveOffer();
        _item = new ShopItem(price, shopReward);

        _isInfinityToSell = isInfinityToSell;

        if (_offerImage != null)
        {
            _offerImage.sprite = sprite;
        }

        if (_nameText != null)
        {
            _nameText.text = name;
        }

        SetAmountText(shopReward.Amount);
        SetPriceText(ShopItem.Currency.USD, price, _priceText);
    }

    public void SetOffer(string name, int price, ShopItem.Currency currency, Sprite sprite, ShopReward shopReward, bool isInfinityToSell = true)
    {
        ActiveOffer();
        _item = new ShopItem(price, currency, shopReward);

        _isInfinityToSell = isInfinityToSell;

        if (_offerImage != null)
        {
            _offerImage.sprite = sprite;
        }

        if (_nameText != null)
        {
            _nameText.text = name;
        }

        SetAmountText(shopReward.Amount);
        SetPriceText(currency, price, _priceText);
    }

    private void SetAmountText(int amount)
    {
        if (_amountText == null)
        {
            return;
        }

        if (amount > 1)
        {
            _amountText.text = $"x{amount}";
        }
        else
        {
            _amountText.text = string.Empty;
        }
    }

    private void SetPriceText(ShopItem.Currency currency, float price, TMP_Text priceText)
    {
        if (currency == ShopItem.Currency.ADS)
        {
            priceText.text = $"get:{ADSIcon}";
            return;
        }

        if (currency == ShopItem.Currency.USD)
        {
            priceText.text = $"${price}";
            return;
        }

        string currencyIcon = currency switch
        {
            ShopItem.Currency.Diamond => DiamondIcon,
            ShopItem.Currency.Coin => CoinIcon,
            _ => string.Empty,
        };

        if (price == 0)
        {
            priceText.text = "Free";
        }
        else
        {
            priceText.text = $"{price}{currencyIcon}";
        }
    }

    private void OnClickHandler()
    {
        if (PlayerData.TryToBuy(_item) && _isInfinityToSell == false)
        {
            DisactiveOffer();
        }
    }

    private void ActiveOffer()
    {
        _button.interactable = true;

        foreach (var image in _imagesToHideOnSoldout)
        {
            image.SetActive(true);
        }

        foreach (var soldoutImage in _soldoutImages)
        {
            soldoutImage.SetActive(false);
        }
    }

    private void DisactiveOffer()
    {
        _button.interactable = false;

        foreach (var image in _imagesToHideOnSoldout)
        {
            image.SetActive(false);
        }

        foreach (var soldoutImage in _soldoutImages)
        {
            soldoutImage.SetActive(true);
        }
    }
}
