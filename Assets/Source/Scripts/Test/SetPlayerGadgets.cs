using System.Collections.Generic;
using UnityEngine;

public class SetPlayerGadgets : MonoBehaviour
{
    [SerializeField] private List<GadgetScriptableObject> _gadgets;

    private void Awake()
    {
        if (_gadgets == null)
        {
            return;
        }

        foreach (GadgetScriptableObject gadget in _gadgets)
        {
            PlayerData.AddGadget(new Gadget(gadget));
        }
    }
}
