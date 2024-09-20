using TMPro;
using UnityEngine;

public class AddChest : MonoBehaviour
{
    [SerializeField] private ChestReward.ChestType _chestRare;

    [ContextMenu("Add Chest")]
    public void AddRewardToPlayer()
    {
        PlayerData.AddReward(new ChestReward(_chestRare));
    }

    public void AddChestWithButton(TMP_Dropdown dropdown)
    {
        _chestRare = (ChestReward.ChestType)dropdown.value;
        AddRewardToPlayer();
    }
}
