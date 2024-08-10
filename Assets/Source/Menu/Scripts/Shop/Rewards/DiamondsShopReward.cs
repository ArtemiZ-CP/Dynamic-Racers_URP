public class DiamondsShopReward : ShopReward
{
    public DiamondsShopReward(int diamonds)
    {
        Amount = diamonds;
    }

    public override void ApplyReward()
    {
        PlayerData.AddDiamonds(Amount);
    }
}
