using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	private readonly List<Chunk> _currentChunks = new();

	public List<Chunk> Chunks => _currentChunks;

	private void Awake()
	{
		foreach (var chunk in GetComponentsInChildren<Chunk>())
		{
			_currentChunks.Add(chunk);
		}
	}
}
