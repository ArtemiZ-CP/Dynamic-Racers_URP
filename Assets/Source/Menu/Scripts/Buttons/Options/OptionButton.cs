using UnityEngine;

public class OptionButton : MonoBehaviour
{
    [SerializeField] private GameObject _on;
    [SerializeField] private GameObject _off;

    private bool _isOn;

    public void Toggle()
    {
        _isOn = !_isOn;

        if (_isOn)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    protected virtual void TurnOn()
    {
        _on.SetActive(true);
        _off.SetActive(false);
    }

    protected virtual void TurnOff()
    {
        _on.SetActive(false);
        _off.SetActive(true);
    }
}
