using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class FoundGadgetsCount : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _text.text = $"{PlayerProgress.PlayerGadgets.Count}/{GlobalSettings.Instance.AllGadgets.Count}";
    }
}
