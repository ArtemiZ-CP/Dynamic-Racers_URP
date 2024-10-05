using UnityEditor;
using UnityEngine;

public abstract class ChunkMeshDrawer : MonoBehaviour
{
	protected enum MarginsOffset
	{
		Side,
		Middle,
		Road
	}

	[SerializeField] private Transform _meshParent;

	private GlobalSettings _globalSettings;

	public Transform MeshParent => _meshParent;

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

	public void DestroyMesh()
	{
		if (_meshParent.childCount == 0)
		{
			return;
		}

		for (int i = _meshParent.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(_meshParent.GetChild(i).gameObject);
		}
	}

	public virtual Vector3Int SpawnMesh(Vector3Int size, MapCellsContainer mapCellsContainer, bool emptyBefore, bool emptyAfter)
	{
		if (size.x < GlobalSettings.MinRoadsCount)
		{
			size.x = GlobalSettings.MinRoadsCount;
		}

		size.x = Mathf.Max(size.x, 1);
		size.x += GlobalSettings.ChunkMargin * 2;

		DestroyMesh();

		return size;
	}

	protected void SpawnLine(float xPosition, int lenght, bool mirror, GameObject start, GameObject middle, bool emptyBefore, bool emptyAfter)
	{
		Vector3 size = new(1, 1, 1);

		if (mirror)
		{
			size.x *= -1;
		}

		for (int z = 0; z < lenght; z++)
		{
			if (z == 0)
			{
				SpawnCell(start, new Vector3(xPosition, 0, z), size, emptyBefore, emptyAfter);
			}
			else if (z == lenght - 1)
			{
				size.z *= -1;
				SpawnCell(start, new Vector3(xPosition, 0, z), size, emptyBefore, emptyAfter);
				size.z *= -1;
			}
			else
			{
				SpawnCell(middle, new Vector3(xPosition, 0, z), size, emptyBefore, emptyAfter);
			}
		}
	}

	protected void SpawnLine(float xPosition, int lenght, bool mirror, GameObject start, GameObject second, GameObject middle, GameObject end, bool emptyBefore, bool emptyAfter)
	{
		Vector3 size = new(1, 1, 1);

		if (mirror)
		{
			size.x *= -1;
		}

		for (int z = 0; z < lenght; z++)
		{
			if (z == 0)
			{
				SpawnCell(start, new Vector3(xPosition, 0, z), size, emptyBefore, emptyAfter);
			}
			else if (z == 1)
			{
				SpawnCell(second, new Vector3(xPosition, 0, 1), size, emptyBefore, emptyAfter);
			}
			else if (z == lenght - 1)
			{
				size.z *= -1;
				SpawnCell(end, new Vector3(xPosition, 0, z), size, emptyBefore, emptyAfter);
				size.z *= -1;
			}
			else
			{
				SpawnCell(middle, new Vector3(xPosition, 0, z), size, emptyBefore, emptyAfter);
			}
		}
	}

	protected void SpawnWallLine(float xPosition, int lenght, bool mirror, GameObject start, GameObject middle, GameObject end, bool emptyBefore, bool emptyAfter)
	{
		Vector3 size = new(1, 1, 1);

		if (mirror)
		{
			size.x *= -1;
		}

		for (int y = 0; y < lenght; y++)
		{
			if (y == 0)
			{
				SpawnCell(start, new Vector3(xPosition, 1, 1), size, emptyBefore, emptyAfter);
			}
			else if (y == lenght - 1)
			{
				SpawnCell(end, new Vector3(xPosition, lenght, 1), size, emptyBefore, emptyAfter);
			}
			else
			{
				SpawnCell(middle, new Vector3(xPosition, y + 1, 1), size, emptyBefore, emptyAfter);
			}
		}
	}

	protected GameObject SpawnCell(GameObject objectToSpawn, Vector3 position, Vector3 scaleMultiplier, bool emptyBefore, bool emptyAfter)
	{
		if (scaleMultiplier.z < 0)
		{
			(emptyBefore, emptyAfter) = (emptyAfter, emptyBefore);
		}
		
		GameObject gameObject = SpawnObject(objectToSpawn, CellToWorldPosition(position), scaleMultiplier, emptyBefore, emptyAfter);

		if (gameObject != null && gameObject.TryGetComponent(out TileEditor tileEditor))
		{
			tileEditor.SetupTile(emptyBefore, emptyAfter);
		}

		return gameObject;
	}

	private Vector3 CellToWorldPosition(Vector3 position)
	{
		Vector3 newPosition = position;
		newPosition += Vector3.one / 2;
		newPosition *= GlobalSettings.RoadsOffset;
		newPosition += transform.position;
		return newPosition;
	}

	private GameObject SpawnObject(GameObject objectToSpawn, Vector3 position, Vector3 scaleMultiplier, bool emptyBefore, bool emptyAfter)
	{
		if (objectToSpawn == null)
		{
			return null;
		}

		if (Application.isPlaying == false)
		{
#if UNITY_EDITOR
			objectToSpawn = (GameObject)PrefabUtility.InstantiatePrefab(objectToSpawn, MeshParent);
			objectToSpawn.transform.position = position;
#else
			objectToSpawn = Instantiate(objectToSpawn, position, Quaternion.identity, MeshParent);
#endif
		}
		else
		{
			objectToSpawn = Instantiate(objectToSpawn, position, Quaternion.identity, MeshParent);
		}

		Vector3 newScale = objectToSpawn.transform.localScale;
		newScale.x *= scaleMultiplier.x;
		newScale.y *= scaleMultiplier.y;
		newScale.z *= scaleMultiplier.z;
		objectToSpawn.transform.localScale = newScale;

		return objectToSpawn;
	}
}
