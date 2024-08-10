public class GoldsShopReward : ShopReward
{
    public GoldsShopReward(int coins)
    {
        Amount = coins;
    }

    public override void ApplyReward()
    {
        PlayerData.AddCoins(Amount);
    }
}
