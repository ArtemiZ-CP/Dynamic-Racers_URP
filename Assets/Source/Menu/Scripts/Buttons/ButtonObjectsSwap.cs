using UnityEngine;

public class ButtonObjectsSwap : MonoBehaviour
{
    [SerializeField] private GameObject _selectedObject;
    [SerializeField] private GameObject _deselectedObject;

    public void Select()
    {
        _selectedObject.SetActive(true);
        _deselectedObject.SetActive(false);
    }

    public void Deselect()
    {
        _selectedObject.SetActive(false);
        _deselectedObject.SetActive(true);
    }
}
