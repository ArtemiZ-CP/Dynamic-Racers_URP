public class ShopItem
{
    public enum Currency
    {
        Diamond,
        Coin,
        Real,
        ADS
    }

    private int _price;
    private Currency _currency;
    private ShopReward _reward;

    public int Price => _price;
    public Currency CurrencyType => _currency;
    public ShopReward Reward => _reward;

    public ShopItem(int price, Currency currency, ShopReward reward)
    {
        _price = price;
        _currency = currency;
        _reward = reward;
    }
}
