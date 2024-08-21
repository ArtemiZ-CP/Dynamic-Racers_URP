using TMPro;
using UnityEngine;

public class AddChest : MonoBehaviour
{
    [SerializeField] private ChestReward.ChestType _chestRare;
    [SerializeField] private OpeningChestsController _openingChestsController;

    [ContextMenu("Add Chest")]
    public void AddRewardToPlayer()
    {
        PlayerData.AddOpeningChest(_chestRare);
        _openingChestsController.ViewChests();
    }

    public void AddChestWithButton(TMP_Dropdown dropdown)
    {
        _chestRare = (ChestReward.ChestType)dropdown.value;
        PlayerData.AddOpeningChest(_chestRare);
        _openingChestsController.ViewChests();
    }
}
