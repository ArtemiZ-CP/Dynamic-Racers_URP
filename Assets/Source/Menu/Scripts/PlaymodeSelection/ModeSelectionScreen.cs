using UnityEngine;

public class ModeSelectionScreen : MonoBehaviour
{
    [SerializeField] private MapDrawer _mapDrawer;
    [SerializeField] private int[] _rankedExperienceRewards;
    [SerializeField] private int[] _companyStarsRewards;
    [SerializeField] private MapPreset[] _mapPresets;

    public void ChooseCompanyMapPreset(ICompanyBiomInfoReadOnly biomInfo)
    {
        RunSettings.SetCompanyRun(biomInfo, _companyStarsRewards, SelectRandomMap(_mapPresets).Map.ToArray(), GlobalSettings.Instance.GameplayPlayersCount);
    }

    public void ChooseRankedMapPreset(ICompanyBiomInfoReadOnly biomInfo)
    {
        RunSettings.SetRankedRun(biomInfo, _rankedExperienceRewards, SelectRandomMap(_mapPresets).Map.ToArray(), GlobalSettings.Instance.GameplayPlayersCount);
    }

    private MapPreset SelectRandomMap(MapPreset[] mapPresets)
    {
        MapPreset mapPreset = mapPresets[Random.Range(0, _mapPresets.Length)];
        _mapDrawer.DrawMap(mapPreset);
        return mapPreset;
    }
}
