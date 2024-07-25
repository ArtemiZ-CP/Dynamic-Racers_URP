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

   override protected int GetEnemyUpgrades(CharacteristicType characteristicType)
    {
		return 0;
    }

    override protected void GiveReward()
	{
		PlayerProgress.AddExperience(RunSettings.ExperienceReward);
		RunSettings.Reset();
	}
}
