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
	private int _usageCount;

	public event Action<GadgetChunkInfo, bool> OnActiveAnimation;
	public event Action OnDisactiveAnimation;

	public GadgetScriptableObject Gadget => _gadget;
	public float RemainingDistance => _distanceToDisactiveGadget - _characterMovement.Distance;
	public int UsageCount => _usageCount;

	public void Init(GadgetScriptableObject gadget)
	{
		if (gadget == null)
		{
			return;
		}

		_gadget = gadget;
		_usageCount = gadget.ActiveCount;
	}

	protected virtual void Awake()
	{
		_characterMovement = GetComponent<CharacterMovement>();
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

	private void OnChangeChunkTypeHandler(Chunk chunk, CharacterMovement characterMovement)
	{
		_currentChunkType = chunk.Type;

		if (TryContinue()) return;
		TryDisactiveGadget();
		if (TryActiveGadget()) return;

		OnActiveAnimation.Invoke(new GadgetChunkInfo(_currentChunkType), false);
	}

	private bool TryContinue()
	{
		if (_activeGadget != null)
		{
			GadgetChunkInfo gadgetChunkInfo = _activeGadget.GetChunkInfo(_currentChunkType);

			if (gadgetChunkInfo != null)
			{
				SubtractAcceleration();
				AddAcceleration(gadgetChunkInfo.IsAccelerates);
				OnActiveAnimation.Invoke(_activeGadget.GetChunkInfo(_currentChunkType), true);

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
			if (_usageCount <= 0)
			{
				return false;
			}

			ActiveGadget(_gadget, gadgetChunkInfo.IsAccelerates, gadgetChunkInfo.StartDelay, gadgetChunkInfo.SpeedMultiplierOnDelay);

			return true;
		}

		return false;
	}

	private void ActiveGadget(GadgetScriptableObject gadget, bool isAccelerates, float delay, float speedMultiplierOnDelay)
	{
		_activeGadget = gadget;

		if (_usageCount != int.MaxValue)
		{
			_usageCount--;
		}

		StartCoroutine(AddAcceleration(isAccelerates, delay, speedMultiplierOnDelay));
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
		OnDisactiveAnimation.Invoke();

		return true;
	}

	private IEnumerator AddAcceleration(bool isAccelerates, float delay, float speedMultiplierOnDelay)
	{
		AddAcceleration(speedMultiplierOnDelay);

		yield return new WaitForSeconds(delay);

		AddAcceleration(-speedMultiplierOnDelay);
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
}
