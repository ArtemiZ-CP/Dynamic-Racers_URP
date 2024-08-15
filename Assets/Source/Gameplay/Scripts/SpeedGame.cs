public class SpeedGame : SpeedGameBase
{
	override protected void Start()
	{
		base.Start();
		Invoke(nameof(StartRunning), GlobalSettings.Instance.TimeToStartRun);
        ShowHint();
	}
}
