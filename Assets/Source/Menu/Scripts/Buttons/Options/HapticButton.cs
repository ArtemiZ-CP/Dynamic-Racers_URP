public class HapticButton : OptionButton
{
    protected override void TurnOn()
    {
        base.TurnOn();
        PlayerData.SetHaptic(true);
    }

    protected override void TurnOff()
    {
        base.TurnOff();
        PlayerData.SetHaptic(false);
    }

    private void OnEnable()
    {
        if (PlayerData.IsHapticOn)
        {
            base.TurnOn();
        }
        else
        {
            base.TurnOff();
        }
    }
}
