using System.Collections;
using UnityEngine;

public class PlayerSkinRotation : MonoBehaviour
{
    [SerializeField] private Transform _playerSkin;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _touchRotationSpeed = 10f;

    public bool IsActive = true;

    private RectTransform _rectTransform;
    private float _startRotation;
    private float _targetRotation;
    private float _currentRotation;
    private bool _mooving;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        StartCoroutine(RotatePlayerSkin());
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (CursorInMoovingArea(touch.position))
                {
                    _mooving = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved && _mooving && IsActive)
            {
                _targetRotation -= touch.deltaPosition.x * _touchRotationSpeed * Time.deltaTime;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _mooving = false;
            }
        }
        else
        {
            while (_currentRotation < 0)
            {
                _currentRotation += 360;
            }

            while (_currentRotation > 360)
            {
                _currentRotation -= 360;
            }

            _targetRotation = _startRotation;
        }
    }

    private bool CursorInMoovingArea(Vector2 touchPosition)
    {
        touchPosition = new Vector2(touchPosition.x - Screen.width / 2, touchPosition.y - Screen.height / 2);
        return _rectTransform.rect.Contains(touchPosition);
    }

    private IEnumerator RotatePlayerSkin()
    {
        _startRotation = _playerSkin.localRotation.eulerAngles.y;
        _targetRotation = _startRotation;
        _currentRotation = _startRotation;

        while (true)
        {
            _currentRotation = Mathf.Lerp(_currentRotation, _targetRotation, Time.deltaTime * _rotationSpeed);
            _playerSkin.localRotation = Quaternion.Euler(0, _currentRotation, 0);
            yield return null;
        }
    }
}
