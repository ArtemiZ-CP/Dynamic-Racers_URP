using System.Linq;
using UnityEngine;

public class ModeSelectionScreen : MonoBehaviour
{
    [SerializeField] private MapDrawer _mapDrawer;
    [SerializeField] private int[] _rankedExperienceRewards;
    [SerializeField] private MapPreset[] _mapPresets;

    private GlobalSettings _globalSettings;

    private GlobalSettings GlobalSettings
    {
        get
        {
            if (_globalSettings == null) _globalSettings = GlobalSettings.Instance;
            return _globalSettings;
        }
        set
        {
            _globalSettings = value;
        }
    }

    public void ChooseCompanyMapPreset(ICompanyBiomInfoReadOnly biomInfo)
    {
        RunSettings.SetCompanyRun(biomInfo,  GlobalSettings.StarsRewards.ToArray(), SelectRandomMap(_mapPresets).Map.ToArray(), GlobalSettings.GameplayPlayersCount);
    }

    public void ChooseRankedMapPreset(ICompanyBiomInfoReadOnly biomInfo)
    {
        RunSettings.SetRankedRun(biomInfo, _rankedExperienceRewards, SelectRandomMap(_mapPresets).Map.ToArray(), GlobalSettings.GameplayPlayersCount);
    }

    private MapPreset SelectRandomMap(MapPreset[] mapPresets)
    {
        MapPreset mapPreset = mapPresets[Random.Range(0, _mapPresets.Length)];
        _mapDrawer.DrawMap(mapPreset);
        return mapPreset;
    }
}
