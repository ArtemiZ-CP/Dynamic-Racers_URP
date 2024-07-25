using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InactiveButtonCondition : MonoBehaviour
{
    [SerializeField] private int _levelToActiveButton;
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
        if (PlayerProgress.Level >= _levelToActiveButton)
        {
            _activeButton.SetActive(true);
            _inactiveButton.SetActive(false);
            _button.interactable = true;
        }
        else
        {
            _activeButton.SetActive(false);
            _inactiveButton.SetActive(true);
            _button.interactable = false;
        }
    }
}
