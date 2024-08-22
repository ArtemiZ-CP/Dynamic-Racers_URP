using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesBackground : MonoBehaviour
{
    [SerializeField] private BackgroundTile _tilePrefab;
    [SerializeField] private Vector2Int _tilesCountInGrid;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _isCanvasOverlay;
    [SerializeField] private bool _toggleBlicks;
    [Header("Blick Duration")]
    [SerializeField] private float _startBlickDuration;
    [SerializeField] private float _middleBlickDuration;
    [SerializeField] private float _endBlickDuration;
    [Header("Blick Intensity")]
    [SerializeField] private float _maxBlickIntesity;
    [Header("Blick Interval")]
    [SerializeField] private float _blickIntervalMin;
    [SerializeField] private float _blickIntervalMax;
    [Header("Reverse")]
    [SerializeField] private float _speedReverseTime;
    [SerializeField] private float _speedReverseDuration;

    private BackgroundTile[,] _tiles;
    private Vector2 _tileSize;
    private float _maxTilePositionX;

    private void Awake()
    {
        _tileSize = _tilePrefab.GetComponent<RectTransform>().sizeDelta;
        SpawnTiles();
    }

    private void OnEnable()
    {
        StartCoroutine(MoveTiles());
        StartCoroutine(ReverseSpeedCoroutine());
        if (_toggleBlicks) StartCoroutine(BlickTiles());
    }

    private void SpawnTiles()
    {
        _tiles = new BackgroundTile[_tilesCountInGrid.x, _tilesCountInGrid.y];

        for (int x = 0; x < _tilesCountInGrid.x; x++)
        {
            for (int y = 0; y < _tilesCountInGrid.y; y++)
            {
                Vector3 position = new(x, y, 0);
                _tiles[x, y] = SpawnTile(position);
            }
        }
    }

    private BackgroundTile SpawnTile(Vector3 localPosition)
    {
        BackgroundTile tile = Instantiate(_tilePrefab, transform);

        localPosition -= new Vector3(_tilesCountInGrid.x / 2f, _tilesCountInGrid.y / 2f);
        localPosition *= _tileSize;
        localPosition += new Vector3(_tileSize.x / 2f, _tileSize.y / 2f);

        tile.transform.localPosition = localPosition;
        tile.transform.rotation = transform.rotation;

        if (localPosition.x > _maxTilePositionX)
        {
            _maxTilePositionX = localPosition.x;
        }

        return tile;
    }

    private IEnumerator MoveTiles()
    {
        while (true)
        {
            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _tiles.GetLength(1); j++)
                {
                    if (j % 2 == 0)
                    {
                        _tiles[i, j].transform.localPosition += Vector3.right * _moveSpeed * Time.deltaTime;
                    }
                    else
                    {
                        _tiles[i, j].transform.localPosition += Vector3.left * _moveSpeed * Time.deltaTime;
                    }

                    MoveTileToStartLine(_tiles[i, j]);
                }
            }

            yield return null;
        }
    }

    private IEnumerator ReverseSpeedCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_speedReverseTime);
            StartCoroutine(ReverseSpeed());
        }
    }

    private IEnumerator ReverseSpeed()
    {
        float time = 0;
        float startSpeed = _moveSpeed;

        while (time < _speedReverseDuration)
        {
            _moveSpeed = Mathf.Lerp(startSpeed, -startSpeed, time / _speedReverseDuration);
            time += Time.deltaTime;
            yield return null;
        }

        _moveSpeed = -startSpeed;
    }

    private void MoveTileToStartLine(BackgroundTile tile)
    {
        if (tile.transform.localPosition.x > _maxTilePositionX)
        {
            tile.transform.localPosition += Vector3.left * _maxTilePositionX * 2;
            tile.SetRandomSprite();
        }

        if (tile.transform.localPosition.x < -_maxTilePositionX)
        {
            tile.transform.localPosition += Vector3.right * _maxTilePositionX * 2;
            tile.SetRandomSprite();
        }
    }

    private IEnumerator BlickTiles()
    {
        while (true)
        {
            GetRandomTileInsideScreen()?.Blick(_startBlickDuration, _middleBlickDuration, _endBlickDuration, _maxBlickIntesity);

            yield return new WaitForSeconds(Random.Range(_blickIntervalMin, _blickIntervalMax));
        }
    }

    private BackgroundTile GetRandomTileInsideScreen()
    {
        List<BackgroundTile> tilesInScreen = new();

        foreach (BackgroundTile tile in _tiles)
        {
            if (IsTileInsideScreen(tile))
            {
                tilesInScreen.Add(tile);
            }
        }

        if (tilesInScreen.Count == 0)
        {
            return null;
        }

        return tilesInScreen[Random.Range(0, tilesInScreen.Count)];
    }

    private bool IsTileInsideScreen(BackgroundTile tile)
    {
        Vector3 screenPosition;

        if (_isCanvasOverlay)
        {
            screenPosition = tile.transform.position;
        }
        else
        {
            screenPosition = Camera.main.WorldToScreenPoint(tile.transform.position);
        }

        return screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height;
    }
}
