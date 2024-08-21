using System.Collections.Generic;
using UnityEngine;

public class DebugGive : MonoBehaviour
{
    [Header("Start")]
    [SerializeField] private bool _skipTutorial;
    [Space(20)]
    [SerializeField] private int _XPToGiveOnStart;
    [Header("On Click")]
    [SerializeField] private CharacteristicType _characteristicType;
    [SerializeField, Min(1)] private int _value;
    [Space(20)]
    [SerializeField] private GadgetReward _gadgetReward;

    [ContextMenu("Add Characteristic Reward")]
    public void AddBagReward()
    {
        List<CharacteristicReward> rewards = new()
        {
            new CharacteristicReward(_characteristicType, _value),
        };

        BagReward bagReward = new(rewards);
        PlayerData.AddReward(bagReward);
    }

    [ContextMenu("Give Gadget Reward")]
    public void GiveGadget()
    {
        GadgetReward gadgetReward = new(_gadgetReward);
        PlayerData.AddReward(new ChestReward(new List<GadgetReward> { gadgetReward }));
    }

    private void Start()
    {
        if (_skipTutorial)
        {
            for (int i = 0; i < GlobalSettings.Instance.TrainingLevelsCount; i++)
            {
                PlayerData.TrainingPassed();
            }
        }

        PlayerData.AddExperience(_XPToGiveOnStart);
    }
}
