using System.Collections.Generic;
using UnityEngine;

public class GiveLevelXP : MonoBehaviour
{
    [SerializeField] private float _XPToGive;
    [SerializeField] CharacteristicType _characteristicType;
    [SerializeField] private int _upgradeAmount;
    [SerializeField] private RewardMenu _rewardMenu;

    [ContextMenu("Give Upgrades")]
    public void GiveUpgrades()
    {
        Queue<CharacteristicReward> rewards = new();
        rewards.Enqueue(new CharacteristicReward(_characteristicType, _upgradeAmount));
        _rewardMenu.AddRewardToQueue(new BagReward(rewards));
    }

    private void Update()
    {
        PlayerProgress.Experience = _XPToGive;
    }
}
