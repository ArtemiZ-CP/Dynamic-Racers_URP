using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
	[SerializeField] private Gradient _gradient;
	[SerializeField] private TMP_Text _countdownText;
	[SerializeField] private float _timeToDisplayText;
	[SerializeField] private SpeedGameBase _speedGameBase;

	private void Awake()
	{
		_countdownText.text = string.Empty;
	}

	private void OnEnable()
	{
		_speedGameBase.ShowStartText += ShowEndText;
	}

	private void OnDisable()
	{
		_speedGameBase.ShowStartText -= ShowEndText;
	}

	public void Run(bool showCount = true)
	{
		_countdownText.gameObject.SetActive(true);
		StartCoroutine(StartCountdown(showCount));
	}

	private void ShowEndText(string text, Color color)
	{
		StopAllCoroutines();
		_countdownText.text = text;
		_countdownText.color = color;
		StartCoroutine(HideText(_timeToDisplayText));
	}

	private IEnumerator StartCountdown(bool showCount)
	{
		float startTime = GlobalSettings.Instance.TimeToStartRun;
		float time = startTime;

		while (time > 0)
		{
			time -= Time.deltaTime;
			string text = ((int)time + 1).ToString();

			if (text != _countdownText.text && showCount)
			{
				_countdownText.color = _gradient.Evaluate(time / startTime);
				_countdownText.text = text;
			}

			yield return null;
		}

		yield return new WaitForSeconds(_timeToDisplayText);

		_countdownText.gameObject.SetActive(false);
	}

	private IEnumerator HideText(float time)
	{
		yield return new WaitForSeconds(time);

		_countdownText.gameObject.SetActive(false);
	}
}
