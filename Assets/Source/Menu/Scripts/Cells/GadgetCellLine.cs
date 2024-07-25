using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class GadgetCellLine : MonoBehaviour
{
    public static readonly int GadgetsCount = 4;

    [SerializeField] private GadgetCell _gadgetCellPrefab;

    private List<Gadget> _gadgets;
    private List<GadgetCell> _gadgetCells = new();
    private RectTransform _gadgetCellLine;
    private int _gadgetsCount;

    public IReadOnlyList<GadgetCell> GadgetCells => _gadgetCells;

    public void Init(List<Gadget> gadgets, int foundGadgetsCount, IClickableGadget clickGadget)
    {
        _gadgetCells.Clear();
        _gadgets = gadgets;
        _gadgetsCount = _gadgets.Count;

        for (int i = 0; i < _gadgets.Count; i++)
        {
            GadgetCell gadgetCell = Instantiate(_gadgetCellPrefab, transform);
            _gadgetCells.Add(gadgetCell);

            if (i < foundGadgetsCount)
            {
                gadgetCell.Init(_gadgets[i], true, clickGadget, fixVerticalSize: true);
            }
            else
            {
                gadgetCell.Init(_gadgets[i], false, clickGadget, fixVerticalSize: true);
            }
        }
    }

    public void Deselect()
    {
        foreach (GadgetCell gadgetCell in GetComponentsInChildren<GadgetCell>())
        {
            gadgetCell.Deselect();
        }
    }

    private void Awake()
    {
        _gadgetCellLine = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Resize();
    }

    private void Resize()
    {
        float width = transform.parent.GetComponent<RectTransform>().rect.width / ((float)GadgetsCount / _gadgetsCount);
        _gadgetCellLine.sizeDelta = new Vector2(width, _gadgetCellLine.sizeDelta.y);
    }
}
