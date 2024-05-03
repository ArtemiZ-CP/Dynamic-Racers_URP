using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour
{
	public event Action OnFinish;

	private List<Chunk> _chunks;
	private GlobalSettings _globalSettings;

	private int _currentChunkIndex = 0;
	private int _currentMovePointIndex = 0;
	private ChunkType _currentChunkType;

	private float _speed;
	private float _speedMultiplier = 1;

	public event Action<ChunkType> OnChangeChunkType;
	
	public float CurrentDistance { get; private set; }
	public float Speed => _speed * _speedMultiplier;

	public void StartMove(List<Chunk> chunks, float multiplier)
	{
		_chunks = chunks;
		StartCoroutine(AddAcceleration(multiplier, _globalSettings.DistanceToStartPower));
		StartCoroutine(Move());
		CurrentDistance = 0;
	}

	public void AddAcceleration(float multiplier)
	{
		_speedMultiplier += multiplier;
	}

	public void SubtractAcceleration(float multiplier)
	{
		_speedMultiplier -= multiplier;
	}

	public IEnumerator AddAcceleration(float multiplier, float distance)
	{
		_speedMultiplier += multiplier;

		float startDistance = CurrentDistance;

		while (CurrentDistance - startDistance < distance)
		{
			yield return null;
		}

		_speedMultiplier -= multiplier;
	}

	protected abstract int GetUpgradeAmount(ChunkType chunkType);

	private void Awake()
	{
		_globalSettings = GlobalSettings.Instance;
		_speed = _globalSettings.BaseSpeed;
	}

	private void OnEnable()
	{
		OnChangeChunkType += OnChangeChunkHandler;
	}

	private void OnDisable()
	{
		OnChangeChunkType -= OnChangeChunkHandler;
	}

	private void Update()
	{
		CurrentDistance += _speed * _speedMultiplier * Time.deltaTime;
	}

	private void OnChangeChunkHandler(ChunkType chunkType)
	{
		float additionalSpeed = GetUpgradeAmount(chunkType) * _globalSettings.AdditionalSpeedByUpgrade;
		_speed = _globalSettings.BaseSpeed + additionalSpeed;
		_currentChunkType = chunkType;
	}

	private Transform MoveToNextPoint()
	{
		if (_currentMovePointIndex < _chunks[_currentChunkIndex].MovePoints.Count - 1)
		{
			_currentMovePointIndex++;
		}
		else
		{
			_currentMovePointIndex = 0;

			if (_currentChunkIndex < _chunks.Count - 1)
			{
				_currentChunkIndex++;
				OnChangeChunkType.Invoke(_chunks[_currentChunkIndex].Type);
			}
			else
			{
				return null;
			}
		}

		return GetTarget();
	}

	private Transform GetTarget()
	{
		if (_currentChunkIndex < _chunks.Count)
		{
			return _chunks[_currentChunkIndex].MovePoints[_currentMovePointIndex];
		}

		return null;
	}

	private IEnumerator Move()
	{
		Transform target = GetTarget();
		transform.position = GetPosition(target.position);
		OnChangeChunkType.Invoke(_chunks[_currentChunkIndex].Type);

		while (target != null)
		{
			Vector3 targetPosition = GetPosition(target.position);
			Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
			transform.position = GetPosition(newPosition);

			if (Vector3.Distance(newPosition, targetPosition) <= 0.1f)
			{
				target = MoveToNextPoint();
			}

			yield return null;
		}

		OnFinish?.Invoke();
	}

	private Vector3 GetPosition(Vector3 targetPosition)
	{
		return new Vector3(transform.position.x, targetPosition.y, targetPosition.z);
	}
}
