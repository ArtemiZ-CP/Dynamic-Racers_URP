using TMPro;
using UnityEngine;

public class SelectedGadgetInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _gadgetName;
    [SerializeField] private TMP_Text _gadgetDescription;
    [SerializeField] private TMP_Text _gadgetAcceleration;
    [SerializeField] private TMP_Text _gadgetApplications;
    [SerializeField] private TMP_Text _gadgetDistance;
    [SerializeField] private GameObject _selectedGadgetInfo;

    public void Select(Gadget gadget)
    {
        if (gadget == null)
        {
            _selectedGadgetInfo.SetActive(false);
            return;
        }

        _selectedGadgetInfo.SetActive(true);

        _gadgetName.text = gadget.ScriptableObject.Name;
        _gadgetDescription.text = gadget.ScriptableObject.Description;
        _gadgetAcceleration.text = (int)(gadget.SpeedMultiplier * 100) + "%";

        if (gadget.ScriptableObject.UsageCount == int.MaxValue)
        {
            _gadgetApplications.text = "Infinity";
        }
        else
        {
            _gadgetApplications.text = gadget.ScriptableObject.UsageCount.ToString();
        }

        if (gadget.ScriptableObject.DistanceToDisactive == float.MaxValue)
        {
            _gadgetDistance.text = "Infinity";
        }
        else
        {
            _gadgetDistance.text = gadget.ScriptableObject.DistanceToDisactive + "m";
        }
    }
}
