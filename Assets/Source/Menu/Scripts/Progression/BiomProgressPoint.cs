using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiomProgressPoint : MonoBehaviour
{
    private const string StarIcon = "<sprite=\"Star\" index=0>";

    [SerializeField] private Image _chestImage;
    [SerializeField] private Image _lastChestImage;
    [SerializeField] private TMP_Text[] _chestProgress;

    public void Initialize(int progress, ChestReward.ChestType chestType, bool isLast)
    {
        Sprite sprite = GlobalSettings.Instance.GetChestSprite(chestType);

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
