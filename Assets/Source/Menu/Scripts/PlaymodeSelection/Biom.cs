using UnityEngine;

[CreateAssetMenu(fileName = "Biom", menuName = "Biom")]
public class Biom : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private Sprite _icon;
    [Header("Map")] 
    [SerializeField] private MapCellsContainer _map;
    [SerializeField] private bool _randomMap;
    [SerializeField] private MapPreset[] _mapPresets;
    [SerializeField] private MapGenerationSettings _mapGenerationSettings;
    [Header("Rewards")]
    [SerializeField] private BiomReward[] _rewards;
    [SerializeField, Min(1)] private int _starPerReward;
    [SerializeField] private int _coinsRewardPerStar;
    [Header("Upgrades")]
    [SerializeField] private float _startReduseUpgrades;
    [SerializeField] private float _reduseUpgradesByStar;
    [Header("Gadgets")]
    [SerializeField] private Rare _maxEnemyGadget;

    public int ID => _id;
    public Sprite Sprite => _icon;
    public MapCellsContainer Map => _map;
    public BiomReward[] Rewards => ConvertRewards();
    public int CoinsRewardPerStar => _coinsRewardPerStar;
    public float StartReduseUpgrades => _startReduseUpgrades;
    public float ReduseUpgradesByStar => _reduseUpgradesByStar;
    public Rare MaxEnemyGadget => _maxEnemyGadget;

    public ChunkSettings[] GetMapPreset()
    {
        if (_randomMap || _mapPresets == null || _mapPresets.Length == 0)
        {
            return _mapGenerationSettings.GenerateMap();
        }

        return _mapPresets[Random.Range(0, _mapPresets.Length)].Map;
    }

    private BiomReward[] ConvertRewards()
    {
        BiomReward[] rewards = new BiomReward[_rewards.Length * _starPerReward];

        for (int i = 0; i < _rewards.Length; i++)
        {
            BiomReward chestType = _rewards[i];

            if (chestType != null)
            {
                rewards[i * _starPerReward + _starPerReward - 1] = chestType;
            }
        }

        return rewards;
    }
}
