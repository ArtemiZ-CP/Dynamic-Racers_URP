using System.Collections.Generic;
using UnityEngine;

public class GadgetSelectionScreen : MonoBehaviour
{
    [SerializeField] private MapDrawer _mapDrawer;
    [SerializeField] private int _experienceReward = 100;
    [SerializeField] private MapCellsContainer _trainingLocation;
    [SerializeField] private MapCellsContainer _alienLocation;
    [SerializeField] private List<MapPreset> _mapPresets = new();
    [SerializeField] private List<MapPreset> _trainingMapPresets = new();

    public void SetMapPreset()
    {
        RunSettings.ExperienceReward = _experienceReward;
        RunSettings.PlayerGadget = null;

        if (PlayerData.PassedTrainings < GlobalSettings.Instance.TrainingLevelsCount)
        {
            ChooseTrainingMapPreset();
        }
        else
        {
            ChooseMapPreset();
        }
    }

    private void ChooseMapPreset()
    {
        MapPreset mapPreset = _mapPresets[Random.Range(0, _mapPresets.Count)];
        RunSettings.Map = mapPreset.Map;
        RunSettings.MapCellsContainer = _alienLocation;
        RunSettings.PlayersCount = GlobalSettings.Instance.GameplayPlayersCount;
        _mapDrawer.DrawMap(mapPreset);
    }

    private void ChooseTrainingMapPreset()
    {
        MapPreset mapPreset = _trainingMapPresets[PlayerData.PassedTrainings];
        RunSettings.Map = mapPreset.Map;
        RunSettings.MapCellsContainer = _trainingLocation;
        RunSettings.PlayersCount = GlobalSettings.Instance.TrainingPlayersCount;
        _mapDrawer.DrawMap(mapPreset);
    }
}
