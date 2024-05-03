using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMeshDrawer : ChunkMeshDrawer
{
	[SerializeField] private GroundMeshDrawer _groundMeshDrawer;
	[SerializeField] private GameObject _bottom;
	[SerializeField] private GameObject _poolMid;
	[SerializeField] private GameObject _poolCorner;
	[SerializeField] private GameObject _poolSide;

	public override Vector3Int SpawnMesh(Vector3Int size)
	{
		size = base.SpawnMesh(size);
		_groundMeshDrawer.DestroyMesh();
		SpawnWaterpool(size);

		return size;
	}

	private void SpawnWaterpool(Vector3Int size)
	{
		if (_bottom == null || _poolMid == null || _poolCorner == null || _poolSide == null)
		{
			return;
		}

		int min = GlobalSettings.Instance.ChunkMargin - 1;
		int max = size.x - 1 - min;

		for (int x = 0; x < size.x; x++)
		{
			float xPosition = (-size.x / 2f + x);

			if (x < min)
			{
				SpawnGroundLine(xPosition, size.z);
			}
			else if (x > max)
			{
				SpawnGroundLine(xPosition, size.z);
			}
			else
			{
				if (x == min)
				{
					SpawnWaterpoolLine(xPosition, size.z - 1, mirror: false, _poolCorner, _poolSide);
				}
				else if (x == max)
				{
					SpawnWaterpoolLine(xPosition, size.z - 1, mirror: true, _poolCorner, _poolSide);
				}
				else
				{
					SpawnWaterpoolLine(xPosition, size.z - 1, mirror: false, _poolMid, _bottom);
				}
			}
		}
	}

	private void SpawnGroundLine(float xPosition, int lenght)
	{
		_groundMeshDrawer.SpawnMesh(new Vector3Int(1, 1, lenght), xPosition);
	}

	private void SpawnWaterpoolLine(float xPosition, int lenght, bool mirror, GameObject start, GameObject middle)
	{
		Vector3 size = new(1, 1, 1);

		if (mirror)
		{
			size.x *= -1;
		}

		SpawnCell(start, new Vector3(xPosition, 0, 0), size);
		size.z *= -1;
		SpawnCell(start, new Vector3(xPosition, 0, lenght), size);

		if (lenght > 1)
		{
			float positionZ = (lenght) / 2f;
			size.z = lenght - 1;
			SpawnCell(middle, new Vector3(xPosition, 0, positionZ), size);
		}
	}
}
