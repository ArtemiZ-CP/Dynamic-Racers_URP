public class GadgetShopReward : ShopReward
{
    public Gadget Gadget { get; private set; }

    public GadgetShopReward(GadgetScriptableObject gadge, int amount)
    {
        Gadget = new Gadget(gadge, amount);
        Amount = amount;
    }

    public override void ApplyReward()
    {
        PlayerData.AddGadget(Gadget);
    }
}
