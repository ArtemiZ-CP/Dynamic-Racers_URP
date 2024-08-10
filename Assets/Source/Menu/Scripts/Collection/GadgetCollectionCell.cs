using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GadgetCollectionCell : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _background;
    [SerializeField] private Image _gadget;
    [SerializeField] private GameObject _notFound;
    [SerializeField] private GadgetBar _countBar;
    [SerializeField] private TMP_Text _levelText;

    public Gadget Gadget { get; private set; }
    public bool IsFound { get; private set; }
    public event Action<GadgetCollectionCell> OnClick;

    public void Init()
    {
        _background.gameObject.SetActive(false);
        _countBar.gameObject.SetActive(false);
    }

    public void Init(Gadget gadget, bool isFound)
    {
        SetGadget(gadget);

        IsFound = isFound;

        SetFill(isFound);
    }

    public void UpdateGadget()
    {
        SetFill(IsFound);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Select);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Select);
    }

    private void Select()
    {
        OnClick?.Invoke(this);
    }

    private void SetFill(bool isFound)
    {
        if (isFound)
        {
            _countBar.gameObject.SetActive(true);
            _notFound.SetActive(false);
            _countBar.SetFill(Gadget);
            _levelText.text = $"Level {Gadget.Level + 1}";
        }
        else
        {
            _countBar.gameObject.SetActive(false);
            _notFound.SetActive(true);
        }
    }

    private void SetGadget(Gadget gadget)
    {
        Gadget = gadget;
        _gadget.sprite = gadget.GadgetScriptableObject.Sprite;
        _background.sprite = GlobalSettings.Instance.GetGadgetRareBackground(gadget.GadgetScriptableObject.Rare);
    }
}
