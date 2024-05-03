using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
	[SerializeField] private Gradient _gradient;
	[SerializeField] private TMP_Text _countdownText;
	[SerializeField] private float _timeToDisplayGO;

	private void Awake()
	{
		_countdownText.gameObject.SetActive(false);
	}

	private void Start()
	{
		StartCoroutine(StartCountdown());
	}

	private IEnumerator StartCountdown()
	{
		_countdownText.gameObject.SetActive(true);
		float startTime = GlobalSettings.Instance.TimeToStartRun;
		float time = startTime;

		while (time > 0)
		{
			time -= Time.deltaTime;
			string text = ((int)time + 1).ToString();

			if (text != _countdownText.text)
			{
				_countdownText.color = _gradient.Evaluate(time / startTime);
				_countdownText.text = text;
			}

			yield return null;
		}

		_countdownText.text = "GO!";

		yield return new WaitForSeconds(_timeToDisplayGO);

		_countdownText.gameObject.SetActive(false);
	}
}
