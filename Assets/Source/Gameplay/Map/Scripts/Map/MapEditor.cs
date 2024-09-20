using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
	private readonly List<Chunk> _currentChunks = new();

	[SerializeField] private List<Chunk> _chunks;
	[Header("Debug")]
	[SerializeField] private MapPreset _debugChunkSettings;
	[SerializeField] private MapCellsContainer _mapCellsContainer;
	[SerializeField] private int _playersCount;

	[ContextMenu("Respawn Chunks")]
	public void DebugRespawnChunks()
	{
		RespawnChunks(_playersCount, _debugChunkSettings.Map, _mapCellsContainer);

#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(this);
#endif
	}

	private void Start()
	{
		int playersCount;
		List<ChunkSettings> map;
		MapCellsContainer mapCellsContainer;

		if (RunSettings.Map != null)
		{
			map = RunSettings.Map.ToList();
			playersCount = RunSettings.PlayersCount;
			mapCellsContainer = RunSettings.MapCellsContainer;
		}
		else
		{
			map = _debugChunkSettings.Map;
			playersCount = _playersCount;
			mapCellsContainer = _mapCellsContainer;
		}

		RespawnChunks(playersCount, map, mapCellsContainer);
	}

	private void RespawnChunks(int linesCount, List<ChunkSettings> map, MapCellsContainer mapCellsContainer)
	{
		linesCount += 2 * GlobalSettings.Instance.AdditionalRoadWidht;
		DestroyAll();
		SpawnChunks(linesCount, map, mapCellsContainer);
	}

	private void SpawnChunks(int linesCount, List<ChunkSettings> map, MapCellsContainer mapCellsContainer)
	{
		foreach (ChunkSettings chunkSettings in map)
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

			Vector3Int size = new(linesCount, chunkSettings.Size.y, chunkSettings.Size.x);

			chunk = AddChunk(chunk, position);

			bool emptyBefore;
			bool emptyAfter;
			int index = map.IndexOf(chunkSettings);

			if (index == 0 || map[index - 1].Type == ChunkType.Fly)
			{
				emptyBefore = true;
			}
			else
			{
				emptyBefore = false;
			}
			
			if (index == map.Count - 1 || map[index + 1].Type == ChunkType.Fly || map[index].Type == ChunkType.Wall)
			{
				emptyAfter = true;
			}
			else
			{
				emptyAfter = false;
			}

			chunk.SetupChunk(size, mapCellsContainer, emptyBefore, emptyAfter);
		}
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
