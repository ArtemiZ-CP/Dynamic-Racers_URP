using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoxReward : RewardContainer
{
    [SerializeField] private List<GadgetReward> _rewards;

    public List<GadgetReward> RewardsQueue => _rewards;

    public BoxReward(List<GadgetReward> rewards)
    {
        _rewards = new List<GadgetReward>(rewards);
    }
}