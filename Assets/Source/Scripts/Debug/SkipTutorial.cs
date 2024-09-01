using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
    public void Skip()
    {
        for (int i = 0; i < GlobalSettings.Instance.TrainingLevelsCount; i++)
        {
            PlayerData.TrainingPassed();
        }
    }
}
