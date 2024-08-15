using TMPro;
using UnityEngine;

public class FoundGadgetsCount : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _format;

    private GlobalSettings _globalSettings;

    private void Awake()
    {
        _globalSettings = GlobalSettings.Instance;
    }

    private void OnEnable()
    {
        SetText();
    }

    private void SetText()
    {
        _text.text = $"{_format}{PlayerData.PlayerGadgets.Count}/{_globalSettings.GetAllGadgets().Count}";
    }
}
