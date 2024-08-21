using UnityEngine;

public class AddRunGadget : MonoBehaviour
{
    [SerializeField] private GadgetScriptableObject _gadgetScriptableObject;

    private void Awake()
    {
#if UNITY_EDITOR
        if (_gadgetScriptableObject != null)
        {
            RunSettings.PlayerGadget = _gadgetScriptableObject;
        }
#endif
    }
}
