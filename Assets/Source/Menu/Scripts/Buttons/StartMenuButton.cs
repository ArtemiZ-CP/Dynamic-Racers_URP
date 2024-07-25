using UnityEngine;

public class StartMenuButton : MonoBehaviour
{
    [SerializeField] private ActiveMenu _activeMenu;
    [SerializeField] private GameObject _nextMenu;
    [SerializeField] private GameObject _loadingMenu;
    [SerializeField] private SelectionGadgetScreen _selectionGadgetScreen;

    public void ClickStart()
    {
        if (PlayerProgress.PassedTrainings < GlobalSettings.Instance.TrainingLevelsCount)
        {
            _activeMenu.SetActiveMenu(_loadingMenu);
        }
        else
        {
            _activeMenu.SetActiveMenu(_nextMenu);
        }

        _selectionGadgetScreen.SetMapPreset();
    }
}
