using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeGadget : MonoBehaviour
{
    private const string CoinIcon = "<sprite=\"Coin\" index=0>";

    [SerializeField] private GadgetCollectionCell _gadgetCollectionCell;
    [SerializeField] private TMP_Text _upgradeCost;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private GameObject _upgradeButtonObject;
    [SerializeField] private BlickAnimation _blickAnimation;

    private GlobalSettings _globalSettings;

    public event Action<Gadget> LevelUp;

    private void Awake()
    {
        _globalSettings = GlobalSettings.Instance;
    }

    private void OnEnable()
    {
        UpdateGadget();
    }

    public void Upgrade()
    {
        _gadgetCollectionCell.Gadget.TryLevelUp();
        UpdateGadget();
        LevelUp?.Invoke(_gadgetCollectionCell.Gadget);
    }

    private void UpdateGadget()
    {
        _gadgetCollectionCell.UpdateGadget();

        if (_globalSettings.TryGetGadgetsLevelProgression(_gadgetCollectionCell.Gadget, out int gadgetsToLEvelUp, out int coinsCost))
        {
            _upgradeCost.text = $"{coinsCost}{CoinIcon}";
            bool isAbleToUpgrade = PlayerData.Coins >= coinsCost && _gadgetCollectionCell.Gadget.GetAmount() >= gadgetsToLEvelUp;
            _upgradeButton.interactable = isAbleToUpgrade;

            if (isAbleToUpgrade == false)
            {
                _blickAnimation.DisactiveButton();
            }

            _upgradeButtonObject.SetActive(true);
        }
        else
        {
            _upgradeButton.interactable = false;
            _upgradeButtonObject.SetActive(false);
        }
    }
}
