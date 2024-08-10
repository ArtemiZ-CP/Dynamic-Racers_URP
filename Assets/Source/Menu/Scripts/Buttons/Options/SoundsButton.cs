public class SoundsButton : OptionButton
{
    protected override void TurnOn()
    {
        base.TurnOn();
        PlayerData.SetSounds(true);
    }

    protected override void TurnOff()
    {
        base.TurnOff();
        PlayerData.SetSounds(false);
    }

    private void OnEnable()
    {
        if (PlayerData.IsSoundsOn)
        {
            base.TurnOn();
        }
        else
        {
            base.TurnOff();
        }
    }
}
