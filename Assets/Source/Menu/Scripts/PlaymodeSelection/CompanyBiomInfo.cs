using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public interface ICompanyBiomInfoReadOnly
{
    public int ID { get; }
    public Sprite BiomSprite { get; }
    public MapCellsContainer Map { get; }
    public ReadOnlyCollection<ChestReward.ChestType?> Rewards { get; }
    public CoinsReward RewardsPerStar { get; }
    public string BiomName { get; }
    public int CurrentStars { get; }
    public int ReduseUpgrades { get; }
}

public class CompanyBiomInfo : ICompanyBiomInfoReadOnly
{
    private readonly int _id;
    private readonly Sprite _biomSprite;
    private readonly MapCellsContainer _map;
    private readonly ChestReward.ChestType?[] _rewards;
    private readonly CoinsReward _rewardsPerStar;
    private readonly string _biomName;
    private int _currentStars;
    private int _startReduseUpgrades;
    private int _reduseUpgradesByStar;

    public int ID => _id;
    public Sprite BiomSprite => _biomSprite;
    public MapCellsContainer Map => _map;
    public ReadOnlyCollection<ChestReward.ChestType?> Rewards => new(_rewards);
    public CoinsReward RewardsPerStar => _rewardsPerStar;
    public string BiomName => _biomName;
    public int CurrentStars => _currentStars;
    public int ReduseUpgrades => _startReduseUpgrades + _reduseUpgradesByStar * _currentStars;

    public CompanyBiomInfo(Biom biom)
    {
        _id = biom.ID;
        _biomSprite = biom.Sprite;
        _map = biom.Map;
        _rewards = biom.Rewards;
        _rewardsPerStar = new CoinsReward(biom.CoinsRewardPerStar);
        _biomName = biom.name;
        _startReduseUpgrades = biom.StartReduseUpgrades;
        _reduseUpgradesByStar = biom.ReduseUpgradesByStar;
        _currentStars = 0;
    }

    public void LoadSave(CompanyBiomSaveInfo saveInfo)
    {
        if (saveInfo.ID == _id)
        {
            _currentStars = saveInfo.CurrentStars;
            return;
        }

        throw new System.Exception("Save info does not match biom name");
    }

    public List<Reward> AddStars(int stars)
    {
        List<Reward> rewards = new();

        for (int i = 0; i < stars; i++)
        {
            if (_currentStars + i < _rewards.Length)
            {
                if (_rewards[_currentStars + i] is ChestReward.ChestType reward)
                {
                    rewards.Add(new ChestReward(reward));
                }
            }

            rewards.Add(_rewardsPerStar);
        }

        _currentStars += stars;

        if (_currentStars > _rewards.Length)
        {
            _currentStars = _rewards.Length;
        }

        return rewards;
    }
}
