public class ChestShopReward : ShopReward
{
    public BoxReward Chests { get; private set; }

    public ChestShopReward(BoxReward.ChestType chestsType)
    {
        Chests = new(chestsType);;
    }

    public override void ApplyReward()
    {
        PlayerData.AddReward(Chests);
    }
}
