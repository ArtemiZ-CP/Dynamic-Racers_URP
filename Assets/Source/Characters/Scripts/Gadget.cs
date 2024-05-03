using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gadget", menuName = "Gadget")]
public class Gadget : ScriptableObject
{
	[SerializeField] private List<ChunkType> _chunkTypes;
	[SerializeField] private float _speedMultiplier;
	[SerializeField] private float _distanceToDisactive;

	public float SpeedMultiplier => _speedMultiplier;
	public float DistanceToDisactive => _distanceToDisactive;

	public bool ContainsChunkType(ChunkType chunkType)
	{
		return _chunkTypes.Contains(chunkType);
	}

	public void ChangeAnimation(ChunkType chunkType)
	{
		// Change animation
	}

	public void Disactive()
	{
		// Disactive gadget
	}
}
