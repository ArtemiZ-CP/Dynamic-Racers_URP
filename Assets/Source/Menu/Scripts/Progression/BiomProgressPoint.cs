using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiomProgressPoint : MonoBehaviour
{
    private const string StarIcon = "<sprite=\"Star\" index=0>";

    [SerializeField] private Image _chestImage;
    [SerializeField] private Image _lastChestImage;
    [SerializeField] private TMP_Text[] _chestProgress;

    public void Initialize(int progress, BiomReward biomReward, bool isLast)
    {
        GlobalSettings globalSettings = GlobalSettings.Instance;
        GadgetSettings gadgetSettings = GadgetSettings.Instance;
        Sprite sprite;

        switch (biomReward.Type)
        {
            case BiomReward.RewardType.Coins:
                sprite = globalSettings.CoinsSprite;
                break;
            case BiomReward.RewardType.Diamonds:
                sprite = globalSettings.DiamondsSprite;
                break;
            case BiomReward.RewardType.Characteristics:
                sprite = globalSettings.GetCharacteristicRareBackground(biomReward.Rare);
                break;
            case BiomReward.RewardType.Gadget:
                sprite = gadgetSettings.GetGadgetMisteryRareBackground(biomReward.Rare);
                break;
            case BiomReward.RewardType.Chest:
                sprite = globalSettings.GetChestSprite(biomReward.ChestType);
                break;
            default:
                sprite = null;
                break;
        }

        foreach (TMP_Text text in _chestProgress)
        {
            text.text = $"{progress}{StarIcon}";
        }

        if (isLast)
        {
            _lastChestImage.gameObject.SetActive(true);
            _chestImage.gameObject.SetActive(false);
            _lastChestImage.sprite = sprite;
        }
        else
        {
            _lastChestImage.gameObject.SetActive(false);
            _chestImage.gameObject.SetActive(true);
            _chestImage.sprite = sprite;
        }
    }
}
