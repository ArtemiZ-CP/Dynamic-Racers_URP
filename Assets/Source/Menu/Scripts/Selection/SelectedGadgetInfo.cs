using TMPro;
using UnityEngine;

public class SelectedGadgetInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _gadgetName;
    [SerializeField] private TMP_Text _gadgetDescription;
    [SerializeField] private TMP_Text _gadgetAcceleration;
    [SerializeField] private TMP_Text _gadgetApplications;
    [SerializeField] private TMP_Text _gadgetDistance;

    public void Select(GadgetScriptableObject gadget)
    {
        if (gadget == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        _gadgetName.text = gadget.Name;
        _gadgetDescription.text = gadget.Description;
        _gadgetAcceleration.text = (int)(gadget.SpeedMultiplier * 100) + "%";

        if (gadget.UsageCount == int.MaxValue)
        {
            _gadgetApplications.text = "Infinity";
        }
        else
        {
            _gadgetApplications.text = gadget.UsageCount.ToString();
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
