using System.Collections.Generic;
using UnityEngine;

public class BagReward : RewardContainer
{
    [SerializeField] private List<CharacteristicReward> _rewards;

    public List<CharacteristicReward> RewardsQueue => _rewards;

    public BagReward(List<CharacteristicReward> rewards)
    {
        _rewards = new List<CharacteristicReward>(rewards);
    }
}
