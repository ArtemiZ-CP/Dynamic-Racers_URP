using UnityEngine;

[CreateAssetMenu(fileName = "MapPreset", menuName = "MapPreset", order = 0)]
public class MapPreset : ScriptableObject
{
    [SerializeField] private ChunkSettings[] _map;

    public ChunkSettings[] Map => _map;

    private void OnValidate()
    {
        System.Array.ForEach(_map, chunkSettings =>
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
