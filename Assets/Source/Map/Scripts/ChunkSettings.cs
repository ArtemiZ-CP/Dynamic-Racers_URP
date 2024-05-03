using System;
using UnityEngine;

[Serializable]
public class ChunkSettings
{
	[SerializeField] private ChunkType _chunkType;
	[SerializeField, Min(3)] private int _chunkLength;

	public ChunkType Type => _chunkType;
	public Vector2Int Size => GetChunkSize();
	public int Length => _chunkLength;

	public ChunkSettings(ChunkType chunkType)
	{
		_chunkType = chunkType;
		_chunkLength = GlobalSettings.Instance.MinRoadLength;
	}

	public ChunkSettings(ChunkType chunkType, int chunkLength)
	{
		_chunkType = chunkType;

		if (chunkLength < GlobalSettings.Instance.MinRoadLength)
		{
			_chunkLength = GlobalSettings.Instance.MinRoadLength;
		}
		else
		{
			_chunkLength = chunkLength;
		}
	}

	private Vector2Int GetChunkSize()
	{
		Vector2Int size = new();

		if (_chunkType == ChunkType.Wall)
		{
			size.y = _chunkLength;
		}
		else
		{
			size.x = _chunkLength;
		}

		return size;
	}
}
