using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GadgetsCollection))]
public class SelectGadgetArea : MonoBehaviour, IClickableGadget
{
    private const string FoundTitle = "Found";

    private GadgetsCollection _gadgetCellArea;

    public void Click(GadgetScriptableObject gadget)
    {
        List<GadgetCellLine> gadgetLines = _gadgetCellArea.GadgetLines;

        if (gadget != null && gadgetLines.Count > 0)
        {
            foreach (GadgetCellLine cell in gadgetLines)
            {
                cell.Deselect();
            }

            GadgetCellLine gadgetCellLine = 
            gadgetLines.Find(gadgetLine => 
            gadgetLine.GadgetCells.ToList().Exists(gadgetCell => 
            gadgetCell.Gadget.GadgetScriptableObject == gadget));

            gadgetCellLine.GadgetCells.ToList().Find(gadgetCell => 
            gadgetCell.Gadget.GadgetScriptableObject == gadget).Select();
            
            RunSettings.PlayerGadget = gadget;
        }
    }

    private void Awake()
    {
        _gadgetCellArea = GetComponent<GadgetsCollection>();
    }

    private void OnEnable()
    {
        _gadgetCellArea.InitCells(PlayerProgress.PlayerGadgets.ToList(), this);
        Click(RunSettings.PlayerGadget);
    }
}
