using System.Collections.Generic;
using System.Linq;
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
            List<Gadget> gadgets = GlobalSettings.Instance.GetAllGadgets().Where(g => g.ScriptableObject.Rare == Rare.Common).ToList();
            List<GadgetReward> gadgetRewards = gadgets.Select(g => new GadgetReward(g.ScriptableObject, 1)).ToList();
            PlayerData.AddRunReward(new ChestReward(
                ChestReward.ChestType.Wood, 
                gadgetRewards,
                null, null, null), placement);
        }
    }
}
