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
    private static ICompanyBiomInfoReadOnly _biomInfo;
    private static RunType _runType;
    private static int _playersCount;

    public static ChunkSettings[] Map => _map;
    public static MapCellsContainer MapCellsContainer => _biomInfo.Map;
    public static RunType Type => _runType;
    public static int PlayersCount => _playersCount;
    public static int ReduseUpgrades => _biomInfo.ReduseUpgrades;
    public static Rare MaxEnemyGadget => _biomInfo.MaxEnemyGadget;

    public static void SetTrainingRun(ICompanyBiomInfoReadOnly biomInfo, ChunkSettings[] map, int playersCount)
    {
        _biomInfo = biomInfo;
        _map = map;
        _playersCount = playersCount;
        _runType = RunType.Training;
    }

    public static void SetCompanyRun(ICompanyBiomInfoReadOnly biomInfo, int[] biomPlacementRewards, ChunkSettings[] map, int playersCount)
    {
        _biomInfo = biomInfo;
        _map = map;
        _companyStarsRewards = biomPlacementRewards;
        _playersCount = playersCount;
        _runType = RunType.Company;
    }

    public static void SetRankedRun(ICompanyBiomInfoReadOnly biomInfo, int[] rankedPlacementRewards, ChunkSettings[] map, int playersCount)
    {
        _biomInfo = biomInfo;
        _map = map;
        _rankedExperienceRewards = rankedPlacementRewards;
        _playersCount = playersCount;
        _runType = RunType.Company;
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
