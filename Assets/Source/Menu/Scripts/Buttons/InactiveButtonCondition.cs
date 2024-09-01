using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InactiveButtonCondition : MonoBehaviour
{
    [SerializeField] private int _levelToActiveButton;
    [SerializeField] private bool _setInactive;
    [SerializeField] private GameObject _activeButton;
    [SerializeField] private GameObject _inactiveButton;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void FixedUpdate()
    {
        Active();
    }

    private void Active()
    {
        if (_setInactive == false && PlayerData.Level >= _levelToActiveButton)
        {
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
    }

    private void SetActive(bool isActive)
    {
        _activeButton.SetActive(isActive);
        _inactiveButton.SetActive(isActive == false);
        _button.interactable = isActive;
    }
}
