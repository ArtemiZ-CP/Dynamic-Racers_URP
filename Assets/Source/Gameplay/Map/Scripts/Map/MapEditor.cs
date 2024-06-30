using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
	private readonly List<Chunk> _currentChunks = new();

	[SerializeField] private List<Chunk> _chunks;
	[Header("Customize")]
	[SerializeField] private int _mapWidth;
	[Header("Debug")]
	[SerializeField] private List<ChunkSettings> _debugChunkSettings;

	private List<ChunkSettings> _chunkSettings;

	[ContextMenu("Respawn Chunks")]
	public void DebugRespawnChunks()
	{
		_chunkSettings = _debugChunkSettings;
		RespawnChunks();
	}

	private void Awake()
	{
		_chunkSettings = RunSettings.Map;

		if (_chunkSettings != null)
		{
			RespawnChunks();
		}
	}	

	private void RespawnChunks()
	{
		DestroyAll();
		SpawnChunks();
		SetupMap();
	}

	private void SetupMap()
	{
		for (int i = 0; i < _chunkSettings.Count; i++)
		{
			SetupChunk(i);
		}
	}

	private void SpawnChunks()
	{
		foreach (ChunkSettings chunkSettings in _chunkSettings)
		{
			Chunk chunk = _chunks.Find(chunk => chunk.Type == chunkSettings.Type);

			if (chunk == null)
			{
				Debug.LogError($"Chunk info for {chunkSettings.Type} is not found");
				continue;
			}

			Chunk previousChunk = _currentChunks.Count > 0 ? _currentChunks[_currentChunks.Count - 1] : null;

			Vector3 position = Vector3.zero;

			if (previousChunk != null)
			{
				position = previousChunk.EndPoint.position;
			}

			chunk = AddChunk(chunk, position);
			chunk.SetChunkSize(new Vector3Int(_mapWidth, chunkSettings.Size.y, chunkSettings.Size.x));
		}
	}

	private void SetupChunk(int chunkIndex)
	{
		Chunk chunk = _currentChunks[chunkIndex];
		ChunkSettings chunkSettings = _chunkSettings[chunkIndex];

		chunk.SetChunkSize(new Vector3Int(_mapWidth, chunkSettings.Size.y, chunkSettings.Size.x));

		Vector3 chunkPosition = Vector3.zero;

		if (chunkIndex > 0)
		{
			chunkPosition = _currentChunks[chunkIndex - 1].EndPoint.position;
		}

		chunk.transform.position = chunkPosition;
	}

	private void DestroyAll()
	{
		while (_currentChunks.Count > 0)
		{
			DestroyChunk(0);
		}

		while (transform.childCount > 0)
		{
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}

	private void DestroyChunk(int index)
	{
		Chunk chunk = _currentChunks[index];
		_currentChunks.RemoveAt(index);

		if (chunk != null)
		{
			DestroyImmediate(chunk.gameObject);
		}
	}

	private Chunk AddChunk(Chunk chunk, Vector3 position)
	{
		chunk = Instantiate(chunk, position, Quaternion.identity, transform);
		_currentChunks.Add(chunk);

		return chunk;
	}
}
