using UnityEngine;

public class GroundMeshDrawer : ChunkMeshDrawer
{
	[SerializeField] private GameObject _edge;
	[SerializeField] private GameObject _middle;

	public Vector3Int SpawnMesh(Vector3Int size, float offsetX)
	{
		SpawnGround(size, offsetX + GlobalSettings.Instance.RoadsOffset / 4f);

		return size;
	}

	public override Vector3Int SpawnMesh(Vector3Int size)
	{
		size = base.SpawnMesh(size);
		SpawnGround(size);

		return size;
	}

	private void SpawnGround(Vector3Int size, float offsetX = 0)
	{
		if (_edge == null || _middle == null)
		{
			return;
		}

		for (int x = 0; x < size.x; x++)
		{
			float xPosition = (-size.x / 2f + x) + offsetX;
			SpawnLine(xPosition, size.z);
		}
	}

	private void SpawnLine(float xPosition, int lenght)
	{
		SpawnBoard(xPosition, 0, lenght);
	}

	private void SpawnBoard(float xPositionStart, int zPositionStart, int lenght)
	{
		float firstEdgePositionZ = zPositionStart;
		float lastEdgePositionZ = zPositionStart + lenght - 1;

		SpawnCell(_edge, new Vector3(xPositionStart, 0, firstEdgePositionZ), new Vector3(1, 1, 1));
		SpawnCell(_edge, new Vector3(xPositionStart, 0, lastEdgePositionZ), new Vector3(1, 1, -1));

		if (lenght > 2)
		{
			float length = lastEdgePositionZ - firstEdgePositionZ - 1;
			float zPosition = (firstEdgePositionZ + lastEdgePositionZ) / 2;

			SpawnCell(_middle, new Vector3(xPositionStart, 0, zPosition), new Vector3(1, 1, length));
		}
	}
}
