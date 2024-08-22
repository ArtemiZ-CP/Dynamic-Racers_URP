using System;
using UnityEngine;

public class ButtonObjectsSwap : MonoBehaviour
{
    [SerializeField] private GameObject _selectedObject;
    [SerializeField] private GameObject _deselectedObject;

    public event Action<bool> ObjectSwaped;

    public void Select()
    {
        _selectedObject.SetActive(true);
        _deselectedObject.SetActive(false);
        ObjectSwaped?.Invoke(true);
    }

    public void Deselect()
    {
        _selectedObject.SetActive(false);
        _deselectedObject.SetActive(true);
        ObjectSwaped?.Invoke(false);
    }
}
