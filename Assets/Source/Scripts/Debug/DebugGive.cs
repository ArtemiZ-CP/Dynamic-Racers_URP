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
    [Space(20)]
    [SerializeField] private int _coins;

    [ContextMenu("Give Gadget Reward")]
    public void GiveGadget()
    {
        GadgetReward gadgetReward = new(_gadgetReward);
        CharacteristicReward characteristicReward = new(_characteristicType, _value);

        PlayerData.AddReward(new ChestReward(
            ChestReward.ChestType.Wood,
            new List<GadgetReward> { gadgetReward },
            new List<CharacteristicReward> { characteristicReward }, 
            _coins));
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
