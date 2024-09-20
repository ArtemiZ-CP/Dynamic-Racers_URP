using System;
using System.Collections.Generic;
using UnityEngine;

public class AddRunReward : MonoBehaviour
{
    [ContextMenu("Give Run Rewards")]
    public void GiveRunRewardsDebug()
    {
        PlayerData.AddRunRewards(new List<Reward>
        {
            new ChestReward((ChestReward.ChestType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(ChestReward.ChestType)).Length)),
            new CoinsReward(UnityEngine.Random.Range(10, 100) * 100),
            new DiamondsReward(UnityEngine.Random.Range(1, 10) * 10),
        }, UnityEngine.Random.Range(1, 4));
    }
}
