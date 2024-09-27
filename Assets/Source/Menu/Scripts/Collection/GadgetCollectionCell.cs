using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GadgetCollectionCell : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _gadget;
    [SerializeField] private GameObject _notFound;
    [SerializeField] private GadgetBar _countBar;
    [SerializeField] private TMP_Text _levelText;

    public Gadget Gadget { get; private set; }
    public bool IsFound { get; private set; }
    public event Action<GadgetCollectionCell> OnClick;

    private void OnEnable()
    {
        _button?.onClick.AddListener(Select);
    }

    private void OnDisable()
    {
        _button?.onClick.RemoveListener(Select);
    }

    public void Initialize()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void Initialize(Gadget gadget, bool isFound = true)
    {
        SetGadget(gadget);

        IsFound = isFound;

        SetFill(isFound);
    }

    public void UpdateGadget()
    {
        SetFill(IsFound);
    }

    private void Select()
    {
        OnClick?.Invoke(this);
    }

    private void SetFill(bool isFound)
    {
        if (isFound)
        {
            if (_countBar != null) _countBar.gameObject.SetActive(true);
            if (_levelText != null) _levelText.gameObject.SetActive(true);
            if (_notFound != null) _notFound.SetActive(false);
            if (_countBar != null) _countBar.SetFill(Gadget);
            if (_levelText != null) _levelText.text = $"Level {Gadget.Level + 1}";
        }
        else
        {
            if (_countBar != null) _countBar.gameObject.SetActive(false);
            if (_levelText != null) _levelText.gameObject.SetActive(false);
            _notFound?.SetActive(true);
        }
    }

    private void SetGadget(Gadget gadget)
    {
        Gadget = gadget;
        _gadget.sprite = gadget.ScriptableObject.SmallSprite;
    }
}
