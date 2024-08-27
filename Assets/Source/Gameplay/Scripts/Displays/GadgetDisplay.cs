using TMPro;
using UnityEngine;

public class GadgetDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _usageCountText;
    [SerializeField] private GameObject _usageCount;
    [SerializeField] private TMP_Text[] _splitedUsageCountTexts;
    [SerializeField] private GameObject _splitedUsageCount;
    [SerializeField] private RectTransform _gadgetEnergyBar;
    [SerializeField] private GameObject[] _energyBarDisplay;

    private PlayerGadgets _playerGadgets;
    private float _startSize;

    private void Start()
    {
        _startSize = _gadgetEnergyBar.sizeDelta.x;
        _playerGadgets = FindObjectOfType<PlayerGadgets>();

        if (_playerGadgets != null && _playerGadgets.Gadget != null)
        {
            if (_playerGadgets.Gadget.DistanceToDisactive == float.MaxValue)
            {
                foreach (var item in _energyBarDisplay)
                {
                    item.SetActive(false);
                }
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

        SetEnerguBar();
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

    private void SetEnerguBar()
    {
        float persent = 1;

        if (_playerGadgets.DistanceToDisactiveGadget > 0)
        {
            persent = _playerGadgets.RemainingDistance / _playerGadgets.Gadget.DistanceToDisactive;
        }
        else
        {
            if (IsGadgetHasUsages() == false)
            {
                persent = 0;
            }
        }

        _gadgetEnergyBar.sizeDelta = new Vector2(_startSize * persent, _gadgetEnergyBar.sizeDelta.y);
        _gadgetEnergyBar.anchoredPosition = new Vector2(_startSize * persent / 2, _gadgetEnergyBar.anchoredPosition.y);
    }

    private bool IsGadgetHasUsages()
    {
        foreach (var item in _playerGadgets.UsageCounts)
        {
            if (item > 0)
            {
                return true;
            }
        }

        return false;
    }
}
