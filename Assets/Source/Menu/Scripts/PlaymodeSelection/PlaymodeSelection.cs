using UnityEngine;

public class PlaymodeSelection : MonoBehaviour
{
    [SerializeField] private bool _skipScreen;
    [SerializeField] private ActiveMenu _activeMenu;
    [SerializeField] private GameObject _menuToActive;

    private void OnEnable()
    {
        if (_skipScreen)
        {
            _activeMenu.SetActiveMenu(_menuToActive);
        }
    }
}
