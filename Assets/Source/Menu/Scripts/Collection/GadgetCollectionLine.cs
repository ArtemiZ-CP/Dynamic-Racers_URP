using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(HorizontalLayoutGroup))]
public class GadgetCollectionLine : MonoBehaviour
{
    [SerializeField] private GadgetCollectionCell _gadgetCellPrefab;

    private HorizontalLayoutGroup _layoutGroup;
    private List<GadgetCollectionCell> _gadgetCells = new();
    private float _gadgetsWidth;

    public IReadOnlyList<GadgetCollectionCell> GadgetCells => _gadgetCells;
    public int GadgetsCount { get; private set; }

    private void Awake()
    {
        _layoutGroup = GetComponent<HorizontalLayoutGroup>();
        _gadgetsWidth = _gadgetCellPrefab.GetComponent<RectTransform>().sizeDelta.x;
        GadgetsCount = GetGadgetsCountInLine();
    }

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
                    gadgetCell.Initialize(gadgets[i], isFound: true);
                }
                else
                {
                    gadgetCell.Initialize(gadgets[i], isFound: false);
                }
            }
            else
            {
                gadgetCell.Initialize();
            }
        }
    }

    private int GetGadgetsCountInLine()
    {
        float screenWidth = Screen.width;
        int gadgetsCount = 1;

        while (GetGadgetsWidth(gadgetsCount) < screenWidth)
        {
            gadgetsCount++;
        }

        return gadgetsCount;
    }
    
    private float GetGadgetsWidth(int gadgetsCount)
    {
        float width = 0;
        width += _layoutGroup.padding.left;
        width += _layoutGroup.padding.right;
        width += _layoutGroup.spacing * (gadgetsCount - 1);
        width += gadgetsCount * _gadgetsWidth;
        return width;
    }
}
