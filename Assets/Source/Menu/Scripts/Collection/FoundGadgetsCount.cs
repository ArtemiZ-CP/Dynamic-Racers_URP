using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class FoundGadgetsCount : MonoBehaviour
{
    private TMP_Text _text;
    private GlobalSettings _globalSettings;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _globalSettings = GlobalSettings.Instance;
    }

    private void OnEnable()
    {
        _text.text = $"{PlayerData.PlayerGadgets.Count}/{_globalSettings.GetAllGadgets().Count}";
    }
}
