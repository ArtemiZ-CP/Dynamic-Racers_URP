using System;
using UnityEngine;

[Serializable]
public class ChunkSettings
{
	[SerializeField] private ChunkType _chunkType;
	[Min(3)] public int Length;

	public ChunkType Type => _chunkType;
	public Vector2Int Size => GetChunkSize();

	public ChunkSettings(ChunkType chunkType)
	{
		_chunkType = chunkType;
		Length = GlobalSettings.Instance.MinRoadLength;
	}

	public ChunkSettings(ChunkType chunkType, int chunkLength)
	{
		_chunkType = chunkType;

		if (chunkLength < GlobalSettings.Instance.MinRoadLength)
		{
			Length = GlobalSettings.Instance.MinRoadLength;
		}
		else
		{
			Length = chunkLength;
		}
	}

	private Vector2Int GetChunkSize()
	{
		Vector2Int size = new();

		if (_chunkType == ChunkType.Wall)
		{
			size.y = Length;
		}
		else
		{
			size.x = Length;
		}

		return size;
	}
}
