using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapPreset", menuName = "MapPreset", order = 0)]
public class MapPreset : ScriptableObject
{
    [SerializeField] private List<ChunkSettings> _map = new();

    public List<ChunkSettings> Map => _map;

    private void OnValidate()
    {
        _map.ForEach(chunkSettings =>
        {
            if (chunkSettings.Type == ChunkType.Start)
            {
                chunkSettings.Length = 3;
            }

            if (chunkSettings.Type == ChunkType.Finish)
            {
                chunkSettings.Length = 7;
            }

            if (chunkSettings.Length < 3)
            {
                chunkSettings.Length = 3;
            }
        });
    }
}
