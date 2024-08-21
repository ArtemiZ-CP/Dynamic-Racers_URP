using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningChestUI : MonoBehaviour
{
    [SerializeField] private Image _chestImage;
    [SerializeField] private TMP_Text _chestTimeText;

    private OpeningChest _openingChest;
    private GlobalSettings _globalSettings;

    public bool HasChest => _openingChest != null;
    public bool IsOpened => _openingChest.IsOpend();

    private void Awake()
    {
        _globalSettings = GlobalSettings.Instance;
    }

    private void Update()
    {
        if (_openingChest == null)
        {
            _chestTimeText.text = string.Empty;
            return;
        }

        if (_openingChest.IsOpening)
        {
            if (_openingChest.IsOpeningCompleted)
            {
                _chestTimeText.text = "Completed";
            }
            else
            {
                _chestTimeText.text = _openingChest.TimeToOpen.ToString(@"hh\:mm\:ss");
            }
        }
        else
        {
            _chestTimeText.text = string.Empty;
        }
    }

    public bool TryOpen()
    {
        if (_openingChest.TryOpen())
        {
            ViewChest(null);
            return true;
        }

        return false;
    }

    public void StartOpening()
    {
        _openingChest.StartOpening();
    }

    public void ViewChest(OpeningChest openingChest)
    {
        if (openingChest == null || openingChest.HasChest == false)
        {
            _chestImage.sprite = null;
            _openingChest = null;
            _chestImage.gameObject.SetActive(false);

            return;
        }

        _chestImage.sprite = _globalSettings.GetChestSprite(openingChest.ChestRare);
        _openingChest = openingChest;
        _chestImage.gameObject.SetActive(true);
    }
}
