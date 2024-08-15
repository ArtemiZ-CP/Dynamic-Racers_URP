public class PlayerGadgets : CharacterGadgets
{
    protected override void Awake()
    {
        base.Awake();
        Init(RunSettings.PlayerGadget);
    }
}
