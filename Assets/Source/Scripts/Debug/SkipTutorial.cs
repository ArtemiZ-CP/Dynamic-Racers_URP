using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
    public void Skip()
    {
        if (PlayerData.PassedTrainings >= GlobalSettings.Instance.TrainingLevelsCount)
        {
            return;
        }

        for (int i = 0; i < GlobalSettings.Instance.TrainingLevelsCount; i++)
        {
            PlayerData.TrainingPassed();
        }

        List<Gadget> gadgets = GlobalSettings.Instance.GetAllGadgets().Where(g => g.ScriptableObject.Rare == Rare.Common).ToList();
        List<GadgetReward> gadgetRewards = gadgets.Select(g => new GadgetReward(g.ScriptableObject, 1)).ToList();
        PlayerData.AddReward(new ChestReward(
            ChestReward.ChestType.Wood,
            gadgetRewards,
            null, null, null));
    }
}
