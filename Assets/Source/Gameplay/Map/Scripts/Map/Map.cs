using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	public List<Chunk> Chunks => GetChunks();

	private List<Chunk> GetChunks()
	{
		List<Chunk> _currentChunks = new();

		foreach (var chunk in GetComponentsInChildren<Chunk>())
		{
			_currentChunks.Add(chunk);
		}

		return _currentChunks;
	}
}
