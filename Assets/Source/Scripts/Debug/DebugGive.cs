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
    [SerializeField] private int _diamonds;
    [Space(20)]
    [SerializeField] private int _biomID;
    [SerializeField] private int _starsAmount;

    [ContextMenu("Add Stars")]
    public void AddStars()
    {
        PlayerData.AddCompanyStars(_biomID, _starsAmount);
    }

    [ContextMenu("Give Rewards")]
    public void GiveGadget()
    {
        GadgetReward gadgetReward = new(_gadgetReward);
        CharacteristicReward characteristicReward = new(_characteristicType, _value);

        PlayerData.AddReward(new ChestReward(
            ChestReward.ChestType.Wood,
            new List<GadgetReward> { gadgetReward },
            new List<CharacteristicReward> { characteristicReward }, 
            new CoinsReward(_coins), new DiamondsReward(_diamonds)));
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
