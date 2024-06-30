using TMPro;
using UnityEngine;

public class SelectedGadgetInfo : MonoBehaviour
{
    [SerializeField] private GameObject _withoutGadgetWindow;
    [SerializeField] private GameObject _withGadgetWindow;
    [SerializeField] private TMP_Text _gadgetName;
    [SerializeField] private TMP_Text _gadgetDescription;
    [SerializeField] private TMP_Text _gadgetAcceleration;
    [SerializeField] private TMP_Text _gadgetApplications;
    [SerializeField] private TMP_Text _gadgetDistance;

    public void Select(GadgetScriptableObject gadget)
    {
        if (gadget == null)
        {
            _withoutGadgetWindow.SetActive(true);
            _withGadgetWindow.SetActive(false);
            return;
        }

        _withoutGadgetWindow.SetActive(false);
        _withGadgetWindow.SetActive(true);

        _gadgetName.text = gadget.Name;
        _gadgetDescription.text = gadget.Description;
        _gadgetAcceleration.text = (int)(gadget.SpeedMultiplier * 100) + "%";

        if (gadget.ActiveCount == int.MaxValue)
        {
            _gadgetApplications.text = "Infinity";
        }
        else
        {
            _gadgetApplications.text = gadget.ActiveCount.ToString();
        }

        if (gadget.DistanceToDisactive == float.MaxValue)
        {
            _gadgetDistance.text = "Infinity";
        }
        else
        {
            _gadgetDistance.text = gadget.DistanceToDisactive + "m";
        }
    }
}
