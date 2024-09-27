using TMPro;
using UnityEngine;

public class FoundGadgetsCount : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _format;

    private GadgetSettings _gadgetSettings;

    private GadgetSettings GadgetSettings
    {
        get
        {
            if (_gadgetSettings == null)
            {
                _gadgetSettings = GadgetSettings.Instance;
            }

            return _gadgetSettings;
        }
    }

    private void OnEnable()
    {
        SetText();
    }

    private void SetText()
    {
        _text.text = $"{_format}{PlayerData.PlayerGadgets.Count}/{GadgetSettings.GetAllGadgets().Count}";
    }
}
