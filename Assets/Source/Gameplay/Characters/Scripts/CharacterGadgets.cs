using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterGadgets : MonoBehaviour
{
	private CharacterMovement _characterMovement;
	private GadgetScriptableObject _gadget;
	private GadgetScriptableObject _activeGadget;
	private ChunkType _currentChunkType;
	private float _lastAcceleration;
	private float _distanceToDisactiveGadget;
	private int[] _usageCounts;
	private bool _isUsageSplited;
	private bool _isSpeedIncresing;

	public event Action<GadgetChunkInfo, bool> OnActiveAnimation;
	public event Action<ChunkType> OnDisactiveAnimation;

	public GadgetScriptableObject Gadget => _gadget;
	public float RemainingDistance => _distanceToDisactiveGadget - _characterMovement.Distance;
	public int[] UsageCounts => _usageCounts;
	public bool IsUsageSplited => _isUsageSplited;

	protected virtual void Awake()
	{
		_characterMovement = GetComponent<CharacterMovement>();
		_usageCounts = new int[GlobalSettings.ChunksTypeCount];
	}

	private void OnEnable()
	{
		_characterMovement.OnChangeChunk += OnChangeChunkTypeHandler;
	}

	private void OnDisable()
	{
		_characterMovement.OnChangeChunk -= OnChangeChunkTypeHandler;
	}

	private void Update()
	{
		if (_distanceToDisactiveGadget < _characterMovement.Distance)
		{
			TryDisactiveGadget();
		}
	}

	public void Init(GadgetScriptableObject gadget)
	{
		if (gadget == null)
		{
			return;
		}

		_gadget = gadget;
		_isSpeedIncresing = gadget.IsSpeedIncreasing;
		_isUsageSplited = gadget.IsUsageSplited;
		Array.Fill(_usageCounts, gadget.UsageCount);
	}

	private void OnChangeChunkTypeHandler(Chunk chunk, CharacterMovement characterMovement)
	{
		_currentChunkType = chunk.Type;

		if (TryContinue()) return;
		TryDisactiveGadget();
		if (TryActiveGadget()) return;

		OnActiveAnimation.Invoke(new GadgetChunkInfo(_currentChunkType), false);
		_characterMovement.SetChunkSpeed(chunk.Type);
	}

	private bool TryContinue()
	{
		if (_activeGadget != null)
		{
			GadgetChunkInfo gadgetChunkInfo = _activeGadget.GetChunkInfo(_currentChunkType);

			if (gadgetChunkInfo != null)
			{
				if (_isSpeedIncresing == false)
				{
					SubtractAcceleration();
					AddAcceleration(gadgetChunkInfo.IsAccelerates);
				}

				OnActiveAnimation.Invoke(_activeGadget.GetChunkInfo(_currentChunkType), true);
				_characterMovement.SetChunkSpeed(gadgetChunkInfo.PowerUsing);

				return true;
			}
		}

		return false;
	}

	private bool TryActiveGadget()
	{
		if (_activeGadget != null)
		{
			throw new Exception("Active gadget is not null");
		}

		if (_gadget == null)
		{
			return false;
		}

		GadgetChunkInfo gadgetChunkInfo = _gadget.GetChunkInfo(_currentChunkType);

		if (gadgetChunkInfo != null)
		{
			if (_usageCounts[(int)_currentChunkType] <= 0)
			{
				return false;
			}

			ActiveGadget(_gadget, gadgetChunkInfo.IsAccelerates, gadgetChunkInfo.StartDelay, gadgetChunkInfo.SpeedMultiplierOnDelay);
			_characterMovement.SetChunkSpeed(gadgetChunkInfo.PowerUsing);

			return true;
		}

		return false;
	}

	private void ActiveGadget(GadgetScriptableObject gadget, bool isAccelerates, float delay, float speedMultiplierOnDelay)
	{
		_activeGadget = gadget;

		if (_usageCounts[(int)_currentChunkType] != int.MaxValue)
		{
			if (_isUsageSplited)
			{
				_usageCounts[(int)_currentChunkType]--;
			}
			else
			{
				for (int i = 0; i < _usageCounts.Length; i++)
				{
					_usageCounts[i]--;
				}
			}
		}

		if (_isSpeedIncresing)
		{
			StartCoroutine(AddAccelerationWithIncreasing(delay, speedMultiplierOnDelay));
		}
		else
		{
			StartCoroutine(AddAcceleration(isAccelerates, delay, speedMultiplierOnDelay));
		}

		OnActiveAnimation.Invoke(_activeGadget.GetChunkInfo(_currentChunkType), true);

		_distanceToDisactiveGadget = _characterMovement.Distance + _activeGadget.DistanceToDisactive;
	}

	private bool TryDisactiveGadget()
	{
		if (_activeGadget == null)
		{
			return false;
		}

		StopAllCoroutines();
		SubtractAcceleration();
		_distanceToDisactiveGadget = 0;
		_activeGadget = null;
		OnDisactiveAnimation.Invoke(_currentChunkType);

		return true;
	}

	private IEnumerator AddAccelerationWithIncreasing(float delay, float speedMultiplierOnDelay)
	{
		AddAcceleration(speedMultiplierOnDelay);

		yield return new WaitForSeconds(delay);

		SubtractAcceleration();
		AddAcceleration(_activeGadget.SpeedMultiplier / 4);
		print(1);

		while (IsDistancePassed(0.25f) == false)
		{
			yield return null;
		}

		SubtractAcceleration();
		AddAcceleration(_activeGadget.SpeedMultiplier / 2);
		print(2);

		while (IsDistancePassed(0.5f) == false)
		{
			yield return null;
		}

		SubtractAcceleration();
		AddAcceleration(_activeGadget.SpeedMultiplier);
		print(3);
	}

	private IEnumerator AddAcceleration(bool isAccelerates, float delay, float speedMultiplierOnDelay)
	{
		AddAcceleration(speedMultiplierOnDelay);

		yield return new WaitForSeconds(delay);

		SubtractAcceleration();
		AddAcceleration(isAccelerates);
	}

	private void AddAcceleration(bool isAccelerates)
	{
		if (isAccelerates)
		{
			AddAcceleration(_activeGadget.SpeedMultiplier);
		}
		else
		{
			AddAcceleration(-0.5f);
		}
	}

	private void AddAcceleration(float speedMultiplier)
	{
		_lastAcceleration += speedMultiplier;

		_characterMovement.AddAcceleration(speedMultiplier);
	}

	private void SubtractAcceleration()
	{
		_characterMovement.SubtractAcceleration(_lastAcceleration);
		_lastAcceleration = 0;
	}

	private bool IsDistancePassed(float percentage)
	{
		float endPoint = _distanceToDisactiveGadget;
		float startPoint = _distanceToDisactiveGadget - _activeGadget.DistanceToDisactive;
		float curentDistance = _characterMovement.Distance;

		return (curentDistance - startPoint) / (endPoint - startPoint) >= percentage;
	}
}
