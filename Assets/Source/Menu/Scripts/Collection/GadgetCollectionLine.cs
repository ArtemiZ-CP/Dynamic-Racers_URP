using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class GadgetCollectionLine : MonoBehaviour
{
    public static readonly int GadgetsCount = 4;

    [SerializeField] private GadgetCollectionCell _gadgetCellPrefab;

    private List<GadgetCollectionCell> _gadgetCells = new();

    public IReadOnlyList<GadgetCollectionCell> GadgetCells => _gadgetCells;

    public void Init(List<Gadget> gadgets, int foundGadgetsCount)
    {
        _gadgetCells.Clear();

        for (int i = 0; i < GadgetsCount; i++)
        {
            GadgetCollectionCell gadgetCell = Instantiate(_gadgetCellPrefab, transform);

            if (i < gadgets.Count)
            {
                _gadgetCells.Add(gadgetCell);

                if (i < foundGadgetsCount)
                {
                    gadgetCell.Init(gadgets[i], isFound: true);
                }
                else
                {
                    gadgetCell.Init(gadgets[i], isFound: false);
                }
            }
            else
            {
                gadgetCell.Init();
            }
        }
    }
}
