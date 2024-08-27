using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeGadget : MonoBehaviour
{
    private const string CoinIcon = "<sprite=\"Coin\" index=0>";

    [SerializeField] private GadgetCollectionCell _gadgetCollectionCell;
    [SerializeField] private TMP_Text _upgradeCost;
    [SerializeField] private Material _upgradeButtonMaterial;
    [SerializeField] private Sprite _upgradeButtonSprite;
    [SerializeField] private Sprite _disactiveUpgradeButtonSprite;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private GameObject _upgradeButtonObject;
    [SerializeField] private BlickAnimation _blickAnimation;

    private GlobalSettings _globalSettings;

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
    }

    private void UpdateGadget()
    {
        _gadgetCollectionCell.UpdateGadget();

        if (_globalSettings.TryGetGadgetsLevelProgression(_gadgetCollectionCell.Gadget.Level, out int gadgetsToLEvelUp, out int coinsCost))
        {
            _upgradeCost.text = $"{coinsCost}{CoinIcon}";
            bool isAbleToUpgrade = PlayerData.Coins >= coinsCost && _gadgetCollectionCell.Gadget.GetAmount() >= gadgetsToLEvelUp;
            _upgradeButtonMaterial.SetTexture("_Texture2D", isAbleToUpgrade ? _upgradeButtonSprite.texture : _disactiveUpgradeButtonSprite.texture);
            _upgradeButton.interactable = isAbleToUpgrade;

            if (isAbleToUpgrade == false)
            {
                _blickAnimation.StopAnimation();
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
