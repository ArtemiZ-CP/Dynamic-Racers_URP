using System;
using UnityEngine;

[Serializable]
public class MapTile
{
    [SerializeField] private ChunkType _chunkType;
    [SerializeField] private RectTransform _tilePrefab;

    public ChunkType ChunkType => _chunkType;
    public RectTransform TilePrefab => _tilePrefab;    
}
