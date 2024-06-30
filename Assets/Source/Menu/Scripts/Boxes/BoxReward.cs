using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoxReward : RewardContainer
{
    [SerializeField] private List<GadgetReward> _boxRewards;

    public Queue<GadgetReward> RewardsQueue => new(_boxRewards);

    public BoxReward(Queue<GadgetReward> boxRewards)
    {
        _boxRewards = new List<GadgetReward>(boxRewards);
    }
}