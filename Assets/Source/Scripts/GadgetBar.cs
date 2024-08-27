using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GadgetBar : MonoBehaviour
{
    [SerializeField] private Image _bar;
    [SerializeField] private Image _fullBar;
    [SerializeField] private Image _maxBar;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private TMP_Text _countText;

    public void SetFill(Gadget gadget)
    {
        float fill;

        if (GlobalSettings.Instance.TryGetGadgetsLevelProgression(gadget.Level, out int gadgetsToLevelUp, out _))
        {
            int count = gadget.GetAmount();

            fill = (float)count / gadgetsToLevelUp;
            _countText.text = $"{count}/{gadgetsToLevelUp}";
        }
        else
        {
            _countText.text = "MAX";
            _maxBar.gameObject.SetActive(true);
            _fullBar.gameObject.SetActive(false);
            _bar.gameObject.SetActive(false);
            _arrow.SetActive(false);
            return;
        }

        if (fill >= 1)
        {
            _maxBar.gameObject.SetActive(false);
            _fullBar.gameObject.SetActive(true);
            _bar.gameObject.SetActive(false);
            _arrow.SetActive(true);
            return;
        }

        _maxBar.gameObject.SetActive(false);
        _fullBar.gameObject.SetActive(false);
        _bar.gameObject.SetActive(true);
        _arrow.SetActive(false);
        _bar.fillAmount = fill;
    }
}
