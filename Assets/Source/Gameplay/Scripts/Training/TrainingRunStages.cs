using UnityEngine;

public class TrainingRunStages : RunStagesBase
{
	[Header("Countdown")]
	[SerializeField] private Countdown _countdown;
    
	override protected void ActiveSpeedGame()
	{
		base.ActiveSpeedGame();
		_countdown.Run(showCount: false);
	}

    protected override void GiveReward(int placement)
    {
        PlayerData.TrainingPassed();

        if (PlayerData.PassedTrainings == GlobalSettings.Instance.TrainingLevelsCount)
        {
            PlayerData.AddReward(GlobalSettings.Instance.TrainingGadgetReward);
        }
    }
}
