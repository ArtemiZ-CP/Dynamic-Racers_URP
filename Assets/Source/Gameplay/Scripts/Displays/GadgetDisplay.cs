using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GadgetDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _usageCountText;
    [SerializeField] private Image _gadgetEnergyBar;
    [SerializeField] GameObject _energyBarDisplay;

    private PlayerGadgets _playerGadgets;

    private void Start()
    {
        _playerGadgets = FindObjectOfType<PlayerGadgets>();

        if (_playerGadgets != null && _playerGadgets.Gadget != null 
        && _playerGadgets.Gadget.DistanceToDisactive == float.MaxValue)
        {
            _energyBarDisplay.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (_playerGadgets == null || _playerGadgets.Gadget == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if (_playerGadgets.UsageCount == int.MaxValue)
        {
            _usageCountText.text = "∞";
        }
        else
        {
            _usageCountText.text = _playerGadgets.UsageCount.ToString();
        }

        _gadgetEnergyBar.fillAmount = _playerGadgets.RemainingDistance / _playerGadgets.Gadget.DistanceToDisactive;
    }
}
