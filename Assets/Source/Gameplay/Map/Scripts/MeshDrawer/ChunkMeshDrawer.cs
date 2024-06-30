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

	public Transform MeshParent => _meshParent;

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

	public virtual Vector3Int SpawnMesh(Vector3Int size)
	{
		if (size.x < GlobalSettings.Instance.MinRoadsCount)
		{
			size.x = GlobalSettings.Instance.MinRoadsCount;
		}

		size.x = Mathf.Max(size.x, 1);
		size.x += GlobalSettings.Instance.ChunkMargin * 2;

		DestroyMesh();

		return size;
	}

	protected void SpawnLine(float xPosition, int lenght, bool mirror, GameObject start, GameObject middle)
	{
		Vector3 size = new(1, 1, 1);
		lenght--;

		if (mirror)
		{
			size.x *= -1;
		}

		SpawnCell(start, new Vector3(xPosition, 0, 0), size);
		size.z *= -1;
		SpawnCell(start, new Vector3(xPosition, 0, lenght), size);

		if (lenght > 1)
		{
			float positionZ = lenght / 2f;
			size.z = lenght - 1;
			SpawnCell(middle, new Vector3(xPosition, 0, positionZ), size);
		}
	}

	protected void SpawnWallLine(float xPosition, int lenght, bool mirror, GameObject start, GameObject middle, GameObject end)
	{
		Vector3 size = new(1, 1, 1);

		if (mirror)
		{
			size.x *= -1;
		}

		SpawnCell(start, new Vector3(xPosition, 1, 1), size);
		SpawnCell(end, new Vector3(xPosition, lenght, 1), size);

		if (lenght > 2)
		{
			float positionY = (1 + lenght) / 2f;
			size.y = lenght - 2;
			SpawnCell(middle, new Vector3(xPosition, positionY, 1), size);
		}
	}

	protected void SpawnCell(GameObject objectToSpawn, Vector3 position, Vector3 scaleMultiplier)
	{
		SpawnObject(objectToSpawn, CellToWorldPosition(position), scaleMultiplier);
	}

	private Vector3 CellToWorldPosition(Vector3 position)
	{
		Vector3 newPosition = position;
		newPosition += Vector3.one / 2;
		newPosition *= GlobalSettings.Instance.RoadsOffset;
		newPosition += transform.position;
		return newPosition;
	}

	private void SpawnObject(GameObject objectToSpawn, Vector3 position, Vector3 scaleMultiplier)
	{
		if (objectToSpawn == null)
		{
			return;
		}

#if UNITY_EDITOR
		objectToSpawn = (GameObject)PrefabUtility.InstantiatePrefab(objectToSpawn, MeshParent);
		objectToSpawn.transform.position = position;
#else
		objectToSpawn = Instantiate(objectToSpawn, position, Quaternion.identity, MeshParent);
#endif

		Vector3 newScale = objectToSpawn.transform.localScale;
		newScale.x *= scaleMultiplier.x;
		newScale.y *= scaleMultiplier.y;
		newScale.z *= scaleMultiplier.z;
		objectToSpawn.transform.localScale = newScale;
	}
}
