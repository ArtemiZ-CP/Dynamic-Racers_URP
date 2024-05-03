using System;
using System.Collections;
using UnityEngine;

public class SpeedGame : MonoBehaviour
{
	public static float RandomDeviation => UnityEngine.Random.Range(0, 1f);

	[SerializeField] private Transform _arrow;
	[SerializeField] private Transform _gradient;
	[SerializeField] private float _arrowSpeed;
	[SerializeField] private float _maxAngle;

	private float _currentAngle;
	private bool _isGameActive;

	public event Action<float> OnFinish;

	private void Start()
	{
		SetActiveGame(true);
		StartCoroutine(RotateArrow());
	}

	private void Update()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				_isGameActive = false;
				OnFinish?.Invoke(Mathf.Abs(_currentAngle / _maxAngle));
			}
		}
	}

	private void SetActiveGame(bool isActive)
	{
		_arrow.eulerAngles = new Vector3(0, 0, -_maxAngle);
		_gradient.gameObject.SetActive(isActive);
		_arrow.gameObject.SetActive(isActive);
	}

	private IEnumerator RotateArrow()
	{
		float t = 0;
		_isGameActive = true;

		while (_isGameActive)
		{
			t += Time.deltaTime * _arrowSpeed;
			_currentAngle = Mathf.PingPong(t, _maxAngle * 2) - _maxAngle;
			_arrow.eulerAngles = new Vector3(0, 0, _currentAngle);

			yield return null;
		}
	}
}
