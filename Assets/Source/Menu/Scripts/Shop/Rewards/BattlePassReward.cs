public class BattlePassReward : ShopReward
{
    public override void ApplyReward()
    {
        PlayerData.BuyBattlePass();
    }
}
