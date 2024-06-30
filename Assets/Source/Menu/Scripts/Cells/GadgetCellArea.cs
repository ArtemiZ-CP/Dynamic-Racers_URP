using System.Collections.Generic;
using UnityEngine;

public class GadgetCellArea : MonoBehaviour
{
    private const string FoundTitle = "Found";

    [SerializeField] private RectTransform _gadgetCellParent;
    [SerializeField] private GadgetCellLine _gadgetCellLinePrefab;
    [SerializeField] private TitleCellLine _titleCellLinePrefab;

    private List<GadgetCellLine> _gadgetLines = new();
    private List<GadgetScriptableObject> _gadgets = new();
    private float _contentHeight;

    public List<GadgetCellLine> GadgetLines => _gadgetLines;

    public void AddGadgets(List<GadgetScriptableObject> gadgets, ISelectableGadget clickGadget)
    {
        _gadgets.AddRange(gadgets);

        ClearCells();
        InstantiateCells(clickGadget);

        _contentHeight = 0;
    }

    public void InitCells(List<GadgetScriptableObject> gadgets, ISelectableGadget clickGadget)
    {
        _gadgets = gadgets;

        ClearCells();
        InstantiateCells(clickGadget);

        _contentHeight = 0;
    }

    private void Update()
    {
        if (IsContentWindowResized())
        {
            SetContentPosition();
        }
    }

    private void ClearCells()
    {
        _gadgetLines.Clear();

        while (_gadgetCellParent.childCount > 0)
        {
            DestroyImmediate(_gadgetCellParent.GetChild(0).gameObject);
        }
    }

    private void InstantiateCells(ISelectableGadget clickGadget)
    {
        if (_titleCellLinePrefab != null)
        {
            TitleCellLine titlePrefab = Instantiate(_titleCellLinePrefab, _gadgetCellParent);
            titlePrefab.Init(FoundTitle);
        }

        List<GadgetScriptableObject> gadgetsInLine = new();
        GadgetCellLine gadgetCellLine = Instantiate(_gadgetCellLinePrefab, _gadgetCellParent);

        for (int spawnedGadgets = 0; spawnedGadgets < _gadgets.Count; spawnedGadgets++)
        {
            gadgetsInLine.Add(_gadgets[spawnedGadgets]);

            if (spawnedGadgets % GadgetCellLine.GadgetsCount == GadgetCellLine.GadgetsCount - 1)
            {
                gadgetCellLine.Init(gadgetsInLine, clickGadget);
                _gadgetLines.Add(gadgetCellLine);

                gadgetsInLine = new();
                gadgetCellLine = Instantiate(_gadgetCellLinePrefab, _gadgetCellParent);
            }
            else if (spawnedGadgets == _gadgets.Count - 1)
            {
                gadgetCellLine.Init(gadgetsInLine, clickGadget);
                _gadgetLines.Add(gadgetCellLine);
            }
        }
    }

    private bool IsContentWindowResized()
    {
        float newHeight = _gadgetCellParent.rect.height;

        if (newHeight != _contentHeight)
        {
            _contentHeight = newHeight;
            return true;
        }

        return false;
    }

    private void SetContentPosition()
    {
        RectTransform content = _gadgetCellParent.GetComponent<RectTransform>();
        content.anchoredPosition = new Vector3(content.localPosition.x, 0, content.localPosition.z);
    }
}
