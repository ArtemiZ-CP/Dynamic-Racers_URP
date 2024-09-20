using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    private const string RunIcon = "<sprite=\"Run\" index=0>";
    private const string SwimIcon = "<sprite=\"Swim\" index=0>";
    private const string ClimbIcon = "<sprite=\"Climb\" index=0>";
    private const string FlyIcon = "<sprite=\"Fly\" index=0>";

    private readonly int SpeedMultiplier = Animator.StringToHash(nameof(SpeedMultiplier));

    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemSpeedBonus;
    [SerializeField] private TMP_Text _itemDescription;
    [SerializeField] private TMP_Text _itemDistance;
    [SerializeField] private TMP_Text _itemUses;
    [SerializeField] private TMP_Text _itemSpeedBoost;
    [SerializeField] private GadgetCollectionCell _gadgetCollectionCell;
    [SerializeField] private Image _itemRareImage;
    [SerializeField] private TMP_Text _itemRareText;
    [SerializeField] private TMP_Text _itemCategoryText;
    [SerializeField] private Transform _playerAnimatorParent;
    [SerializeField] private Material _playerMaterial;
    [SerializeField] private UpgradeGadget _upgradeGadget;

    private GlobalSettings _globalSettings;
    private GadgetScriptableObject _gadget;
    private Animator _gadgetAnimator;
    private GadgetChunkInfo _gadgetChunkInfo;
    private Animator _playerAnimator;

    private void Awake()
    {
        _globalSettings = GlobalSettings.Instance;
        _playerAnimator = _playerAnimatorParent.GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        if (_gadgetAnimator != null)
        {
            Destroy(_gadgetAnimator.gameObject);
            _gadgetAnimator = null;
        }

        _playerAnimator.SetFloat(SpeedMultiplier, _globalSettings.BaseSpeed);
        ActivePlayerAnimation();
        
        if (RunSettings.PlayerGadget != null)
        {
            ShowGadgetInfo(RunSettings.PlayerGadget);
        }

        _upgradeGadget.LevelUp += ShowGadgetInfo;
    }

    private void OnDisable()
    {
        _upgradeGadget.LevelUp -= ShowGadgetInfo;
    }

    public void SetPlayerAnimation(GadgetScriptableObject gadget, GadgetChunkInfo gadgetChunkInfo)
    {
        _gadget = gadget;
        _gadgetChunkInfo = gadgetChunkInfo;
    }

    private void ActivePlayerAnimation()
    {
        if (Instantiate(_gadget.Prefab, _playerAnimator.transform).TryGetComponent(out _gadgetAnimator))
        {
            _gadgetAnimator.SetTrigger(_gadgetChunkInfo.AnimationTriggerName);
            _gadgetAnimator.SetFloat(SpeedMultiplier, _globalSettings.BaseSpeed);
            SkinnedMeshRenderer skinnedMeshRenderer = _gadgetAnimator.transform.GetComponentInChildren<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.material = _playerMaterial;
            }
        }

        _playerAnimator.SetTrigger(_gadgetChunkInfo.AnimationTriggerName);
    }

    private void ShowGadgetInfo(Gadget gadget)
    {
        _itemName.text = gadget.ScriptableObject.Name;
        ShowSpeedBonus(gadget);
        _itemDescription.text = gadget.ScriptableObject.Description;
        _itemDistance.text = gadget.ScriptableObject.DistanceToDisactive == float.MaxValue ? "Infinity" : $"{gadget.ScriptableObject.DistanceToDisactive}m";
        _itemUses.text = gadget.ScriptableObject.UsageCount == int.MaxValue ? "Infinity" : gadget.ScriptableObject.UsageCount.ToString();

        if (gadget.TryGetAdditionalSpeed(out float additionalSpeed, out bool isAbleToUpgrade))
        {
            string color = isAbleToUpgrade ? "green" : "grey";

            _itemSpeedBoost.text = $"{gadget.SpeedMultiplier * 100}% <color={color}>+{additionalSpeed * 100}%</color>";
        }
        else
        {
            _itemSpeedBoost.text = $"{gadget.SpeedMultiplier * 100}%";
        }

        ShowGadget(gadget);
        ShowGadgetRare(gadget);
        ShowGadgetCategory(gadget);
    }

    private void ShowGadgetCategory(Gadget gadget)
    {
        _itemCategoryText.text = gadget.ScriptableObject.Group.ToString();
    }

    private void ShowGadgetRare(Gadget gadget)
    {
        _itemRareImage.sprite = _globalSettings.GetGadgetRareBackground(gadget.ScriptableObject.Rare);
        _itemRareText.text = gadget.ScriptableObject.Rare.ToString();
        _itemRareText.color = _globalSettings.GetGadgetRareColor(gadget.ScriptableObject.Rare);
    }

    private void ShowGadget(Gadget gadget)
    {
        _gadgetCollectionCell.Initialize(gadget, PlayerData.PlayerGadgets.Any(g => g.ScriptableObject == gadget.ScriptableObject));
    }

    private void ShowSpeedBonus(Gadget gadget)
    {
        _itemSpeedBonus.text = string.Empty;

        if (IsGadgetAccelerates(gadget, ChunkType.Ground))
        {
            _itemSpeedBonus.text += $"{RunIcon}Run    ";
        }

        if (IsGadgetAccelerates(gadget, ChunkType.Water))
        {
            _itemSpeedBonus.text += $"{SwimIcon}Swim    ";
        }

        if (IsGadgetAccelerates(gadget, ChunkType.Wall))
        {
            _itemSpeedBonus.text += $"{ClimbIcon}Climb    ";
        }

        if (IsGadgetAccelerates(gadget, ChunkType.Fly))
        {
            _itemSpeedBonus.text += $"{FlyIcon}Fly    ";
        }
    }

    private bool IsGadgetAccelerates(Gadget gadget, ChunkType chunkType)
    {
        GadgetChunkInfo chunkInfo = gadget.ScriptableObject.GetChunkInfo(chunkType);

        return chunkInfo != null && chunkInfo.IsAccelerates;
    }
}
