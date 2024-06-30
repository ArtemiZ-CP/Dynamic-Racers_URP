using System.Collections.Generic;

public class BagReward : RewardContainer
{
    public Queue<CharacteristicReward> RewardsQueue { get; }

    public BagReward(Queue<CharacteristicReward> rewards)
    {
        RewardsQueue = rewards;
    }
}
