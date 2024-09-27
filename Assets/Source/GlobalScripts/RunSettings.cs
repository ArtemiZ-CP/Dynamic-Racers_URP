using System.Collections.Generic;

public static class RunSettings
{
    public enum RunType
    {
        None,
        Training,
        Company,
        Ranked
    }

    public static Gadget PlayerGadget { get; set; }

    private static int[] _companyStarsRewards;
    private static int[] _rankedExperienceRewards;
    private static ChunkSettings[] _map;
    private static Gadget[] _enemyGadgets;
    private static ICompanyBiomInfoReadOnly _biomInfo;
    private static RunType _runType;
    private static int _playersCount;

    public static IReadOnlyCollection<ChunkSettings> Map => _map;
    public static MapCellsContainer MapCellsContainer => _biomInfo.Map;
    public static RunType Type => _runType;
    public static IReadOnlyCollection<Gadget> EnemyGadgets => _enemyGadgets;
    public static int PlayersCount => _playersCount;
    public static float ReduseUpgrades => _biomInfo.ReduseUpgrades;

    public static void SetTrainingRun(ICompanyBiomInfoReadOnly biomInfo, ChunkSettings[] map, int playersCount)
    {
        _biomInfo = biomInfo;
        _map = map;
        _playersCount = playersCount;
        _runType = RunType.Training;
        SetEnemyGadgets(biomInfo);
    }

    public static void SetCompanyRun(ICompanyBiomInfoReadOnly biomInfo, int[] biomPlacementRewards, ChunkSettings[] map, int playersCount)
    {
        _biomInfo = biomInfo;
        _map = map;
        _companyStarsRewards = biomPlacementRewards;
        _playersCount = playersCount;
        _runType = RunType.Company;
        SetEnemyGadgets(biomInfo);
    }

    public static void SetRankedRun(ICompanyBiomInfoReadOnly biomInfo, int[] rankedPlacementRewards, ChunkSettings[] map, int playersCount)
    {
        _biomInfo = biomInfo;
        _map = map;
        _rankedExperienceRewards = rankedPlacementRewards;
        _playersCount = playersCount;
        _runType = RunType.Company;
        SetEnemyGadgets(biomInfo);
    }

    public static void ApplyRewards(int placement)
    {
        if (_runType == RunType.Company)
        {
            if (_companyStarsRewards == null || placement < 1 || placement > _companyStarsRewards.Length)
            {
                Reset();
                return;
            }

            int starsReward = _companyStarsRewards[placement - 1];
            PlayerData.AddCompanyStars(_biomInfo.ID, starsReward);
        }
        else if (_runType == RunType.Ranked)
        {
            if (_rankedExperienceRewards == null || placement < 1 || placement > _rankedExperienceRewards.Length)
            {
                Reset();
                return;
            }

            int experienceReward = _rankedExperienceRewards[placement - 1];
            PlayerData.AddExperience(experienceReward);
        }

        Reset();
    }

    private static void SetEnemyGadgets(ICompanyBiomInfoReadOnly biomInfo)
    {
        _enemyGadgets = new Gadget[_playersCount - 1];

        for (int i = 0; i < _playersCount - 1; i++)
        {
            List<Rare> rares = new();

            for (int gadget = (int)Rare.Common; gadget <= (int)biomInfo.MaxEnemyGadget; gadget++)
            {
                rares.Add((Rare)gadget);
            }

            _enemyGadgets[i] = new Gadget(GadgetSettings.Instance.GetRandomGadget(rares.ToArray()));
        }
    }

    private static void Reset()
    {
        PlayerGadget = null;
        _companyStarsRewards = null;
        _map = null;
        _biomInfo = null;
        _playersCount = 0;
        _runType = RunType.None;
    }
}
