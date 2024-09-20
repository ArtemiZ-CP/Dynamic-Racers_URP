using System;

[Serializable]
public abstract class Reward
{
    public abstract void ApplyReward();
    public abstract Reward[] GetSimpleRewards();
}
