using System;
using UnityEngine;

[Serializable]
public class DiamondsReward : Reward
{
    [SerializeField] private int _amount;

    public int Amount => _amount;

    public DiamondsReward(int amount)
    {
        _amount = amount;
    }

    public DiamondsReward(DiamondsSaveInfo saveInfo)
    {
        _amount = saveInfo.Amount;
    }

    public override void ApplyReward()
    {
        PlayerData.AddDiamonds(_amount);
    }

    public override Reward[] GetSimpleRewards()
    {
        return new Reward[] { this };
    }
}
