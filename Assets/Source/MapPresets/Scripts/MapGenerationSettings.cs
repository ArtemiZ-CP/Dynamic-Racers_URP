using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapGenerationSettings", menuName = "MapGenerationSettings", order = 0)]
public class MapGenerationSettings : ScriptableObject
{
    [SerializeField, Min(1)] private int _minChunks = 1;
    [SerializeField, Min(1)] private int _maxChunks = 1;
    [SerializeField, Min(1)] private int _minChunkSize = 1;
    [SerializeField, Min(1)] private int _maxChunkSize = 1;

    public ChunkSettings[] GenerateMap()
    {
        int chunksCount = Random.Range(_minChunks, _maxChunks + 1);
        ChunkSettings[] chunks = new ChunkSettings[chunksCount + 2];

        chunks[0] = new ChunkSettings(ChunkType.Start, 3);
        chunks[chunksCount + 1] = new ChunkSettings(ChunkType.Finish, 7);

        for (int i = 1; i < chunksCount + 1; i++)
        {
            ChunkType chunkType = GetRandomChunkType(chunks[i - 1].Type, i == 1, i == chunksCount);
            int chunkLength = Random.Range(_minChunkSize, _maxChunkSize + 1);
            chunks[i] = new ChunkSettings(chunkType, chunkLength);
        }

        return chunks;
    }

    private ChunkType GetRandomChunkType(ChunkType previousChunkType, bool isFirstChunk, bool isLastChunk) 
    {
        List<ChunkType> availableChunkTypes = new();

        for (int i = 0; i < System.Enum.GetValues(typeof(ChunkType)).Length; i++)
        {
            ChunkType chunkType = (ChunkType)i;

            if (chunkType == previousChunkType)
            {
                continue;
            }

            if (chunkType == ChunkType.Finish)
            {
                continue;
            }

            if (chunkType == ChunkType.Start)
            {
                continue;
            }

            if (chunkType == ChunkType.Water && (previousChunkType == ChunkType.Wall || isFirstChunk))
            {
                continue;
            }

            if (chunkType == ChunkType.Fly && (previousChunkType == ChunkType.Wall || isFirstChunk))
            {
                continue;
            }

            if (chunkType == ChunkType.Wall && (previousChunkType == ChunkType.Fly || isFirstChunk))
            {
                continue;
            }

            if (isLastChunk && chunkType == ChunkType.Wall)
            {
                continue;
            }

            if (isLastChunk && chunkType == ChunkType.Fly)
            {
                continue;
            }

            availableChunkTypes.Add((ChunkType)i);
        }

        return availableChunkTypes[Random.Range(0, availableChunkTypes.Count)];
    }
}