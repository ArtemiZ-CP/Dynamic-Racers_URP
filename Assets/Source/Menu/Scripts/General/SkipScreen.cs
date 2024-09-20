using UnityEngine;

public class SkipScreen : MonoBehaviour
{
    [SerializeField] private ActiveMenu _activeMenu;
    [SerializeField] private GameObject _menuToActive;

    private void OnEnable()
    {
        _activeMenu.SetActiveMenu(_menuToActive);
    }
}
