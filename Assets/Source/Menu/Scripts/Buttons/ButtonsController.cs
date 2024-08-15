using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] private ButtonObjectsSwap _startActiveButton;
    [SerializeField] private ButtonObjectsSwap[] _buttons;

    private ButtonObjectsSwap _currentButton;

    private void Start()
    {
        SetButtonActive(_startActiveButton);
    }

    public void SetButtonActive(ButtonObjectsSwap buttonToActivate)
    {
        if (_currentButton == buttonToActivate)
        {
            return;
        }

        foreach (ButtonObjectsSwap button in _buttons)
        {
            if (button == buttonToActivate)
            {
                button.Select();
            }
            else
            {
                button.Deselect();
            }
        }

        _currentButton = buttonToActivate;
    }
}
