using System;
using UnityEngine;

[Serializable]
public class CoinsReward : Reward
{
    [SerializeField] private int _amount;

    public int Amount => _amount;

    public CoinsReward(int amount)
    {
        _amount = amount;
    }

    public CoinsReward(CoinsSaveInfo saveInfo)
    {
        _amount = saveInfo.Amount;
    }

    public override void ApplyReward()
    {
        PlayerData.AddCoins(_amount);
    }

    public override Reward[] GetSimpleRewards()
    {
        return new Reward[] { this };
    }
}
