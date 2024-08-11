using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour
{
	private const float _minSpeedMultiplier = 0;

	private List<Chunk> _chunks;
	private GlobalSettings _globalSettings;
	private int _currentChunkIndex = 0;
	private int _currentMovePointIndex = 0;
	private float _speed;
	private float _speedMultiplier = 1;
	private float _distance;

	public event Action<Chunk, CharacterMovement> OnChangeChunk;

	public float Distance => _distance;
	public bool IsFinished => _currentChunkIndex == _chunks.Count - 1;
	public float Speed
	{
		get
		{
			if (_speedMultiplier < _minSpeedMultiplier)
			{
				return _speed * _minSpeedMultiplier;
			}

			return _speed * _speedMultiplier;
		}
	}

	private void Awake()
	{
		_globalSettings = GlobalSettings.Instance;
		_speed = _globalSettings.BaseSpeed;
	}

	private void Update()
	{
		_distance += Speed * Time.deltaTime;
	}

	public void SetChunkSpeed(ChunkType chunkType)
	{
		float additionalSpeed = GetUpgradeAmount(chunkType) * _globalSettings.AdditionalSpeedByUpgrade;
		_speed = _globalSettings.BaseSpeed + additionalSpeed;
	}

	public float DistanceToFinish()
	{
		if (_chunks == null || _chunks.Count == 0)
		{
			return float.PositiveInfinity;
		}

		float distance = Vector3.Distance(transform.position, GetTarget().position);

		distance += DistanceToChunkEnd(_currentChunkIndex, _currentMovePointIndex);

		for (int chunkIndex = _currentChunkIndex + 1; chunkIndex < _chunks.Count; chunkIndex++)
		{
			distance += DistanceToChunkEnd(chunkIndex, 0);
		}

		return distance;
	}

	public void StartMove(List<Chunk> chunks, float multiplier)
	{
		_chunks = chunks;
		_distance = 0;
		StartCoroutine(AddAcceleration(multiplier, _globalSettings.DistanceToStartPower, _globalSettings.StartPowerCurve));
		StartCoroutine(Move());
	}

	public void AddAcceleration(float multiplier)
	{
		_speedMultiplier += multiplier;
	}

	public void SubtractAcceleration(float multiplier)
	{
		_speedMultiplier -= multiplier;
	}

	public IEnumerator AddAcceleration(float multiplier, float distance, AnimationCurve moveCurve)
	{
		float startDistance = Distance;

		while (Distance - startDistance < distance)
		{
			float t = moveCurve.Evaluate((Distance - startDistance) / distance);
			_speedMultiplier += multiplier * t;

			yield return null;

			_speedMultiplier -= multiplier * t;
		}
	}

	protected abstract int GetUpgradeAmount(ChunkType chunkType);

	private float DistanceToChunkEnd(int chunkIndex, int startPointIndex)
	{
		Transform from;
		Transform to;
		float distance = 0;

		for (int j = startPointIndex; j < _chunks[chunkIndex].MovePoints.Count - 1; j++)
		{
			from = _chunks[chunkIndex].MovePoints[j];
			to = _chunks[chunkIndex].MovePoints[j + 1];

			distance += Vector3.Distance(GetPosition(from.position), GetPosition(to.position));
		}

		if (chunkIndex != _chunks.Count - 1)
		{
			from = _chunks[chunkIndex].MovePoints[^1];
			to = _chunks[chunkIndex + 1].MovePoints[0];

			distance += Vector3.Distance(GetPosition(from.position), GetPosition(to.position));
		}

		return distance;
	}

	private Transform MoveToNextPoint()
	{
		if (GetTarget() == _chunks[_currentChunkIndex].ChangeAnimationPoint)
		{
			if (_chunks[_currentChunkIndex].Type != ChunkType.Finish)
			{
				OnChangeChunk?.Invoke(_chunks[_currentChunkIndex + 1], this);
			}
		}

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
			}
			else
			{
				_currentChunkIndex = _chunks.Count - 1;
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
		OnChangeChunk.Invoke(_chunks[_currentChunkIndex], this);

		while (target != null)
		{
			Vector3 targetPosition = GetPosition(target.position);
			Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
			transform.position = newPosition;

			if (Vector3.Distance(newPosition, targetPosition) <= 0.1f)
			{
				transform.position = GetPosition(targetPosition);
				target = MoveToNextPoint();
			}

			yield return null;
		}
	}

	private Vector3 GetPosition(Vector3 targetPosition)
	{
		return new Vector3(transform.position.x, targetPosition.y, targetPosition.z);
	}
}
