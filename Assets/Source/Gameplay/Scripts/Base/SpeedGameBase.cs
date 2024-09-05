using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpeedGameBase : MonoBehaviour
{
    [SerializeField] private List<SpeedPower> _speedPowers = new();
    [SerializeField] private GameObject _visual;
    [SerializeField] private Transform _arrow;
    [SerializeField] private PlayerMovement _playerMovement;
    [Header("Hints")]
    [SerializeField] private GameObject _playerHint;
    [Header("Settings")]
    [SerializeField] private float _arrowSpeed;
    [SerializeField] private AnimationCurve _arrowMovementCurve;
    [SerializeField] private float _dragOffset;
    [SerializeField] private string _startTextOnNotTouch;
    [SerializeField] private Color _startTextColorOnNotTouch;

    public event Action<float> EndedSpeedGame;
    public event Action<string, Color> ShowStartText;

    public PlayerMovement PlayerMovement => _playerMovement;
    public Vector3 StartPlayerPosition => _startPlayerPosition;
    public float RandomSpeedMultiplier => _speedPowers[UnityEngine.Random.Range(0, _speedPowers.Count)].SpeedMultiplier;
    public float DragOffset => _dragOffset;
    public float MaxPlayerOffset => _maxPlayerOffset;
    public bool IsGameActive => _isGameActive;

    private float _maxPlayerOffset;
    private Vector3 _startPlayerPosition;
    private bool _isGameActive;
    private float _speedMultiplier;
    private string _startText;
    private Color _startTextColor;
    private float _startTouchPositionY;

    protected virtual void Awake()
    {
        _visual.SetActive(false);
        _maxPlayerOffset = GlobalSettings.Instance.CharacterStartOffset;
        _startText = _startTextOnNotTouch;
        _startTextColor = _startTextColorOnNotTouch;
    }

    protected virtual void Start()
    {
        _startPlayerPosition = _playerMovement.transform.position;
        SortSpeedPowers();
        ActiveGame();
    }

    private void Update()
    {
        ProcessTouch();
    }

    protected virtual void SetStartTouchPosition()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            _startTouchPositionY = touch.position.y;
        }
        else
        {
            _startTouchPositionY = Input.mousePosition.y;
        }
    }

    protected virtual void FinishSpeedGame()
    {
        _isGameActive = false;
    }

    protected void ProcessTouch()
    {
		if (IsTouchGown() && IsGameActive == false)
		{
			SetStartTouchPosition();
		}
		else if (IsTouch() && IsGameActive == false)
		{
			float t = Mathf.Clamp01(GetTouchOffset() / DragOffset);
			PlayerMovement.CurrentOffset = MaxPlayerOffset * t;

			if (GetTouchOffset() > DragOffset)
			{
				StartGame();
			}
		}
		else if (IsTouchUp() && IsGameActive)
		{
			FinishSpeedGame();
		}
    }

    protected bool IsTouchGown()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Began;
        }

        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    protected bool IsTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
        }

        return Input.GetKey(KeyCode.Mouse0);
    }

    protected bool IsTouchUp()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Ended;
        }

        return Input.GetKeyUp(KeyCode.Mouse0);
    }

    protected void StartGame()
    {
        _speedMultiplier = 1;
        _isGameActive = true;
        _visual.SetActive(true);
        StartCoroutine(MoveArrow());
        HideHint();
    }

    protected void StartRunning()
    {
        HideHint();
        EndedSpeedGame?.Invoke(_speedMultiplier);
        ShowStartText?.Invoke(_startText, _startTextColor);
    }

    protected float GetTouchOffset()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return -touch.position.y + _startTouchPositionY;
        }

        return -Input.mousePosition.y + _startTouchPositionY;
    }

    protected void ShowHint()
    {
        _playerHint.SetActive(true);
    }

    private void HideHint()
    {
        _playerHint.SetActive(false);
    }

    private void SortSpeedPowers()
    {
        _speedPowers = _speedPowers.OrderBy(point => point.LeftPosition.x).ToList();
    }

    private void ActiveGame()
    {
        _isGameActive = false;
        _arrow.localRotation = Quaternion.Euler(0, 0, SpeedPower.GetRotationZ(_speedPowers[0].LeftPosition, _arrow.position));
        gameObject.SetActive(true);
    }

    private float GetMultiplierRotation(float rotationZ, out string startText, out Color startTextColor)
    {
        foreach (SpeedPower speedPower in _speedPowers)
        {
            if (speedPower.IsBetween(rotationZ, _arrow.localPosition))
            {
                startText = speedPower.StartText;
                startTextColor = speedPower.StartTextColor;
                return speedPower.SpeedMultiplier;
            }
        }

        throw new Exception("SpeedPower not found");
    }

    private IEnumerator MoveArrow()
    {
        float t = 0;
        float leftPosition = SpeedPower.GetRotationZ(_speedPowers[0].LeftPosition, _arrow.localPosition);
        float rightPosition = SpeedPower.GetRotationZ(_speedPowers[^1].RightPosition, _arrow.localPosition);
        float currentPositionX = 0;

        while (_isGameActive)
        {
            t += _arrowSpeed * Time.deltaTime;
            float pingPongValue = Mathf.PingPong(t, 1);
            float curvedValue = _arrowMovementCurve.Evaluate(pingPongValue);
            currentPositionX = Mathf.Lerp(leftPosition, rightPosition, curvedValue);
            _arrow.localRotation = Quaternion.Euler(0, 0, currentPositionX);
            yield return null;
        }

        _speedMultiplier = GetMultiplierRotation(currentPositionX, out _startText, out _startTextColor);
    }
}
