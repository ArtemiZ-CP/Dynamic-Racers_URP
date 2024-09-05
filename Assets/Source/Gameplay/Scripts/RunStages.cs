using UnityEngine;

public class RunStages : RunStagesBase
{
	[Header("Countdown")]
	[SerializeField] private Countdown _countdown;

	override protected void ActiveSpeedGame()
	{
		base.ActiveSpeedGame();
		_countdown.Run();
	}

	override protected void GiveReward()
	{
		PlayerData.AddExperience(RunSettings.ExperienceReward);
		RunSettings.Reset();
	}
}
