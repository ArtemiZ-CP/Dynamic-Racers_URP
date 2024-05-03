using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRun : MonoBehaviour
{
	[SerializeField] private Map _map;
	[SerializeField] private SpeedGame _speedGame;
	[Header("Characters")]
	[SerializeField] private PlayerMovement _playerMovement;
	[SerializeField] private List<CharacterMovement> _enemies;

	private float _speedMultiplier;

	private void Start()
	{
		SetMultiplier();
		Invoke(nameof(StartRunning), GlobalSettings.Instance.TimeToStartRun);
	}

	private void OnEnable()
	{
		_speedGame.OnFinish += OnFinishHandler;
	}

	private void OnDisable()
	{
		_speedGame.OnFinish -= OnFinishHandler;
	}

	private void OnFinishHandler(float deviation)
	{
		SetMultiplier(deviation);
	}

	private void SetMultiplier(float deviation = 1)
	{
		List<SpeedPower> speedPowers = GlobalSettings.Instance.SpeedPowers;
		speedPowers.Sort((a, b) => a.MinDeviation.CompareTo(b.MinDeviation));

		foreach (SpeedPower speedPower in speedPowers)
		{
			if (deviation < speedPower.MinDeviation)
			{
				_speedMultiplier = speedPower.SpeedMultiplier;
				return;
			}
		}

		_speedMultiplier = speedPowers[^1].SpeedMultiplier;
	}

	private void StartRunning()
	{
		_speedGame.gameObject.SetActive(false);

		_playerMovement.StartMove(_map.Chunks, _speedMultiplier);

		if (_enemies.Count > 0)
		{
			foreach (CharacterMovement enemy in _enemies)
			{
				enemy.StartMove(_map.Chunks, SpeedGame.RandomDeviation);
			}
		}
	}
}
