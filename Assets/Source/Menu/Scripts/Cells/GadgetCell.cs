using UnityEngine;
using UnityEngine.UI;

public class GadgetCell : MonoBehaviour
{
    [SerializeField] private GameObject _glow;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    [SerializeField] private GadgetBar _countBar;
    [SerializeField] private GameObject _notFound;
    [Header("Backgrounds")]
    [SerializeField] private Image _commonImage;
    [SerializeField] private Image _rareImage;
    [SerializeField] private Image _epicImage;
    [SerializeField] private Image _legendaryImage;
    [Header("Size")]
    [SerializeField] private float _refWigth;
    [SerializeField] private float _refHeight;

    private RectTransform _rectTransform;
    private bool _fixVerticalSize;

    public Gadget Gadget { get; private set; }

    public void Init(Gadget gadget, bool fixVerticalSize)
    {
        _fixVerticalSize = fixVerticalSize;
        Gadget = gadget;
        _image.sprite = gadget.GadgetScriptableObject.Icon;
        Deselect();
        ActiveBackground(gadget.GadgetScriptableObject.Rare);

        _notFound.SetActive(false);
        _countBar.gameObject.SetActive(false);
    }

    public void Init(Gadget gadget, IClickableGadget clickableGadget, bool fixVerticalSize)
    {
        Init(gadget, fixVerticalSize);

        if (clickableGadget != null)
        {
            _button.onClick.AddListener(() => clickableGadget.Click(gadget.GadgetScriptableObject));
        }
    }

    public void Init(Gadget gadget, bool isFound, IClickableGadget clickGadget, bool fixVerticalSize)
    {
        Init(gadget, clickGadget, fixVerticalSize);

        if (isFound)
        {
            _countBar.gameObject.SetActive(true);
            _countBar.SetFill(Gadget);
        }
        else
        {
            _notFound.SetActive(true);
        }
    }

    public void Select()
    {
        _glow?.SetActive(true);
    }

    public void Deselect()
    {
        _glow?.SetActive(false);
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        FixSize();
    }

    private void FixSize()
    {
        float newWidth = _rectTransform.rect.width;
        float newHeight = _rectTransform.rect.height;

        if (_fixVerticalSize)
        {
            newHeight = _rectTransform.rect.width / _refWigth * _refHeight;
        }
        else
        {
            newWidth = _rectTransform.rect.height / _refHeight * _refWigth;
        }

        _rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
    }

    private void ActiveBackground(GadgetRare rare)
    {
        _commonImage.gameObject.SetActive(rare == GadgetRare.Common);
        _rareImage.gameObject.SetActive(rare == GadgetRare.Rare);
        _epicImage.gameObject.SetActive(rare == GadgetRare.Epic);
        _legendaryImage.gameObject.SetActive(rare == GadgetRare.Legendary);
    }
}
