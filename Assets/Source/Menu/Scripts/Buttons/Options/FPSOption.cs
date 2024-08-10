public class FPSOption : OptionButton
{
    private GlobalSettings _globalSettings;

    protected override void TurnOn()
    {
        base.TurnOn();
        PlayerData.SetFPS(_globalSettings.MaxFPS);
    }

    protected override void TurnOff()
    {
        PlayerData.SetFPS(_globalSettings.MinFPS);
    }

    private void Awake()
    {
        _globalSettings = GlobalSettings.Instance;
    }

    private void OnEnable()
    {
        if (PlayerData.FPS == _globalSettings.MaxFPS)
        {
            base.TurnOn();
        }
        else
        {
            base.TurnOff();
        }
    }
}
