public class ChestShopReward : ShopReward
{
    public ChestReward Chests { get; private set; }

    public ChestShopReward(ChestReward.ChestType chestsType, int amount = 1)
    {
        Chests = new ChestReward(chestsType);
        Amount = amount;
    }

    public override void ApplyReward()
    {
        for (int i = 0; i < Amount; i++)
        {
            PlayerData.AddReward(Chests);
        }
    }
}
