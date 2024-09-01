using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GadgetSelectionCell : MonoBehaviour
{
    [SerializeField] private GameObject[] _select;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _notFound;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private GameObject _recomendedWindow;

    public Gadget Gadget { get; private set; }

    public void Init()
    {
        _image.gameObject.SetActive(false);
        _notFound.SetActive(false);
        _recomendedWindow.SetActive(false);
        Deselect();
    }

    public void Init(Gadget gadget)
    {
        Gadget = gadget;
        _image.sprite = gadget.ScriptableObject.BigSprite;
        Deselect();

        _notFound.SetActive(false);
        _recomendedWindow.SetActive(false);
    }

    public void Init(Gadget gadget, bool isFound, IClickableGadget clickableGadget)
    {
        Init(gadget);

        if (clickableGadget != null)
        {
            _button.onClick.AddListener(() => clickableGadget.Click(gadget));
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
}
