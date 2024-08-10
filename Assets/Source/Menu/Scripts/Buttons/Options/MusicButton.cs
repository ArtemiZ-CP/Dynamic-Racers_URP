public class MusicButton : OptionButton
{
    protected override void TurnOn()
    {
        base.TurnOn();
        PlayerData.SetMusic(true);
    }

    protected override void TurnOff()
    {
        base.TurnOff();
        PlayerData.SetMusic(false);
    }

    private void OnEnable()
    {
        if (PlayerData.IsMusicOn)
        {
            base.TurnOn();
        }
        else
        {
            base.TurnOff();
        }
    }
}
