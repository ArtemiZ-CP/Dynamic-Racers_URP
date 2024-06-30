using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunStages : MonoBehaviour
{
	[SerializeField] private Map _map;
	[SerializeField] private CameraSwitcher _cameraSwitcher;
	[Header("Start")]
	[SerializeField] private SpeedGame _speedGame;
	[SerializeField] private Countdown _countdown;
	[Header("End")]
	[SerializeField] private EndGame _endGame;
	[SerializeField] private string _menuSceneName;
	[Header("Characters")]
	[SerializeField] private PlayerMovement _playerMovement;
	[SerializeField] private List<CharacterMovement> _enemies;

	private void Start()
	{
		StartCoroutine(StartPrewiev());
	}

	private void OnEnable()
	{
		_speedGame.OnSpeedGameEnd += StartRunning;
		_playerMovement.OnChangeChunkType += CheckEndChunk;
	}

	private void OnDisable()
	{
		_speedGame.OnSpeedGameEnd -= StartRunning;
		_playerMovement.OnChangeChunkType -= CheckEndChunk;
	}

	private void StartCounter()
	{
		_speedGame.gameObject.SetActive(true);
		_countdown.Run();
	}

	private void CheckEndChunk(ChunkType chunkType)
	{
		if (chunkType == ChunkType.Finish)
		{
			_endGame.Run();
			GiveReward();
			StartCoroutine(LoadMenuOnClick());
		}
	}

	private void StartRunning(float multiplier)
	{
		_cameraSwitcher.SwitchToGameplayCamera();
		_speedGame.gameObject.SetActive(false);

		List<Chunk> chunks = _map.Chunks;

		_playerMovement.StartMove(chunks, multiplier);

		if (_enemies.Count > 0)
		{
			foreach (CharacterMovement enemy in _enemies)
			{
				enemy.StartMove(chunks, _speedGame.RandomSpeedMultiplier);
			}
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

	private void GiveReward()
	{
		PlayerProgress.Experience += RunSettings.ExperienceReward;
		RunSettings.Reset();
	}

	private IEnumerator LoadMenuOnClick()
	{
		while (true)
		{
			if (IsTouchGown())
			{
				SceneManager.LoadScene(_menuSceneName);
				break;
			}

			yield return null;
		}
	}

	private IEnumerator StartPrewiev()
	{
		yield return StartCoroutine(_cameraSwitcher.SwitchToPreviewCamera());

		StartCounter();
	}
}
