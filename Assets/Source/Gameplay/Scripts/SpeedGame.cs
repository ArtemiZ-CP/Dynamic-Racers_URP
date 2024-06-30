using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpeedGame : MonoBehaviour
{
	[SerializeField] private List<SpeedPower> _speedPowers = new();
	[SerializeField] private GameObject _visual;
	[SerializeField] private Transform _arrow;
	[SerializeField] private GameObject _player;
	[SerializeField] private GameObject _playerHint;
	[Header("Settings")]
	[SerializeField] private float _arrowSpeed;
	[SerializeField] private AnimationCurve _arrowMovementCurve;
	[SerializeField] private float _dragOffset;
	[SerializeField] private float _hintMoveDuration;
	[SerializeField] private float _maxPlayerOffset;

	private Vector3 _startPlayerPosition;
	private bool _isGameActive;
	private float _speedMultiplier;
	private float _startTouchPositionY;

	public float RandomSpeedMultiplier => _speedPowers[UnityEngine.Random.Range(0, _speedPowers.Count)].SpeedMultiplier;
	public float MinMultiplier => _speedPowers.Min(x => x.SpeedMultiplier);
	public event Action<float> OnSpeedGameEnd;

	private void Awake()
	{
		_visual.SetActive(false);
		gameObject.SetActive(false);
	}

	private void Start()
	{
		_startPlayerPosition = _player.transform.position;
		SortSpeedPowers();
		ActiveGame();
		Invoke(nameof(StartRunning), GlobalSettings.Instance.TimeToStartRun);
		StartCoroutine(ShowHint());
	}

	private void Update()
	{
		if (IsTouchGown() && _isGameActive == false)
		{
			SetStartTouchPosition();
		}
		else if (IsTouch() && _isGameActive == false)
		{
			float t = Mathf.Clamp01(GetTouchOffset() / _dragOffset);
			_player.transform.position = _startPlayerPosition + _maxPlayerOffset * t * Vector3.back;

			if (GetTouchOffset() > _dragOffset)
			{
				StartGame();
			}
		}
		else if (IsTouchUp() && _isGameActive)
		{
			_isGameActive = false;
		}
	}

	private bool IsTouchGown()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			return touch.phase == TouchPhase.Began;
		}

		return Input.GetKeyDown(KeyCode.Mouse0);
	}

	private bool IsTouch()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			return touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
		}

		return Input.GetKey(KeyCode.Mouse0);

	}

	private bool IsTouchUp()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			return touch.phase == TouchPhase.Ended;
		}

		return Input.GetKeyUp(KeyCode.Mouse0);
	}

	private void SetStartTouchPosition()
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

	private void StartGame()
	{
		_speedMultiplier = MinMultiplier;
		_isGameActive = true;
		_visual.SetActive(true);
		StartCoroutine(MoveArrow());
		HideHint();
	}

	private void StartRunning()
	{
		OnSpeedGameEnd?.Invoke(_speedMultiplier);
	}

	private IEnumerator ShowHint()
	{
		_playerHint.transform.position = _startPlayerPosition;

		Vector3 playerHintPosition = _player.transform.position + Vector3.back * _maxPlayerOffset;
		float speed = Vector3.Distance(_playerHint.transform.position, playerHintPosition) / _hintMoveDuration;

		while (_playerHint.transform.position.z > playerHintPosition.z)
		{
			_playerHint.transform.position += speed * Time.deltaTime * Vector3.back;
			yield return null;
		}

		_playerHint.transform.position = playerHintPosition;
	}

	private void HideHint()
	{
		_playerHint.SetActive(false);
	}

	private float GetTouchOffset()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			return -touch.position.y + _startTouchPositionY;
		}

		return -Input.mousePosition.y + _startTouchPositionY;
	}

	private void SortSpeedPowers()
	{
		_speedPowers = _speedPowers.OrderBy(point => point.LeftPosition.x).ToList();
	}

	private void ActiveGame()
	{
		_isGameActive = false;
		_arrow.rotation = Quaternion.Euler(0, 0, SpeedPower.GetRotationZ(_speedPowers[0].LeftPosition, _arrow.position));
		gameObject.SetActive(true);
	}

	private float GetMultiplierRotation(float rotationZ)
	{
		foreach (SpeedPower speedPower in _speedPowers)
		{
			if (speedPower.IsBetween(rotationZ, _arrow.position))
			{
				return speedPower.SpeedMultiplier;
			}
		}

		throw new Exception("SpeedPower not found");
	}

	private IEnumerator MoveArrow()
	{
		float t = 0;
		float leftPosition = SpeedPower.GetRotationZ(_speedPowers[0].LeftPosition, _arrow.position);
		float rightPosition = SpeedPower.GetRotationZ(_speedPowers[^1].RightPosition, _arrow.position);
		float currentPositionX = 0;

		while (_isGameActive)
		{
			t += _arrowSpeed * Time.deltaTime;
			float pingPongValue = Mathf.PingPong(t, 1);
			float curvedValue = _arrowMovementCurve.Evaluate(pingPongValue);
			currentPositionX = Mathf.Lerp(leftPosition, rightPosition, curvedValue);
			_arrow.rotation = Quaternion.Euler(0, 0, currentPositionX);
			yield return null;
		}

		_speedMultiplier = GetMultiplierRotation(currentPositionX);
	}
}
