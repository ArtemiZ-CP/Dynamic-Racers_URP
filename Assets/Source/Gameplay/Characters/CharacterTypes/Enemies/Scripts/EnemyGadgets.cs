public class EnemyGadgets : CharacterGadgets
{
    protected override void Awake()
    {
        base.Awake();

        if (RunSettings.PlayerGadget != null)
        {
            Init(new Gadget(GlobalSettings.Instance.GetRandomGadget(), level: RunSettings.PlayerGadget.Level));
        }
    }
}
