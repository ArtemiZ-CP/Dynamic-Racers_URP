using System.Collections.Generic;
using UnityEngine;

public class SelectionGadgetScreen : MonoBehaviour
{
    [SerializeField] private MapDrawer _mapDrawer;
    [SerializeField] private float _experienceReward = 100;
    [SerializeField] private List<MapPreset> _mapPresets = new();

    private void OnEnable()
    {
        RunSettings.ExperienceReward = _experienceReward;
        RunSettings.PlayerGadget = null;
        ChooseMapPreset();
    }

    private void ChooseMapPreset()
    {
        MapPreset mapPreset = _mapPresets[Random.Range(0, _mapPresets.Count)];
        RunSettings.Map = mapPreset.Map;
        _mapDrawer.DrawMap(mapPreset);
    }
}
