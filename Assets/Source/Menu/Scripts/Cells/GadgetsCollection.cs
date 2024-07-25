using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GadgetsCollection : MonoBehaviour
{
    [SerializeField] private RectTransform _gadgetCellParent;
    [SerializeField] private TitleCellLine _titleCellLinePrefab;
    [SerializeField] private GadgetCellLine _gadgetCellLinePrefab;
    [SerializeField] private GameObject _emptyLine;

    private List<GadgetCellLine> _gadgetLines = new();
    private float _contentHeight;

    public List<GadgetCellLine> GadgetLines => _gadgetLines;

    public void InitCells(List<Gadget> gadgets, IClickableGadget clickGadget)
    {
        ClearCells();
        InstantiateCells(clickGadget, gadgets);

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

    private void InstantiateCells(IClickableGadget clickGadget, List<Gadget> gadgets)
    {
        if (_titleCellLinePrefab != null)
        {
            Instantiate(_titleCellLinePrefab, _gadgetCellParent);
        }

        List<Gadget> gadgetsInLine = new();
        GadgetCellLine gadgetCellLine = Instantiate(_gadgetCellLinePrefab, _gadgetCellParent);
        int foundGadgetsCount = 0;

        for (int spawnedGadgets = 0; spawnedGadgets < GlobalSettings.Instance.AllGadgets.Count; spawnedGadgets++)
        {
            if (spawnedGadgets < gadgets.Count)
            {
                foundGadgetsCount++;
                gadgetsInLine.Add(gadgets[spawnedGadgets]);
            }
            else
            {
                gadgetsInLine.Add(new Gadget(GetNotFoundGadget(gadgets, spawnedGadgets - gadgets.Count)));
            }

            if (spawnedGadgets == GlobalSettings.Instance.AllGadgets.Count - 1)
            {
                gadgetCellLine.Init(gadgetsInLine, foundGadgetsCount, clickGadget);
                _gadgetLines.Add(gadgetCellLine);
            }
            else if (spawnedGadgets % GadgetCellLine.GadgetsCount == GadgetCellLine.GadgetsCount - 1)
            {
                gadgetCellLine.Init(gadgetsInLine, foundGadgetsCount, clickGadget);
                _gadgetLines.Add(gadgetCellLine);

                gadgetsInLine = new();
                gadgetCellLine = Instantiate(_gadgetCellLinePrefab, _gadgetCellParent);
                foundGadgetsCount = 0;
            }
        }

        if (_emptyLine != null)
        {
            Instantiate(_emptyLine, _gadgetCellParent);
        }
    }

    private GadgetScriptableObject GetNotFoundGadget(List<Gadget> foundGadgets, int gadgetIndex)
    {
        int notFoundIndex = -1;

        for (int i = 0; i < GlobalSettings.Instance.AllGadgets.Count; i++)
        {
            if (foundGadgets.Any(g => g.GadgetScriptableObject == GlobalSettings.Instance.AllGadgets[i]) == false)
            {
                notFoundIndex++;

                if (notFoundIndex == gadgetIndex)
                {
                    return GlobalSettings.Instance.AllGadgets[i];
                }
            }
        }

        return null;
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
