using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectGadgetLine : MonoBehaviour, ISelectableGadget
{
    [SerializeField] private RectTransform _gadgetCellParent;
    [SerializeField] private GadgetCell _gadgetCellPrefab;
    [SerializeField] private SelectedGadgetInfo _selectedGadgetInfo;

    private List<GadgetScriptableObject> _gadgets;
    private List<GadgetCell> _gadgetCells = new();
    private float _contentWidth;

    public void SelectGadget(GadgetScriptableObject gadget)
    {
        _selectedGadgetInfo.Select(gadget);

        if (gadget != null && _gadgetCells.Count > 0)
        {
            foreach (GadgetCell cell in _gadgetCells)
            {
                cell.Deselect();
            }

            _gadgetCells.Find(gadgetCell => gadgetCell.Gadget == gadget)?.Select();
            RunSettings.PlayerGadget = gadget;
        }
    }

    private void Update()
    {
        if (IsContentWindowResized())
        {
            SetContentPosition();
        }
    }

    private void OnEnable()
    {
        InitCells();
        SelectGadget(RunSettings.PlayerGadget);
    }

    private void InitCells()
    {
        _gadgets = PlayerProgress.PlayerGadgets.ToList();
        _gadgetCells.Clear();

        while (_gadgetCellParent.childCount > 0)
        {
            DestroyImmediate(_gadgetCellParent.GetChild(0).gameObject);
        }

        for (int i = 0; i < _gadgets.Count; i++)
        {
            GadgetCell gadgetCell = Instantiate(_gadgetCellPrefab, _gadgetCellParent);
            gadgetCell.Init(_gadgets[i], this);
            _gadgetCells.Add(gadgetCell);
        }

        _contentWidth = 0;
        
        SelectGadget(null);
    }

    private bool IsContentWindowResized()
    {
        float newWidth = _gadgetCellParent.rect.width;

        if (newWidth != _contentWidth)
        {
            _contentWidth = newWidth;
            return true;
        }

        return false;
    }

    private void SetContentPosition()
    {
        RectTransform content = _gadgetCellParent.GetComponent<RectTransform>();
        content.anchoredPosition = new Vector3(content.rect.width / 2, content.localPosition.y, content.localPosition.z);
    }
}
