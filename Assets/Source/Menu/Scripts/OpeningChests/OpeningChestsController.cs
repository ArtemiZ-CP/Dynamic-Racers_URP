using UnityEngine;

public class OpeningChestsController : MonoBehaviour
{
    [SerializeField] private OpeningChestUI[] _openingChestsUI = new OpeningChestUI[4];

    private OpeningChestUI _openingChestUI;

    private void OnEnable()
    {
        ViewChests();
    }

    public void ClickOnChest(OpeningChestUI openingChestUI)
    {
        if (openingChestUI == null || openingChestUI.HasChest == false)
        {
            return;
        }

        if (openingChestUI.TryOpen())
        {
            return;
        }

        if (_openingChestUI == null || _openingChestUI.IsOpened)
        {
            _openingChestUI = openingChestUI;
            _openingChestUI.StartOpening();
        }
    }

    public void ViewChests()
    {
        for (int i = 0; i < _openingChestsUI.Length; i++)
        {
            if (_openingChestsUI[i] != null)
            {
                _openingChestsUI[i].ViewChest(PlayerData.OpeningChests[i]);
            }
        }
    }
}
