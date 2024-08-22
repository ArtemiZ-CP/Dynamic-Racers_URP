using UnityEngine;

public class ButtonNotification : MonoBehaviour
{
    [SerializeField] private ButtonObjectsSwap _buttonObjectsSwap;
    [SerializeField] private Vector3 _defaultButtonPosition;
    [SerializeField] private Vector3 _selectedButtonPosition;

    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _buttonObjectsSwap.ObjectSwaped += MoveButton;
    }

    private void OnDisable()
    {
        _buttonObjectsSwap.ObjectSwaped -= MoveButton;
    }

    private void MoveButton(bool isSelected)
    {
        if (isSelected)
        {
            _rect.anchoredPosition = _selectedButtonPosition;
        }
        else
        {
            _rect.anchoredPosition = _defaultButtonPosition;
        }
    }
}
