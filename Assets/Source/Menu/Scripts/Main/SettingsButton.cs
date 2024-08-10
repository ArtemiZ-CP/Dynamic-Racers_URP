using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;

    public void OpenSettings()
    {
        _settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

    private void OnEnable()
    {
        CloseSettings();
    }
}
