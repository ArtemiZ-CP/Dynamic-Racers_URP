using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectGadgetLine : MonoBehaviour, IClickableGadget
{
    [SerializeField] private RectTransform _gadgetCellParent;
    [SerializeField] private GadgetCell _gadgetCellPrefab;
    [SerializeField] private SelectedGadgetInfo _selectedGadgetInfo;

    private List<Gadget> _gadgets;
    private List<GadgetCell> _gadgetCells = new();
    private float _contentWidth;

    public void Click(GadgetScriptableObject gadget)
    {
        _selectedGadgetInfo.Select(gadget);

        if (gadget != null && _gadgetCells.Count > 0)
        {
            foreach (GadgetCell cell in _gadgetCells)
            {
                cell.Deselect();
            }

            _gadgetCells.Find(gadgetCell => gadgetCell.Gadget.GadgetScriptableObject == gadget)?.Select();
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
        Click(RunSettings.PlayerGadget);
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
            gadgetCell.Init(_gadgets[i], clickableGadget: this, fixVerticalSize: false);
            _gadgetCells.Add(gadgetCell);
        }

        _contentWidth = 0;

        Click(null);
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

        if (content.rect.width < Screen.width)
        {
            content.position = new Vector3(Screen.width / 2, content.position.y, content.position.z);
        }
        else
        {
            content.anchoredPosition = new Vector3(content.rect.width / 2, content.localPosition.y, content.localPosition.z);
        }
    }
}
