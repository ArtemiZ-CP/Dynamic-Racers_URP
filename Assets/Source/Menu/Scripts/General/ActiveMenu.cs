using System.Collections.Generic;
using UnityEngine;

public class ActiveMenu : MonoBehaviour
{
    [SerializeField] private GameObject _startActiveMenu;
    [SerializeField] private List<GameObject> _menuObjects;

    private GameObject _currentMenu;

    private void OnEnable()
    {
        SetActiveMenu(_startActiveMenu);
    }

    public void SetActiveMenu(GameObject objectToActive)
    {
        if (_currentMenu == objectToActive)
        {
            return;
        }

        if (_menuObjects == null || objectToActive == null || _menuObjects.Count == 0 || _menuObjects.Contains(objectToActive) == false)
        {
            return;
        }

        foreach (var menuObject in _menuObjects)
        {
            menuObject.SetActive(false);
        }

        objectToActive.SetActive(true);

        _currentMenu = objectToActive;
    }
}
