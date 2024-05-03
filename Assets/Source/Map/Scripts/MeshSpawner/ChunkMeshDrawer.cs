using UnityEditor;
using UnityEngine;

public abstract class ChunkMeshDrawer : MonoBehaviour
{
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
