using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GadgetCellArea))]
public class SelectGadgetArea : MonoBehaviour, ISelectableGadget
{
    private const string FoundTitle = "Found";

    private GadgetCellArea _gadgetCellArea;

    public void SelectGadget(GadgetScriptableObject gadget)
    {
        List<GadgetCellLine> gadgetLines = _gadgetCellArea.GadgetLines;

        if (gadget != null && gadgetLines.Count > 0)
        {
            foreach (GadgetCellLine cell in gadgetLines)
            {
                cell.Deselect();
            }

            GadgetCellLine gadgetCellLine = gadgetLines.Find(gadgetLine => gadgetLine.GadgetCells.Exists(gadgetCell => gadgetCell.Gadget == gadget));
            gadgetCellLine.GadgetCells.Find(gadgetCell => gadgetCell.Gadget == gadget).Select();
            RunSettings.PlayerGadget = gadget;
        }
    }

    private void Awake()
    {
        _gadgetCellArea = GetComponent<GadgetCellArea>();
    }

    private void OnEnable()
    {
        _gadgetCellArea.InitCells(PlayerProgress.PlayerGadgets.ToList(), this);
        SelectGadget(RunSettings.PlayerGadget);
    }
}
