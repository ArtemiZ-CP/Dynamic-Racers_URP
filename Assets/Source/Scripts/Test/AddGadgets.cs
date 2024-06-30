using UnityEngine;

public class AddGadgets : MonoBehaviour
{
    [SerializeField] private GadgetScriptableObject _gadget;
    [SerializeField] private CharacterGadgets _characterGadgets;

    private void Start()
    {
        _characterGadgets.Init(_gadget);
    }
}
