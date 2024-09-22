using UnityEngine;

[CreateAssetMenu(fileName = "Biom", menuName = "Biom")]
public class Biom : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private MapCellsContainer _map;
    [Header("Rewards")]
    [SerializeField] private ChestRewardInfo[] _rewards;
    [SerializeField, Min(1)] private int _starPerReward;
    [SerializeField] private int _coinsRewardPerStar;
    [Header("Upgrades")]
    [SerializeField] private int _startReduseUpgrades;
    [SerializeField] private int _reduseUpgradesByStar;
    [Header("Gadgets")]
    [SerializeField] private Rare _maxEnemyGadget;

    public int ID => _id;
    public Sprite Sprite => _icon;
    public MapCellsContainer Map => _map;
    public ChestRewardInfo[] Rewards => ConvertRewards();
    public int CoinsRewardPerStar => _coinsRewardPerStar;
    public int StartReduseUpgrades => _startReduseUpgrades;
    public int ReduseUpgradesByStar => _reduseUpgradesByStar;
    public Rare MaxEnemyGadget => _maxEnemyGadget;

    private ChestRewardInfo[] ConvertRewards()
    {
        ChestRewardInfo[] rewards = new ChestRewardInfo[_rewards.Length * _starPerReward];

        for (int i = 0; i < _rewards.Length; i++)
        {
            ChestRewardInfo chestType = _rewards[i];

            if (chestType != null)
            {
                rewards[i * _starPerReward + _starPerReward - 1] = chestType;
            }
        }

        return rewards;
    }
}
