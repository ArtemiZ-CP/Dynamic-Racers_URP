public class TicketsShopReward : ShopReward
{
    public TicketsShopReward(int tickets)
    {
        Amount = tickets;
    }

    public override void ApplyReward()
    {
        PlayerData.AddTickets(Amount);
    }
}
