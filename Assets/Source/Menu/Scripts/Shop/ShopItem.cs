using System;

public class ShopItem
{
    public enum Currency
    {
        Diamond,
        Coin,
        USD,
        ADS
    }

    private int _price;
    private float _fiatPrice;
    private Currency _currency;
    private ShopReward[] _rewards;

    public int Price => _price;
    public float FiatPrice => _fiatPrice;
    public Currency CurrencyType => _currency;
    public ShopReward[] Rewards => _rewards;

    public ShopItem(float price, ShopReward reward)
    {
        _fiatPrice = price;
        _currency = Currency.USD;
        _rewards = new ShopReward[] { reward };
    }

    public ShopItem(float price, ShopReward[] rewards)
    {
        _fiatPrice = price;
        _currency = Currency.USD;
        _rewards = rewards;
    }

    public ShopItem(int price, Currency currency, ShopReward reward)
    {
        _price = price;
        _currency = currency;
        _rewards = new ShopReward[] { reward };
    }

    public ShopItem(int price, Currency currency, ShopReward[] rewards)
    {
        _price = price;
        _currency = currency;
        _rewards = rewards;
    }
}
