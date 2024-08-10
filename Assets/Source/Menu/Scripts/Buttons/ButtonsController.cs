using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] private ButtonObjectsSwap[] _buttons;
    
    public void SelectButton(ButtonObjectsSwap button)
    {
        foreach (ButtonObjectsSwap b in _buttons)
        {
            if (b == button)
            {
                b.Select();
            }
            else
            {
                b.Deselect();
            }
        }
    }
}
