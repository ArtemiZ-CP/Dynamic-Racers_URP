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

    public void Init(Gadget gadget, bool isFound = true, IClickableGadget clickableGadget = null)
    {
        Gadget = gadget;
        _image.sprite = gadget.ScriptableObject.BigSprite;

        if (clickableGadget != null)
        {
            _button.onClick.AddListener(() => clickableGadget.Click(gadget));
        }

        if (isFound)
        {
            _levelText.gameObject.SetActive(true);
            _levelText.text = $"Level {gadget.Level + 1}";
            _notFound.SetActive(false);
        }
        else
        {
            _levelText.gameObject.SetActive(false);
            _notFound.SetActive(true);
        }

        _recomendedWindow.SetActive(false);
        Deselect();
    }

    public void UpdateCell()
    {
        _levelText.text = $"Level {Gadget.Level + 1}";
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
