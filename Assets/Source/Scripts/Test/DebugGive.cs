using System.Collections.Generic;
using UnityEngine;

public class DebugGive : MonoBehaviour
{
    [Header("Start")]
    [SerializeField] private bool _skipTutorial;
    [Space(20)]
    [SerializeField] private int _XPToGiveOnStart;
    [Header("On Click")]
    [SerializeField] private int _XPToGive;
    [SerializeField] private int _coinsToGive;
    [SerializeField] private int _diamondsToGive;
    [SerializeField] private int _ticketsToGive;
    [Space(20)]
    [SerializeField] CharacteristicType _characteristicType;
    [SerializeField] private int _upgradeAmount;
    [Space(20)]
    [SerializeField] private GadgetReward _gadgetReward;

    [ContextMenu("Give XP")]
    public void GiveXP()
    {
        PlayerProgress.AddExperience(_XPToGive);
    }

    [ContextMenu("Give Coins")]
    public void GiveCoins()
    {
        PlayerProgress.AddCoins(_coinsToGive);
    }

    [ContextMenu("Give Diamonds")]
    public void GiveDiamonds()
    {
        PlayerProgress.AddDiamonds(_diamondsToGive);
    }

    [ContextMenu("Give Tickets")]
    public void GiveTickets()
    {
        PlayerProgress.AddTickets(_ticketsToGive);
    }

    [ContextMenu("Give Upgrades")]
    public void GiveUpgrades()
    {
        List<CharacteristicReward> rewards = new()
        {
            new CharacteristicReward(_characteristicType, _upgradeAmount)
        };
        PlayerProgress.AddReward(new BagReward(rewards));
    }

    [ContextMenu("Give Gadget")]
    public void GiveGadget()
    {
        GadgetReward gadgetReward = new(_gadgetReward);
        PlayerProgress.AddReward(new BoxReward(new List<GadgetReward> { gadgetReward }));
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (_skipTutorial)
        {
            for (int i = 0; i < GlobalSettings.Instance.TrainingLevelsCount; i++)
            {
                PlayerProgress.TrainingPassed();
            }
        }

        PlayerProgress.AddExperience(_XPToGiveOnStart);
#endif
    }
}
