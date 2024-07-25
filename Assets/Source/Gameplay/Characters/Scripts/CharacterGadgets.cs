using System;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterGadgets : MonoBehaviour
{
	private GadgetScriptableObject _gadget;
	private GadgetScriptableObject _activeGadget;
	private CharacterMovement _characterMovement;
	private float _distanceToDisactiveGadget;
	private int _usageCount;
	private ChunkType _currentChunkType;

	public event Action<GadgetAnimationInfo> OnActiveAnimation;

	public GadgetScriptableObject Gadget => _gadget;
	public bool IsGadgetActive => _activeGadget != null;
	public int UsageCount => _usageCount;
	public float RemainingDistance => _distanceToDisactiveGadget - _characterMovement.Distance;

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
		if (_activeGadget != null && _distanceToDisactiveGadget < _characterMovement.Distance)
		{
			DisactiveGadget();
			OnActiveAnimation.Invoke(new GadgetAnimationInfo(_currentChunkType));
		}
	}

	private void OnChangeChunkTypeHandler(Chunk chunk, CharacterMovement characterMovement)
	{
		_currentChunkType = chunk.Type;

		if (_gadget != null)
		{
			if (TryContinue(chunk.Type)) return;

			if (_activeGadget != null)
			{
				DisactiveGadget();
			}

			if (TryActiveGadget(chunk.Type)) return;
		}

		OnActiveAnimation.Invoke(new GadgetAnimationInfo(_currentChunkType));
	}

	private bool TryContinue(ChunkType chunkType)
	{
		if (_activeGadget != null)
		{
			GadgetAnimationInfo gadgetAnimationInfo = _activeGadget.ContainsChunkType(chunkType);

			if (gadgetAnimationInfo != null)
			{
				OnActiveAnimation.Invoke(gadgetAnimationInfo);
				return true;
			}
		}

		return false;
	}

	private bool TryActiveGadget(ChunkType chunkType)
	{
		if (_activeGadget != null)
		{
			throw new Exception("Active gadget is not null");
		}

		GadgetAnimationInfo gadgetAnimationInfo = _gadget.ContainsChunkType(chunkType);

		if (gadgetAnimationInfo != null)
		{
			if (_usageCount <= 0)
			{
				return false;
			}

			ActiveGadget(_gadget);
			OnActiveAnimation.Invoke(gadgetAnimationInfo);

			return true;
		}

		return false;
	}

	private void ActiveGadget(GadgetScriptableObject gadget)
	{
		_activeGadget = gadget;

		if (_usageCount != int.MaxValue)
		{
			_usageCount--;
		}
		
		_characterMovement.AddAcceleration(_activeGadget.SpeedMultiplier);
		_distanceToDisactiveGadget = _characterMovement.Distance + _activeGadget.DistanceToDisactive;
	}

	private void DisactiveGadget()
	{
		_characterMovement.SubtractAcceleration(_activeGadget.SpeedMultiplier);
		_distanceToDisactiveGadget = 0;
		_activeGadget = null;
	}
}
