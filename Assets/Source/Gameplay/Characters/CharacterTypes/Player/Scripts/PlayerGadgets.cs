public class PlayerGadgets : CharacterGadgets
{
    protected override void Awake()
    {
        base.Awake();
        
        Gadget gadget = RunSettings.PlayerGadget;
        Init(gadget);
        CharacterAnimation characterAnimation = GetComponent<CharacterAnimation>();
        characterAnimation.Initialize(gadget);
    }
}
