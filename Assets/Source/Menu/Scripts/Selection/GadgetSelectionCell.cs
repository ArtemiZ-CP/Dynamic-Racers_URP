using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GadgetSelectionCell : MonoBehaviour
{
    [SerializeField] private GameObject[] _select;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _notFound;
    [SerializeField] private Image _background;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private GameObject _recomendedWindow;

    public Gadget Gadget { get; private set; }

    public void Init()
    {
        _background.gameObject.SetActive(false);
        _notFound.SetActive(false);
        _recomendedWindow.SetActive(false);
        Deselect();
    }

    public void Init(Gadget gadget)
    {
        Gadget = gadget;
        _image.sprite = gadget.GadgetScriptableObject.Sprite;
        Deselect();
        ActiveBackground(gadget.GadgetScriptableObject.Rare);

        _notFound.SetActive(false);
        _recomendedWindow.SetActive(false);
    }

    public void Init(Gadget gadget, bool isFound, IClickableGadget clickableGadget)
    {
        Init(gadget);

        if (clickableGadget != null)
        {
            _button.onClick.AddListener(() => clickableGadget.Click(gadget.GadgetScriptableObject));
        }
        if (isFound)
        {
            _levelText.gameObject.SetActive(true);
            _levelText.text = $"Level {gadget.Level + 1}";
        }
        else
        {
            _levelText.gameObject.SetActive(false);
            _notFound.SetActive(true);
        }
    }

    public void Select()
    {
        foreach (var item in _select)
        {
            item?.SetActive(true);
        }
    }

    public void Deselect()
    {
        foreach (var item in _select)
        {
            item?.SetActive(false);
        }
    }

    public void SetRecomended()
    {
        _recomendedWindow.SetActive(true);
    }

    private void ActiveBackground(Rare rare)
    {
        _background.sprite = GlobalSettings.Instance.GetGadgetRareBackground(rare);
    }
}
