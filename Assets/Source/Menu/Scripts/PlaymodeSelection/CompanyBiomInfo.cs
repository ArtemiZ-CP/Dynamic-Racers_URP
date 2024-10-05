using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public interface ICompanyBiomInfoReadOnly
{
    public int ID { get; }
    public Sprite BiomSprite { get; }
    public MapCellsContainer Map { get; }
    public ReadOnlyCollection<BiomReward> Rewards { get; }
    public CoinsReward RewardsPerStar { get; }
    public string BiomName { get; }
    public int CurrentStars { get; }
    public float ReduseUpgrades { get; }
    public Rare MaxEnemyGadget { get; }
    public Func<ChunkSettings[]> GetMapPreset { get; }
}

public class CompanyBiomInfo : ICompanyBiomInfoReadOnly
{
    private readonly int _id;
    private readonly Biom _biom;
    private int _currentStars;

    public int ID => _id;
    public Sprite BiomSprite => _biom.Sprite;
    public MapCellsContainer Map => _biom.Map;
    public ReadOnlyCollection<BiomReward> Rewards => new(_biom.Rewards);
    public CoinsReward RewardsPerStar => new(_biom.CoinsRewardPerStar);
    public string BiomName => _biom.name;
    public int CurrentStars => _currentStars;
    public float ReduseUpgrades => _biom.StartReduseUpgrades + _biom.ReduseUpgradesByStar * _currentStars;
    public Rare MaxEnemyGadget => _biom.MaxEnemyGadget;
    public Func<ChunkSettings[]> GetMapPreset => () => _biom.GetMapPreset();

    public CompanyBiomInfo(Biom biom)
    {
        _id = biom.ID;
        _biom = biom;
        _currentStars = 0;
    }

    public void LoadSave(CompanyBiomSaveInfo saveInfo)
    {
        if (saveInfo.ID == _id)
        {
            _currentStars = saveInfo.CurrentStars;
            return;
        }

        throw new Exception("Save info does not match biom index");
    }

    public List<Reward> AddStars(int stars)
    {
        List<Reward> rewards = new();
        BiomReward[] biomRewards = _biom.Rewards;

        for (int i = 0; i < stars; i++)
        {
            if (_currentStars + i < biomRewards.Length)
            {
                if (biomRewards[_currentStars + i] != null)
                {
                    rewards.AddRange(biomRewards[_currentStars + i].GetRewards());
                }

                rewards.Add(RewardsPerStar);
            }
        }

        _currentStars += stars;

        if (_currentStars > biomRewards.Length)
        {
            _currentStars = biomRewards.Length;
        }

        return rewards;
    }
}
