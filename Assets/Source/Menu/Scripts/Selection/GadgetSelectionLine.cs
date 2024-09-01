using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GadgetSelectionLine : MonoBehaviour, IClickableGadget
{
    private readonly int SpeedMultiplier = Animator.StringToHash(nameof(SpeedMultiplier));

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Material _playerMaterial;
    [SerializeField] private UpgradePanel _upgradePanel;
    [SerializeField] private RectTransform _gadgetCellsParent;
    [SerializeField] private GadgetSelectionCell _gadgetCellPrefab;
    [SerializeField] private GadgetCollectionCell _gadgetCellInfo;
    [SerializeField] private SelectedGadgetInfo _selectedGadgetInfo;
    [SerializeField] private Button _startButton;
    [SerializeField] private BlickAnimation _startButtonAnimation;
    [SerializeField] private bool _showAllGadgets;

    private List<GadgetSelectionCell> _gadgetCells = new();
    private GlobalSettings _globalSettings;
    private float _contentWidth;
    private Animator _gadgetAnimator;
    private Gadget _selectedGadget;

    private void Awake()
    {
        _globalSettings = GlobalSettings.Instance; 
    }

    private void OnEnable()
    {
        if (PlayerData.PlayerGadgets.Count == 0)
        {
            _startButton.interactable = true;
        }
        else
        {
            _startButtonAnimation.DisactiveButton();
            _startButton.interactable = false;
        }

        if (_gadgetAnimator != null)
        {
            Destroy(_gadgetAnimator.gameObject);
            _gadgetAnimator = null;
        }

        _selectedGadget = null;

        InitCells();
        RunSettings.PlayerGadget = null;
        _selectedGadgetInfo.Select(null);

        _playerAnimator.SetFloat(SpeedMultiplier, _globalSettings.BaseSpeed);
        _playerAnimator.transform.GetComponentInChildren<SkinnedMeshRenderer>().material = _playerMaterial;
    }

    private void Update()
    {
        if (IsContentWindowResized())
        {
            SetContentPosition();
        }
    }

    public void Click(Gadget gadget)
    {
        if (_selectedGadget == gadget)
        {
            return;
        }

        _gadgetCellInfo.Init(gadget);
        _selectedGadgetInfo.Select(gadget);
        _selectedGadget = gadget;

        if (gadget != null && _gadgetCells.Count > 0)
        {
            ActivePlayerAnimation(gadget);

            foreach (GadgetSelectionCell cell in _gadgetCells)
            {
                cell.Deselect();
            }

            _gadgetCells.Find(gadgetCell => gadgetCell.Gadget == gadget)?.Select();
            RunSettings.PlayerGadget = gadget;
            _startButtonAnimation.ActiveButton();
            _startButton.interactable = true;
        }
    }

    private void ActivePlayerAnimation(Gadget gadget)
    {
        for (int i = 0; i < Enum.GetNames(typeof(ChunkType)).Length; i++)
        {
            GadgetChunkInfo gadgetChunkInfo = gadget.ScriptableObject.GetChunkInfo((ChunkType)(i));

            if (gadgetChunkInfo != null)
            {
                if (_gadgetAnimator != null)
                {
                    Destroy(_gadgetAnimator.gameObject);
                    _gadgetAnimator = null;
                }

                if (gadget.ScriptableObject.Prefab != null)
                {
                    if (Instantiate(gadget.ScriptableObject.Prefab, _playerAnimator.transform).TryGetComponent(out _gadgetAnimator))
                    {
                        _gadgetAnimator.SetTrigger(gadgetChunkInfo.AnimationTriggerName);
                        _gadgetAnimator.SetFloat(SpeedMultiplier, _globalSettings.BaseSpeed);
                        SkinnedMeshRenderer skinnedMeshRenderer = _gadgetAnimator.transform.GetComponentInChildren<SkinnedMeshRenderer>();

                        if (skinnedMeshRenderer != null)
                        {
                            skinnedMeshRenderer.material = _playerMaterial;
                        }
                    }
                }

                _playerAnimator.SetTrigger(gadgetChunkInfo.AnimationTriggerName);
                _upgradePanel.SetPlayerAnimation(gadget.ScriptableObject, gadgetChunkInfo);
                return;
            }
        }
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
            gadgetCell.Init(_foundGadgets[_foundGadgets.Count - i - 1], isFound: true, clickableGadget: this);
            _gadgetCells.Add(gadgetCell);
        }

        if (_showAllGadgets)
        {
            for (int i = 0; i < _notFoundGadgets.Count; i++)
            {
                GadgetSelectionCell gadgetCell = Instantiate(_gadgetCellPrefab, _gadgetCellsParent);
                gadgetCell.Init(_notFoundGadgets[_notFoundGadgets.Count - i - 1], isFound: false, clickableGadget: null);
            }
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
