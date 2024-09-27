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

        PlayerData.AddReward(GlobalSettings.Instance.TrainingGadgetReward);
    }
}
