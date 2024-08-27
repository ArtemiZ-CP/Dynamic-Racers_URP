using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GadgetSelectionLine : MonoBehaviour, IClickableGadget
{
    private readonly int SpeedMultiplier = Animator.StringToHash(nameof(SpeedMultiplier));

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private RectTransform _gadgetCellsParent;
    [SerializeField] private GadgetSelectionCell _gadgetCellPrefab;
    [SerializeField] private SelectedGadgetInfo _selectedGadgetInfo;
    [SerializeField] private Button _startButton;
    [SerializeField] private Material _playerMaterial;

    private List<GadgetSelectionCell> _gadgetCells = new();
    private GlobalSettings _globalSettings;
    private float _contentWidth;
    private Animator _gadgetAnimator;
    private GadgetScriptableObject _selectedGadget;

    private void Awake()
    {
        _globalSettings = GlobalSettings.Instance;

        if (PlayerData.PlayerGadgets.Count == 0)
        {
            _startButton.interactable = true;
        }
        else
        {
            _startButton.interactable = false;
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

    public void Click(GadgetScriptableObject gadget)
    {
        if (_selectedGadget == gadget)
        {
            return;
        }

        _selectedGadgetInfo.Select(gadget);
        _selectedGadget = gadget;

        if (gadget != null && _gadgetCells.Count > 0)
        {
            ActivePlayerAnimation(gadget);

            foreach (GadgetSelectionCell cell in _gadgetCells)
            {
                cell.Deselect();
            }

            _gadgetCells.Find(gadgetCell => gadgetCell.Gadget.GadgetScriptableObject == gadget)?.Select();
            RunSettings.PlayerGadget = gadget;
            _startButton.interactable = true;
        }
    }

    private void ActivePlayerAnimation(GadgetScriptableObject gadget)
    {
        for (int i = 0; i < Enum.GetNames(typeof(ChunkType)).Length; i++)
        {
            GadgetChunkInfo gadgetChunkInfo = gadget.GetChunkInfo((ChunkType)(i));

            if (gadgetChunkInfo != null)
            {
                if (_gadgetAnimator != null)
                {
                    Destroy(_gadgetAnimator.gameObject);
                    _gadgetAnimator = null;
                }

                if (gadget.Prefab != null)
                {
                    if (Instantiate(gadget.Prefab, _playerAnimator.transform).TryGetComponent(out _gadgetAnimator))
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
