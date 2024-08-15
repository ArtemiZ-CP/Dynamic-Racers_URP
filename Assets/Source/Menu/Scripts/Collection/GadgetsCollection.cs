using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GadgetsCollection : MonoBehaviour
{
    [SerializeField] private RectTransform _gadgetCellParent;
    [SerializeField] private TitleCellLine _titleCellLinePrefab;
    [SerializeField] private GadgetCollectionLine _gadgetCollectionLinePrefab;
    [SerializeField] private GameObject _emptyLine;

    private GlobalSettings _globalSettings;
    private List<GadgetCollectionCell> _gadgetCells = new();

    private void Awake()
    {
        _globalSettings = GlobalSettings.Instance;
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        foreach (GadgetCollectionCell gadgetCell in _gadgetCells)
        {
            gadgetCell.OnClick -= OnClickGadget;
        }
    }

    private void Initialize()
    {
        Clear();
        AddGadgets();

        foreach (GadgetCollectionCell gadgetCell in _gadgetCells)
        {
            gadgetCell.OnClick += OnClickGadget;
        }
    }

    private void OnClickGadget(GadgetCollectionCell gadgetCell)
    {
        if (gadgetCell.IsFound)
        {
            if (gadgetCell.Gadget.TryLevelUp())
            {
                gadgetCell.UpdateGadget();
            }
        }
    }

    private void Clear()
    {
        _gadgetCells.Clear();
        
        while (_gadgetCellParent.childCount > 0)
        {
            DestroyImmediate(_gadgetCellParent.GetChild(0).gameObject);
        }
    }

    private void AddGadgets()
    {
        AddTitle();

        int allGadgetsCount = _globalSettings.GetAllGadgets().Count;

        List<Gadget> playerGadgets = PlayerData.PlayerGadgets.ToList();

        GadgetCollectionLine gadgetCellLine = Instantiate(_gadgetCollectionLinePrefab, _gadgetCellParent);
        List<Gadget> gadgetsInLine = new();
        int foundGadgetsCount = 0;

        for (int spawnedGadgets = 0; spawnedGadgets < allGadgetsCount; spawnedGadgets++)
        {
            if (spawnedGadgets < playerGadgets.Count)
            {
                gadgetsInLine.Add(playerGadgets[spawnedGadgets]);
                foundGadgetsCount++;
            }
            else
            {
                gadgetsInLine.Add(new Gadget(GetNotFoundGadget(playerGadgets, spawnedGadgets - playerGadgets.Count)));
            }

            if (spawnedGadgets == allGadgetsCount - 1)
            {
                gadgetCellLine.Init(gadgetsInLine, foundGadgetsCount);
            }
            else if (spawnedGadgets % gadgetCellLine.GadgetsCount == gadgetCellLine.GadgetsCount - 1)
            {
                gadgetCellLine.Init(gadgetsInLine, foundGadgetsCount);
                _gadgetCells.AddRange(gadgetCellLine.GadgetCells);

                gadgetCellLine = Instantiate(_gadgetCollectionLinePrefab, _gadgetCellParent);
                gadgetsInLine = new();
                foundGadgetsCount = 0;
            }
        }

        _gadgetCells.AddRange(gadgetCellLine.GadgetCells);

        AddEmptyLine();
    }

    private void AddTitle()
    {
        if (_titleCellLinePrefab != null)
        {
            Instantiate(_titleCellLinePrefab, _gadgetCellParent);
        }
    }

    private void AddEmptyLine()
    {
        if (_emptyLine != null)
        {
            Instantiate(_emptyLine, _gadgetCellParent);
        }
    }

    private GadgetScriptableObject GetNotFoundGadget(List<Gadget> foundGadgets, int gadgetIndex)
    {
        int notFoundIndex = -1;

        List<Gadget> allGadgets = _globalSettings.GetAllGadgets();

        for (int i = 0; i < allGadgets.Count; i++)
        {
            if (foundGadgets.Any(g => g.GadgetScriptableObject == allGadgets[i].GadgetScriptableObject) == false)
            {
                notFoundIndex++;

                if (notFoundIndex == gadgetIndex)
                {
                    return allGadgets[i].GadgetScriptableObject;
                }
            }
        }

        return null;
    }
}
