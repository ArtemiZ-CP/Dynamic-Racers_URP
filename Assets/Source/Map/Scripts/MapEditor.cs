using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapEditor : MonoBehaviour
{
	private readonly List<Chunk> _currentChunks = new();

	[SerializeField] private bool _dynamicMap;
	[SerializeField] private List<Chunk> _chunks;
	[Header("Customize")]
	[SerializeField] private List<ChunkSettings> _chunkSettings;
	[SerializeField] private int _mapWidth;

	[ContextMenu("Respawn Chunks")]
	public void RespawnChunks()
	{
		DestroyAll();
		SpawnChunks();
		SetupMap();
	}

	[ContextMenu("Clear Chunks")]
	public void ClearChunks()
	{
		_chunkSettings.Clear();
		DestroyAll();
	}

	private void Update()
	{
		if (Application.isPlaying == false && _dynamicMap)
		{
			if (IsMapChanged())
			{
				RespawnChunks();
			}
			else
			{
				SetupMap();
			}
		}
	}

	private bool IsMapChanged()
	{
		if (_chunkSettings.Count != _currentChunks.Count)
		{
			return true;
		}

		for (int i = 0; i < _chunkSettings.Count; i++)
		{
			if (_chunkSettings[i].Type != _currentChunks[i].Type)
			{
				return true;
			}
		}

		return false;
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
