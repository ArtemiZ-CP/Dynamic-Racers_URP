using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterGadgets : MonoBehaviour
{
	private List<Gadget> _gadgets = new();
	private Gadget _activeGadget;
	private CharacterMovement _characterMovement;
	private float _distanceToDisactiveGadget;

	private void Awake()
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
		}
	}

	private void OnChangeChunkTypeHandler(ChunkType chunkType)
	{
		if (_gadgets.Count == 0)
		{
			return;
		}

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

	private bool TryContinue(ChunkType chunkType)
	{
		if (_activeGadget != null && _activeGadget.ContainsChunkType(chunkType))
		{
			_activeGadget.ChangeAnimation(chunkType);
			return true;
		}

		return false;
	}

	private bool TryActiveGadget(ChunkType chunkType)
	{
		if (_activeGadget != null)
		{
			throw new System.Exception("Active gadget is not null");
		}

		foreach (Gadget gadget in _gadgets)
		{
			if (gadget.ContainsChunkType(chunkType))
			{
				_activeGadget = gadget;
				_activeGadget.ChangeAnimation(chunkType);
				_characterMovement.AddAcceleration(_activeGadget.SpeedMultiplier);
				_distanceToDisactiveGadget = _characterMovement.CurrentDistance + _activeGadget.DistanceToDisactive;

				return true;
			}
		}

		return false;
	}

	private void DisactiveGadget()
	{
		_activeGadget.Disactive();
		_characterMovement.SubtractAcceleration(_activeGadget.SpeedMultiplier);
		_distanceToDisactiveGadget = 0;
		_activeGadget = null;
	}
}
