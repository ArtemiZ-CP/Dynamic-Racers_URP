using UnityEngine;

public class AddRunGadget : MonoBehaviour
{
    [SerializeField] private GadgetScriptableObject _gadgetScriptableObject;

    private void Awake()
    {
        if (_gadgetScriptableObject != null && RunSettings.PlayerGadget == null)
        {
            RunSettings.PlayerGadget = new Gadget(_gadgetScriptableObject);
        }
    }
}
