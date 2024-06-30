using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterGadgets : MonoBehaviour
{
	private List<GadgetScriptableObject> _gadgets = new();
	private GadgetScriptableObject _activeGadget;
	private CharacterMovement _characterMovement;
	private float _distanceToDisactiveGadget;
	private List<int> _usageCount = new();
	private ChunkType _currentChunkType;

	public event Action<GadgetAnimationInfo> OnActiveAnimation;

	public void Init(GadgetScriptableObject gadget)
	{
		if (gadget == null)
		{
			return;
		}

		_gadgets = new List<GadgetScriptableObject> {gadget};
		_usageCount = new List<int>(_gadgets.Count) {0};
	}

	protected virtual void Awake()
	{
		_characterMovement = GetComponent<CharacterMovement>();
	}

	private void OnEnable()
	{
		_characterMovement.OnChangeChunkType += OnChangeChunkTypeHandler;
	}

	private void OnDisable()
	{
		_characterMovement.OnChangeChunkType -= OnChangeChunkTypeHandler;
	}

	private void Update()
	{
		if (_activeGadget != null && _distanceToDisactiveGadget < _characterMovement.CurrentDistance)
		{
			DisactiveGadget();
			OnActiveAnimation.Invoke(new GadgetAnimationInfo(_currentChunkType));
		}
	}

	private void OnChangeChunkTypeHandler(ChunkType chunkType)
	{
		_currentChunkType = chunkType;

		if (_gadgets.Count != 0)
		{
			if (TryContinue(chunkType))
			{
				return;
			}

			if (_activeGadget != null)
			{
				DisactiveGadget();
			}

			if (TryActiveGadget(chunkType))
			{
				return;
			}
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

		foreach (GadgetScriptableObject gadget in _gadgets)
		{
			GadgetAnimationInfo gadgetAnimationInfo = gadget.ContainsChunkType(chunkType);

			if (gadgetAnimationInfo != null)
			{
				if (_usageCount[_gadgets.IndexOf(gadget)] >= gadget.ActiveCount)
				{
					continue;
				}

				ActiveGadget(gadget);
				OnActiveAnimation.Invoke(gadgetAnimationInfo);

				return true;
			}
		}

		return false;
	}

	private void ActiveGadget(GadgetScriptableObject gadget)
	{
		_activeGadget = gadget;
		_usageCount[_gadgets.IndexOf(_activeGadget)]++;
		_characterMovement.AddAcceleration(_activeGadget.SpeedMultiplier);
		_distanceToDisactiveGadget = _characterMovement.CurrentDistance + _activeGadget.DistanceToDisactive;
	}

	private void DisactiveGadget()
	{
		_characterMovement.SubtractAcceleration(_activeGadget.SpeedMultiplier);
		_distanceToDisactiveGadget = 0;
		_activeGadget = null;
	}
}
