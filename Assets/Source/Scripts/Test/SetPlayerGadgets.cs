using System.Collections.Generic;
using UnityEngine;

public class SetPlayerGadgets : MonoBehaviour
{
    [SerializeField] private List<GadgetScriptableObject> _gadgets;
    [SerializeField] private bool _isAddGadgets;

    private void Awake()
    {
        if (_gadgets == null)
        {
            return;
        }

        if (_isAddGadgets)
        {
            foreach (Gadget gadget in GlobalSettings.Instance.GetAllGadgets())
            {
                PlayerData.AddGadget(gadget);
            }
        }
        else
        {
            foreach (GadgetScriptableObject gadget in _gadgets)
            {
                PlayerData.AddGadget(new Gadget(gadget));
            }
        }
    }
}
