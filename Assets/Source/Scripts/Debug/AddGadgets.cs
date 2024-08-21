using UnityEngine;
using TMPro;

public class AddGadgets : MonoBehaviour
{
    [SerializeField] private int _gadgetCount;

    [ContextMenu("Give Gadget")]
    public void GiveGadget()
    {
        foreach (Gadget gadget in GlobalSettings.Instance.GetAllGadgets())
        {
            PlayerData.AddGadget(new Gadget(gadget.GadgetScriptableObject, _gadgetCount));
        }
    }

    public void GiveGadgetWithButton(TMP_InputField inputField)
    {
        if (int.TryParse(inputField.text, out int gadgetCount))
        {
            foreach (Gadget gadget in GlobalSettings.Instance.GetAllGadgets())
            {
                PlayerData.AddGadget(new Gadget(gadget.GadgetScriptableObject, gadgetCount));
            }
        }
        else
        {
            foreach (Gadget gadget in GlobalSettings.Instance.GetAllGadgets())
            {
                PlayerData.AddGadget(new Gadget(gadget));
            }
        }
    }
}
