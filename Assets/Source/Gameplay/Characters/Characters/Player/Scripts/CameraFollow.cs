using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private PlayerMovement _playerMovement;
	[SerializeField] private Transform _startPoint;
	[Header("Moves")]
	[SerializeField] private float _speedMultiplier;
	[SerializeField] private List<MoveData> _groundMoves;
	[SerializeField] private List<MoveData> _wallMoves;
	[SerializeField] private List<MoveData> _flyMoves;
	[SerializeField] private float _distanceToFinish;
	[SerializeField] private List<MoveData> _finishMoves;

	private CinemachineVirtualCamera _virtualCamera;
	private ChunkType _lastChunkType;
	private bool _isFinishing = false;

	public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

	private void Awake()
	{
		transform.position = _startPoint.position;
		_virtualCamera = GetComponent<CinemachineVirtualCamera>();
		_virtualCamera.Priority = 0;
		enabled = false;
	}

	private void Update()
	{
		if (_isFinishing == false && _playerMovement.DistanceToFinish() < _distanceToFinish)
		{
			_isFinishing = true;
			StopAllCoroutines();
			StartCoroutine(Move(_finishMoves));
		}
	}

	private void OnEnable()
	{
		_playerMovement.OnChangeChunkType += OnChangeChunkType;
	}

	private void OnDisable()
	{
		_playerMovement.OnChangeChunkType -= OnChangeChunkType;
	}

	private void OnChangeChunkType(ChunkType chunkType)
	{
		if (_isFinishing)
		{
			return;
		}

		if ((_lastChunkType == ChunkType.Ground || _lastChunkType == ChunkType.Water)
			&& (chunkType == ChunkType.Ground || chunkType == ChunkType.Water))
		{
			return;
		}

		StopAllCoroutines();

		switch (chunkType)
		{
			case ChunkType.Start:
				break;
			case ChunkType.Ground:
				StartCoroutine(Move(_groundMoves));
				break;
			case ChunkType.Water:
				StartCoroutine(Move(_groundMoves));
				break;
			case ChunkType.Wall:
				StartCoroutine(Move(_wallMoves));
				break;
			case ChunkType.Fly:
				StartCoroutine(Move(_flyMoves));
				break;
			case ChunkType.Finish:
				break;
		}

		_lastChunkType = chunkType;
	}

	private IEnumerator Move(List<MoveData> moves)
	{
		foreach (var move in moves)
		{
			if (move.IsMoveImmediately)
			{
				transform.position = move.Transform.position;
			}
			else
			{
				yield return StartCoroutine(MoveTo(move.Transform, move.Speed * _speedMultiplier));
			}
		}
	}

	private IEnumerator MoveTo(Transform target, float speed)
	{
		while (Vector3.Distance(transform.position, target.position) > 0.1f)
		{
			transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
			yield return null;
		}
	}
}
