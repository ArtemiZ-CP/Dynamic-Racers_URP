using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GadgetSelectionLine : MonoBehaviour, IClickableGadget
{
    [SerializeField] private RectTransform _gadgetCellsParent;
    [SerializeField] private GadgetSelectionCell _gadgetCellPrefab;
    [SerializeField] private SelectedGadgetInfo _selectedGadgetInfo;

    private List<GadgetSelectionCell> _gadgetCells = new();
    private GlobalSettings _globalSettings;
    private float _contentWidth;

    public void Click(GadgetScriptableObject gadget)
    {
        _selectedGadgetInfo.Select(gadget);

        if (gadget != null && _gadgetCells.Count > 0)
        {
            foreach (GadgetSelectionCell cell in _gadgetCells)
            {
                cell.Deselect();
            }

            _gadgetCells.Find(gadgetCell => gadgetCell.Gadget.GadgetScriptableObject == gadget)?.Select();
            RunSettings.PlayerGadget = gadget;
        }
    }

    private void Awake()
    {
        _globalSettings = GlobalSettings.Instance;
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
        List<Gadget> _foundGadgets = PlayerData.PlayerGadgets.ToList();
        List<Gadget> _notFoundGadgets = _globalSettings.GetNotFoundGadgets();

        _gadgetCells.Clear();

        while (_gadgetCellsParent.childCount > 0)
        {
            DestroyImmediate(_gadgetCellsParent.GetChild(0).gameObject);
        }

        for (int i = 0; i < _foundGadgets.Count; i++)
        {
            GadgetSelectionCell gadgetCell = Instantiate(_gadgetCellPrefab, _gadgetCellsParent);
            gadgetCell.Init(_foundGadgets[i], isFound: true, clickableGadget: this);
            _gadgetCells.Add(gadgetCell);
        }

        for (int i = 0; i < _notFoundGadgets.Count; i++)
        {
            GadgetSelectionCell gadgetCell = Instantiate(_gadgetCellPrefab, _gadgetCellsParent);
            gadgetCell.Init(_notFoundGadgets[i], isFound: false, clickableGadget: null);
        }

        _contentWidth = 0;

        Click(RunSettings.PlayerGadget);
    }

    private bool IsContentWindowResized()
    {
        float newWidth = _gadgetCellsParent.rect.width;

        if (newWidth != _contentWidth)
        {
            _contentWidth = newWidth;
            return true;
        }

        return false;
    }

    private void SetContentPosition()
    {
        RectTransform content = _gadgetCellsParent.GetComponent<RectTransform>();

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
