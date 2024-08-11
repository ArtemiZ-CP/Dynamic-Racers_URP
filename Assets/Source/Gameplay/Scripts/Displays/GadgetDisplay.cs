using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GadgetDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _usageCountText;
    [SerializeField] private GameObject _usageCount;
    [SerializeField] private TMP_Text[] _splitedUsageCountTexts;
    [SerializeField] private GameObject _splitedUsageCount;
    [SerializeField] private Image _gadgetEnergyBar;
    [SerializeField] GameObject _energyBarDisplay;

    private PlayerGadgets _playerGadgets;

    private void Start()
    {
        _playerGadgets = FindObjectOfType<PlayerGadgets>();

        if (_playerGadgets != null && _playerGadgets.Gadget != null)
        {
            if (_playerGadgets.Gadget.DistanceToDisactive == float.MaxValue)
            {
                _energyBarDisplay.SetActive(false);
            }

            if (_playerGadgets.IsUsageSplited)
            {
                _usageCount.SetActive(false);
                _splitedUsageCount.SetActive(true);
            }
            else
            {
                _usageCount.SetActive(true);
                _splitedUsageCount.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (_playerGadgets == null || _playerGadgets.Gadget == null)
        {
            return;
        }

        if (_playerGadgets.IsUsageSplited)
        {
            _usageCount.SetActive(false);
            _splitedUsageCount.SetActive(true);

            for (int i = 0; i < _splitedUsageCountTexts.Length; i++)
            {
                SetCount(_splitedUsageCountTexts[i], _playerGadgets.UsageCounts[i]);
            }
        }
        else
        {
            _usageCount.SetActive(true);
            _splitedUsageCount.SetActive(false);

            SetCount(_usageCountText, _playerGadgets.UsageCounts[0]);
        }

        _gadgetEnergyBar.fillAmount = _playerGadgets.RemainingDistance / _playerGadgets.Gadget.DistanceToDisactive;
    }

    private void SetCount(TMP_Text tmpText, int count)
    {
        if (count == int.MaxValue)
        {
            tmpText.text = "∞";
        }
        else
        {
            tmpText.text = count.ToString();
        }
    }
}
