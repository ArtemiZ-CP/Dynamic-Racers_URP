using UnityEngine;
using UnityEngine.UI;

public class GadgetCell : MonoBehaviour
{
    [SerializeField] private GameObject _glow;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    public GadgetScriptableObject Gadget { get; private set; }

    public void Init(GadgetScriptableObject gadget, ISelectableGadget clickGadget)
    {
        Gadget = gadget;
        _image.sprite = gadget.Icon;
        Deselect();

        if (clickGadget != null)
        {
            _button.onClick.AddListener(() => clickGadget.SelectGadget(gadget));
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
}
