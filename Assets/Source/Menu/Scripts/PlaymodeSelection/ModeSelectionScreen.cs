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
        RunSettings.SetCompanyRun(biomInfo,  GlobalSettings.StarsRewards.ToArray(), SelectMap(biomInfo), GlobalSettings.GameplayPlayersCount);
    }

    public void ChooseRankedMapPreset(ICompanyBiomInfoReadOnly biomInfo)
    {
        RunSettings.SetRankedRun(biomInfo, _rankedExperienceRewards,  SelectMap(biomInfo), GlobalSettings.GameplayPlayersCount);
    }

    private ChunkSettings[] SelectMap(ICompanyBiomInfoReadOnly biomInfo)
    {
        ChunkSettings[] mapPreset = biomInfo.GetMapPreset();
        _mapDrawer.DrawMap(mapPreset);
        return mapPreset;
    }
}
