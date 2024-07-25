using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GadgetsCollection))]
public class PlayerGadgetsCollection : MonoBehaviour
{
    private GadgetsCollection _gadgetCellArea;

    private void Awake()
    {
        _gadgetCellArea = GetComponent<GadgetsCollection>();
    }

    private void OnEnable()
    {
        _gadgetCellArea.InitCells(PlayerProgress.PlayerGadgets.ToList(), null);
    }
}
