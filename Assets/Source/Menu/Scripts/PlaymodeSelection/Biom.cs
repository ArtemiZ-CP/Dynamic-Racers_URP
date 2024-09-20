using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Biom", menuName = "Biom")]
public class Biom : ScriptableObject
{
    [Serializable]
    public enum BiomChestType
    {
        No = -1,
        Wood,
        Small,
        Medium,
        Big,
        Legendary
    }

    [SerializeField] private int _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private MapCellsContainer _map;
    [SerializeField] private BiomChestType[] _rewards;
    [SerializeField] private int _coinsRewardPerStar;
    [SerializeField] private int _startReduseUpgrades;
    [SerializeField] private int _reduseUpgradesByStar;

    public int ID => _id;
    public Sprite Sprite => _icon;
    public MapCellsContainer Map => _map;
    public ChestReward.ChestType?[] Rewards => ConvertRewards();
    public int CoinsRewardPerStar => _coinsRewardPerStar;
    public int StartReduseUpgrades => _startReduseUpgrades;
    public int ReduseUpgradesByStar => _reduseUpgradesByStar;

    private ChestReward.ChestType?[] ConvertRewards()
    {
        ChestReward.ChestType?[] rewards = new ChestReward.ChestType?[_rewards.Length];

        for (int i = 0; i < _rewards.Length; i++)
        {
            BiomChestType chestType = _rewards[i];

            if (chestType != BiomChestType.No)
            {
                rewards[i] = (ChestReward.ChestType)chestType;
            }
        }

        return rewards;
    }
}
