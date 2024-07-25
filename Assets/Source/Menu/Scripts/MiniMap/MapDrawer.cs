using System.Collections.Generic;
using UnityEngine;

public class MapDrawer : MonoBehaviour
{
    [SerializeField] private MapPreset _mapPreset;
    [SerializeField] private RectTransform _tilesParent;
    [SerializeField] private Vector2 _tileSize;
    [SerializeField] private Vector2 _mapOffset;
    [SerializeField, Min(0)] private float _maxTileSize;
    [Header("Tiles")]
    [SerializeField] private List<MapTile> _startMapTiles = new();
    [SerializeField] private List<MapTile> _middleMapTiles = new();
    [SerializeField] private List<MapTile> _endMapTiles = new();
    [SerializeField] private RectTransform _startGroundFlyTile;
    [SerializeField] private RectTransform _endGroundFlyTile;
    [SerializeField] private RectTransform _endGroundWallTile;
    [SerializeField] private RectTransform _startWallFlyTile;

    [ContextMenu("Draw Map")]
    public void DrawMap()
    {
        if (_tilesParent == null)
        {
            return;
        }

        ClearMap();
        SpawnMap();
        CenterMap();
    }

    public void DrawMap(MapPreset mapPreset)
    {
        _mapPreset = mapPreset;

        if (isActiveAndEnabled)
        {
            DrawMap();
        }
    }

    private void OnEnable()
    {
        if (_mapPreset != null)
        {
            DrawMap();
        }
    }

    private void ClearMap()
    {
        if (_tilesParent.childCount == 0)
        {
            return;
        }

        for (int i = _tilesParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(_tilesParent.GetChild(i).gameObject);
        }
    }

    private void SpawnMap()
    {
        Vector2Int position = Vector2Int.zero;

        _mapPreset.Map.ForEach(chunkSettings =>
        {
            position += SpawnChunk(position, chunkSettings);
        });
    }

    private void CenterMap()
    {
        if (_tilesParent.childCount == 0)
        {
            return;
        }

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (Transform tile in _tilesParent)
        {
            Vector3 position = tile.localPosition;

            if (position.x < minX)
            {
                minX = position.x;
            }
            if (position.x > maxX)
            {
                maxX = position.x;
            }
            if (position.y < minY)
            {
                minY = position.y;
            }
            if (position.y > maxY)
            {
                maxY = position.y;
            }
        }

        Vector3 center = new((minX + maxX) / 2, (minY + maxY) / 2, _tilesParent.localPosition.z);

        for (int i = 0; i < _tilesParent.childCount; i++)
        {
            _tilesParent.GetChild(i).localPosition -= center;
        }

        Vector2 resize = new()
        {
            x = (_tilesParent.rect.width - _mapOffset.x * 2 * _tileSize.x) / (maxX - minX + _tileSize.x),
            y = (_tilesParent.rect.height - _mapOffset.y * 2 * _tileSize.y) / (maxY - minY + _tileSize.y)
        };

        ResizeMap(resize);
    }

    private void ResizeMap(Vector2 resize)
    {
        float scale = Mathf.Min(resize.x, resize.y);
        scale = Mathf.Min(scale, _maxTileSize);

        foreach (Transform tile in _tilesParent)
        {
            tile.localPosition = new Vector3(tile.localPosition.x * scale, tile.localPosition.y * scale, 0);
            tile.localScale = new Vector3(tile.localScale.x * scale, tile.localScale.y * scale, 1);
        }
    }

    private Vector2Int SpawnChunk(Vector2Int startPosition, ChunkSettings chunkSettings)
    {
        Vector2Int size = chunkSettings.Size;
        ChunkType chunkType = chunkSettings.Type;

        if (chunkType == ChunkType.Fly)
        {
            return new Vector2Int(size.x, -size.x);
        }

        if (chunkType == ChunkType.Start || chunkType == ChunkType.Finish)
        {
            startPosition.x += 1;
            SpawnTiles(startPosition, Vector2Int.zero, chunkSettings);
        }
        else if (chunkType == ChunkType.Wall)
        {
            startPosition.x -= 1;
            SpawnTiles(startPosition, size, chunkSettings);
        }
        else
        {
            SpawnTiles(startPosition, size, chunkSettings);
        }

        return size;
    }

    private void SpawnTiles(Vector2Int startPosition, Vector2Int size, ChunkSettings chunkSettings)
    {
        Vector3 position = (Vector2)startPosition * _tileSize;
        List<MapTile> mapTiles;

        if (size.x == 0)
        {
            size.x = 1;
        }

        if (size.y == 0)
        {
            size.y = 1;
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (x == 0 && y == 0)
                {
                    mapTiles = _startMapTiles;
                }
                else if (x == size.x - 1 && y == size.y - 1)
                {
                    mapTiles = _endMapTiles;
                }
                else
                {
                    mapTiles = _middleMapTiles;
                }

                SpawnTile(mapTiles, position, chunkSettings.Type, chunkSettings);

                position.y += _tileSize.y;
            }

            position.x += _tileSize.x;
            position.y = startPosition.y * _tileSize.y;
        }
    }

    private void SpawnTile(List<MapTile> mapTiles, Vector3 position, ChunkType chunkType, ChunkSettings chunkSettings)
    {
        if (chunkType == ChunkType.Water)
        {
            SpawnTile(mapTiles, position, ChunkType.Ground, chunkSettings);
        }

        RectTransform tile = GetTilePrefab(mapTiles, chunkType, chunkSettings);

        SpawnTile(tile, position, chunkType);
    }

    private RectTransform GetTilePrefab(List<MapTile> mapTiles, ChunkType chunkType, ChunkSettings chunkSettings)
    {
        ChunkType prewievChunkType = _mapPreset.Map[Mathf.Max(_mapPreset.Map.IndexOf(chunkSettings) - 1, 0)].Type;
        ChunkType nextChunkType = _mapPreset.Map[Mathf.Min(_mapPreset.Map.IndexOf(chunkSettings) + 1, _mapPreset.Map.Count - 1)].Type;

        RectTransform tile;

        if (mapTiles == _startMapTiles && chunkType == ChunkType.Ground && prewievChunkType == ChunkType.Fly)
        {
            tile = _startGroundFlyTile;
        }
        else if (mapTiles == _endMapTiles && chunkType == ChunkType.Ground && nextChunkType == ChunkType.Fly)
        {
            tile = _endGroundFlyTile;
        }
        else if (mapTiles == _endMapTiles && chunkType == ChunkType.Ground && nextChunkType == ChunkType.Wall)
        {
            tile = _endGroundWallTile;
        }
        else if (mapTiles == _startMapTiles && chunkType == ChunkType.Wall && prewievChunkType == ChunkType.Fly)
        {
            tile = _startWallFlyTile;
        }
        else
        {
            tile = mapTiles.Find(tile => tile.ChunkType == chunkType).TilePrefab;
        }

        return tile;
    }

    private void SpawnTile(RectTransform tile, Vector3 position, ChunkType chunkType)
    {
        tile = Instantiate(tile, Vector3.zero, Quaternion.identity, _tilesParent);
        tile.localPosition = position;
        tile.name = $"{chunkType}_{position.x}_{position.y}";
    }
}
