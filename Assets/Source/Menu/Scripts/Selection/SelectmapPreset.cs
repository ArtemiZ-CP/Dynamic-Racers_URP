using UnityEngine;

public class SelectmapPreset : MonoBehaviour
{
    [SerializeField] private MapCellsContainer _mapSetting;

    private void Awake()
    {
        RunSettings.MapCellsContainer = _mapSetting;
    }
}
