using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopOffer : MonoBehaviour
{
    private const string DiamondIcon = "<sprite=\"Diamond\" index=0>";
    private const string CoinIcon = "<sprite=\"Coin\" index=0>";
    private const string RubIcon = "<sprite=\"Rub\" index=0>";
    private const string ADSIcon = "<sprite=\"ADS\" index=0>";

    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _button;
    [SerializeField] private Image _offerImage;
    [SerializeField] private GameObject _soldoutImage;

    private ShopItem _item;
    private bool _isInfinityToSell;

    public void SetFreeOffer(string name, Sprite sprite, ShopReward shopReward)
    {
        SetOffer(name, 0, ShopItem.Currency.Coin, sprite, shopReward, isInfinityToSell: false);
    }

    public void SetADSOffer(string name, Sprite sprite, ShopReward shopReward, bool isInfinityToSell = true)
    {
        SetOffer(name, 0, ShopItem.Currency.ADS, sprite, shopReward, isInfinityToSell);
    }

    public void SetOffer(string name, int price, ShopItem.Currency currency, Sprite sprite, ShopReward shopReward, bool isInfinityToSell = true)
    {
        ActiveOffer();
        _item = new ShopItem(price, currency, shopReward);

        _isInfinityToSell = isInfinityToSell;
        _nameText.text = name;
        _offerImage.sprite = sprite;

        SetAmountText(shopReward.Amount);
        SetPriceText(currency, price);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClickHandler);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClickHandler);
    }

    private void SetAmountText(int amount)
    {
        if (amount > 1)
        {
            _amountText.text = $"x{amount}";
        }
        else
        {
            _amountText.text = string.Empty;
        }
    }

    private void SetPriceText(ShopItem.Currency currency, int price)
    {
        if (currency == ShopItem.Currency.ADS)
        {
            _priceText.text = $"get:{ADSIcon}";
            return;
        }

        string currencyIcon = currency switch
        {
            ShopItem.Currency.Diamond => DiamondIcon,
            ShopItem.Currency.Coin => CoinIcon,
            ShopItem.Currency.Real => RubIcon,
            _ => string.Empty,
        };

        if (price == 0)
        {
            _priceText.text = "Free";
        }
        else
        {
            _priceText.text = $"{price}{currencyIcon}";
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
        _soldoutImage.SetActive(false);
        _nameText.gameObject.SetActive(true);
        _amountText.gameObject.SetActive(true);
        _priceText.gameObject.SetActive(true);
    }

    private void DisactiveOffer()
    {
        _button.interactable = false;
        _soldoutImage.SetActive(true);
        _nameText.gameObject.SetActive(false);
        _amountText.gameObject.SetActive(false);
        _priceText.gameObject.SetActive(false);
    }
}
