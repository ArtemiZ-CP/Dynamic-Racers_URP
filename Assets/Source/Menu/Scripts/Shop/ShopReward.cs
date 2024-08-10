public abstract class ShopReward
{
    public int Amount { get; protected set; }

    public abstract void ApplyReward();
}
