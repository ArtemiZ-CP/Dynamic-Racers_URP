using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    private const float OffsetY = 0.25f;
    private const float OffsetZ = 0.1f;
    private const float ChanceToSpawnEmptyOnRoad = 0.4f;
    private const float ChanceToSpawnEmptyOutside = 0.85f;

    [SerializeField] private Transform _environmentParent;

	private GlobalSettings _globalSettings;

	private GlobalSettings GlobalSettings
	{
		get
		{
			if (_globalSettings == null)
			{
				_globalSettings = GlobalSettings.Instance;
			}

			return _globalSettings;
		}
	}

    public void SpawnEnvironment(Vector3Int size, ChunkType chunkType, MapCellsContainer mapCellsContainer)
    {
        if (_environmentParent == null)
        {
            return;
        }

        DestroyMesh();

        Environment.Type type;

        if (chunkType == ChunkType.Wall)
        {
            type = Environment.Type.Wall;
        }
        else
        {
            type = Environment.Type.Ground;
        }

        SpawnEnvironment(size, type, mapCellsContainer.EnvironmentContainer);
    }

    private void DestroyMesh()
    {
        if (_environmentParent.childCount == 0)
        {
            return;
        }

        for (int i = _environmentParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(_environmentParent.GetChild(i).gameObject);
        }
    }

    private void SpawnEnvironment(Vector3Int size, Environment.Type type, EnvironmentContainer environmentContainer)
    {
        int marginWigth = GlobalSettings.ChunkMargin;
        int length = type == Environment.Type.Ground ? size.z : size.y - 1;
        float startMarginPositionX = size.x / 2f * GlobalSettings.RoadsOffset;

        SpawnEnvironment(startMarginPositionX, length, marginWigth, type, false, environmentContainer);
        SpawnEnvironment(-startMarginPositionX, length, marginWigth, type, true, environmentContainer);
        SpawnOutsideEnvironment(startMarginPositionX, size.z, false, environmentContainer);
        SpawnOutsideEnvironment(-startMarginPositionX, size.z, true, environmentContainer);
    }

    private void SpawnOutsideEnvironment(float startMarginPositionX, int length, bool isLeftSide, EnvironmentContainer environmentContainer)
    {
        bool[] environmentMap = new bool[length];

        while (IsMapComplitedFilled(environmentMap) == false)
        {
            int emptyCell = GetEmptyCell(environmentMap);

            if (Random.value < ChanceToSpawnEmptyOutside)
            {
                FillMap(ref environmentMap, emptyCell, 1);
                continue;
            }

            GetMaxEnvironmentSize(environmentMap, emptyCell, out int environmentLength);

            if (environmentLength == 0)
            {
                break;
            }

            Environment environment = environmentContainer.GetRandomEnvironment(Environment.Type.Outside, environmentLength, 1, false);

            if (environment == null)
            {
                FillMap(ref environmentMap, emptyCell, 1);
                continue;
            }

            FillMap(ref environmentMap, emptyCell, environment.Length);
            SpawnEnvironmentObject(environment, startMarginPositionX, emptyCell, isLeftSide);
        }
    }

    private void SpawnEnvironment(float startMarginPositionX, int length, int wight, Environment.Type type, bool isAbleToSpawnHigh, EnvironmentContainer environmentContainer)
    {
        bool[,] environmentMap = new bool[length, wight];

        while (IsMapComplitedFilled(environmentMap) == false)
        {
            Vector2Int emptyCell = GetEmptyCell(environmentMap);

            if (Random.value < ChanceToSpawnEmptyOnRoad)
            {
                FillMap(ref environmentMap, emptyCell, 1, 1);
                continue;
            }

            GetMaxEnvironmentSize(environmentMap, emptyCell, out int environmentLength, out int environmentWight);

            if (environmentLength == 0 || environmentWight == 0)
            {
                break;
            }

            if (type == Environment.Type.Wall)
            {
                isAbleToSpawnHigh = false;
            }

            Environment environment = environmentContainer.GetRandomEnvironment(type, environmentLength, environmentWight, isAbleToSpawnHigh);

            if (environment == null)
            {
                FillMap(ref environmentMap, emptyCell, 1, 1);
                continue;
            }

            FillMap(ref environmentMap, emptyCell, environment.Length, environment.Wigth);
            SpawnEnvironmentObject(environment, startMarginPositionX, emptyCell, isAbleToSpawnHigh == false);
        }
    }

    private void SpawnEnvironmentObject(Environment environment, float startMarginPositionX, Vector2Int emptyCell, bool morror)
    {
        int offset = GlobalSettings.RoadsOffset;
        int marginWigth = GlobalSettings.ChunkMargin;

        Vector3 localPosition = new Vector3(emptyCell.y + environment.Wigth / 2f, 0, 0) * offset;

        if (environment.EnvironmentType == Environment.Type.Ground)
        {
            localPosition += new Vector3(0, 0, emptyCell.x + environment.Length / 2f) * offset;
            localPosition += Vector3.up * (offset + OffsetY);
        }
        else
        {
            localPosition += new Vector3(0, emptyCell.x + environment.Length / 2f, 0) * offset;
            localPosition += Vector3.forward * OffsetZ;
            localPosition += Vector3.up * (offset + OffsetY);
        }

        Vector3 position;

        if (startMarginPositionX > 0)
        {
            position = startMarginPositionX * Vector3.right + localPosition;
        }
        else
        {
            position = startMarginPositionX * Vector3.right + localPosition - marginWigth * offset * Vector3.right;
        }

        position += transform.position;

        Environment newEnvironment = Instantiate(environment, position, Quaternion.identity, _environmentParent);

        if (morror)
        {
            Vector3 newScale = newEnvironment.transform.localScale;
            newScale.x *= -1;
            newEnvironment.transform.localScale = newScale;
        }
    }

    private void SpawnEnvironmentObject(Environment environment, float startMarginPositionX, int emptyCell, bool morror)
    {
        int offset = GlobalSettings.RoadsOffset;
        int marginWigth = GlobalSettings.ChunkMargin;

        Vector3 localPosition = Vector3.zero;

        localPosition += new Vector3(0, 0, emptyCell + environment.Length / 2f) * offset;
        localPosition += Vector3.up * (offset + OffsetY);

        Vector3 position;

        if (startMarginPositionX > 0)
        {
            position = (startMarginPositionX + (marginWigth - 0.5f) * offset) * Vector3.right + localPosition;
        }
        else
        {
            position = (startMarginPositionX - (marginWigth - 0.5f) * offset) * Vector3.right + localPosition;
        }

        position += transform.position;

        Environment newEnvironment = Instantiate(environment, position, Quaternion.identity, _environmentParent);

        if (morror)
        {
            Vector3 newScale = newEnvironment.transform.localScale;
            newScale.x *= -1;
            newEnvironment.transform.localScale = newScale;
        }
    }

    private void FillMap(ref bool[,] map, Vector2Int startPosition, int length, int wigth)
    {
        for (int i = startPosition.x; i < startPosition.x + length; i++)
        {
            for (int j = startPosition.y; j < startPosition.y + wigth; j++)
            {
                map[i, j] = true;
            }
        }
    }

    private void FillMap(ref bool[] map, int startPosition, int length)
    {
        for (int i = startPosition; i < startPosition + length; i++)
        {
            map[i] = true;
        }
    }

    private Vector2Int GetEmptyCell(bool[,] map)
    {
        Vector2Int emptyCell = -Vector2Int.one;

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == false)
                {
                    emptyCell.x = i;
                    emptyCell.y = j;

                    return emptyCell;
                }
            }
        }

        return emptyCell;
    }

    private int GetEmptyCell(bool[] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            if (map[i] == false)
            {
                return i;
            }
        }

        return -1;
    }

    private void GetMaxEnvironmentSize(bool[,] map, Vector2Int emptyCell, out int environmentLength, out int environmentWigth)
    {
        environmentLength = 0;
        environmentWigth = 0;

        for (int wight = emptyCell.y; wight < map.GetLength(1); wight++)
        {
            if (map[emptyCell.x, wight] == false)
            {
                environmentWigth++;
            }
            else
            {
                break;
            }
        }

        for (int length = emptyCell.x; length < map.GetLength(0); length++)
        {
            for (int wight = emptyCell.y; wight < map.GetLength(1); wight++)
            {
                if (map[length, wight])
                {
                    break;
                }
            }

            environmentLength++;
        }
    }

    private void GetMaxEnvironmentSize(bool[] map, int emptyCell, out int environmentLength)
    {
        environmentLength = 0;

        for (int length = emptyCell; length < map.GetLength(0); length++)
        {
            if (map[length])
            {
                break;
            }

            environmentLength++;
        }
    }

    private bool IsMapComplitedFilled(bool[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == false)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool IsMapComplitedFilled(bool[] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == false)
            {
                return false;
            }
        }

        return true;
    }
}
