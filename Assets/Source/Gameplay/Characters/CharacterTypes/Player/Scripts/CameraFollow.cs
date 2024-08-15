using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollow : MonoBehaviour
{
	[SerializeField] private PlayerMovement _playerMovement;
	[SerializeField] private Transform _startPoint;
	[Header("Moves")]
	[SerializeField] private float _speedMultiplier;
	[SerializeField] private List<MoveData> _groundMoves;
	[SerializeField] private List<MoveData> _wallMoves;
	[SerializeField] private List<MoveData> _flyMoves;
	[SerializeField] private MoveData _finishMove;

	private CinemachineVirtualCamera _virtualCamera;
	private Chunk _currentChunk;
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
		if (_isFinishing == false
		&& _playerMovement.DistanceToFinish() / _playerMovement.Speed < DistanceToFinish() / (_finishMove.Speed * _speedMultiplier))
		{
			_isFinishing = true;
			StopAllCoroutines();
			StartCoroutine(Move(_finishMove));
		}
	}

	private void OnEnable()
	{
		_playerMovement.OnChangeChunk += OnChangeChunk;
	}

	private void OnDisable()
	{
		_playerMovement.OnChangeChunk -= OnChangeChunk;
	}
	private float DistanceToFinish()
	{
		return Vector3.Distance(transform.position, _finishMove.Transform.position);
	}

	private void OnChangeChunk(Chunk newChunk, CharacterMovement characterMovement)
	{
		if (_isFinishing)
		{
			return;
		}

		if (_currentChunk != null && newChunk != null)
		{
			if ((_currentChunk.Type == ChunkType.Ground || _currentChunk.Type == ChunkType.Water)
				&& (newChunk.Type == ChunkType.Ground || newChunk.Type == ChunkType.Water))
			{
				return;
			}
		}

		StopAllCoroutines();

		switch (newChunk.Type)
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

		_currentChunk = newChunk;
	}

	private IEnumerator Move(List<MoveData> moves)
	{
		foreach (var move in moves)
		{
			yield return Move(move);
		}
	}

	private IEnumerator Move(MoveData move)
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

	private IEnumerator MoveTo(Transform target, float speed)
	{
		while (Vector3.Distance(transform.position, target.position) > 0.1f)
		{
			transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
			yield return null;
		}
	}
}
