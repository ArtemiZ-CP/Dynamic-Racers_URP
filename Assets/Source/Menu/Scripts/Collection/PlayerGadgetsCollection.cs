using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GadgetCellArea))]
public class PlayerGadgetsCollection : MonoBehaviour
{
    private GadgetCellArea _gadgetCellArea;

    private void Awake()
    {
        _gadgetCellArea = GetComponent<GadgetCellArea>();
    }

    private void OnEnable()
    {
        _gadgetCellArea.InitCells(PlayerProgress.PlayerGadgets.ToList(), null);
    }
}
