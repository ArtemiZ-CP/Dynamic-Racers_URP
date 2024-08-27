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

    private GlobalSettings _globalSettings;

    private void Awake()
    {
        _globalSettings = GlobalSettings.Instance;
    }

    private void OnEnable()
    {
        if (RunSettings.PlayerGadget != null)
        {
            ShowGadgetInfo(PlayerData.PlayerGadgets.First(g => g.GadgetScriptableObject == RunSettings.PlayerGadget));
        }
    }

    private void ShowGadgetInfo(Gadget gadget)
    {
        _itemName.text = gadget.GadgetScriptableObject.Name;
        ShowSpeedBonus(gadget);
        _itemDescription.text = gadget.GadgetScriptableObject.Description;
        _itemDistance.text = gadget.GadgetScriptableObject.DistanceToDisactive == float.MaxValue ? "Infinity" : $"{gadget.GadgetScriptableObject.DistanceToDisactive}m";
        _itemUses.text = gadget.GadgetScriptableObject.UsageCount == int.MaxValue ? "Infinity" : gadget.GadgetScriptableObject.UsageCount.ToString();
        _itemSpeedBoost.text = $"{(int)(gadget.GadgetScriptableObject.SpeedMultiplier * 100)}%";
        ShowGadget(gadget);
        ShowGadgetRare(gadget);
        ShowGadgetCategory(gadget);
    }

    private void ShowGadgetCategory(Gadget gadget)
    {
        _itemCategoryText.text = gadget.GadgetScriptableObject.Group.ToString();
    }

    private void ShowGadgetRare(Gadget gadget)
    {
        _itemRareImage.sprite = _globalSettings.GetGadgetRareBackground(gadget.GadgetScriptableObject.Rare);
        _itemRareText.text = gadget.GadgetScriptableObject.Rare.ToString();
        _itemRareText.color = _globalSettings.GetGadgetRareColor(gadget.GadgetScriptableObject.Rare);
    }

    private void ShowGadget(Gadget gadget)
    {
        _gadgetCollectionCell.Init(gadget, PlayerData.PlayerGadgets.Any(g => g.GadgetScriptableObject == gadget.GadgetScriptableObject));
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
        GadgetChunkInfo chunkInfo = gadget.GadgetScriptableObject.GetChunkInfo(chunkType);

        return chunkInfo != null && chunkInfo.IsAccelerates;
    }
}
