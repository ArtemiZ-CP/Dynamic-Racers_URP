using System.Collections.Generic;
using UnityEngine;

public class ActiveMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> _menuObjects;

    public void SetActiveMenu(GameObject objectToActive)
    {
        if (_menuObjects == null || objectToActive == null || _menuObjects.Count == 0 || _menuObjects.Contains(objectToActive) == false)
        {
            return;
        }

        foreach (var menuObject in _menuObjects)
        {
            menuObject.SetActive(false);
        }

        objectToActive.SetActive(true);
    }

    private void Awake()
    {
        SetActiveMenu(_menuObjects[0]);
    }
}
