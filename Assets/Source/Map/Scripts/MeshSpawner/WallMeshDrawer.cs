using UnityEngine;

public class WallMeshDrawer : ChunkMeshDrawer
{
	[SerializeField] private GroundMeshDrawer _groundMeshSpawner;
	[Header("Stair")]
	[SerializeField] private GameObject _corner;
	[SerializeField] private GameObject _downMiddle;
	[SerializeField] private GameObject _sideMiddle;
	[SerializeField] private GameObject _middle;
	[Header("Carpet")]
	[SerializeField] private GameObject _carpetStartMiddle;
	[SerializeField] private GameObject _carpetMiddleMiddle;
	[SerializeField] private GameObject _carpetFinishMiddle;
	[SerializeField] private GameObject _carpetStartSide;
	[SerializeField] private GameObject _carpetMiddleSide;
	[SerializeField] private GameObject _carpetFinishSide;

	public override Vector3Int SpawnMesh(Vector3Int size)
	{
		if (size.y < 3)
		{
			size.y = 3;
		}

		size.z = 2;

		_groundMeshSpawner.SpawnMesh(size);

		size = base.SpawnMesh(size);
		SpawnWall(size);

		return size;
	}

	private void SpawnWall(Vector3Int size)
	{
		if (_corner == null || _downMiddle == null || _sideMiddle == null || _middle == null)
		{
			return;
		}

		size.y--;
		int min = GlobalSettings.Instance.ChunkMargin;
		int max = size.x - 1 - min;

		for (int x = 0; x < size.x; x++)
		{
			float xPosition = (-size.x / 2f + x);

			if (x == 0)
			{
				SpawnWallLine(xPosition, size.y, mirror: false, _corner, _sideMiddle);
			}
			else if (x == size.x - 1)
			{
				SpawnWallLine(xPosition, size.y, mirror: true, _corner, _sideMiddle);
			}
			else
			{
				SpawnWallLine(xPosition, size.y, mirror: false, _downMiddle, _middle);

				if (x == min)
				{
					SpawnCarpetLine(xPosition, size.y, mirror: false, _carpetStartSide, _carpetMiddleSide, _carpetFinishSide);
				}
				else if (x == max)
				{
					SpawnCarpetLine(xPosition, size.y, mirror: true, _carpetStartSide, _carpetMiddleSide, _carpetFinishSide);
				}
				else if (x > min && x < max)
				{
					SpawnCarpetLine(xPosition, size.y, mirror: false, _carpetStartMiddle, _carpetMiddleMiddle, _carpetFinishMiddle);
				}
			}
		}
	}

	private void SpawnWallLine(float xPosition, int lenght, bool mirror, GameObject down, GameObject middle)
	{
		Vector3 size = new(1, 1, 1);

		if (mirror)
		{
			size.x *= -1;
		}

		SpawnCell(down, new Vector3(xPosition, 1, 1), size);

		size.y *= -1;
		SpawnCell(down, new Vector3(xPosition, lenght, 1), size);

		if (lenght > 2)
		{
			float positionY = (1 + lenght) / 2f;
			size.y = lenght - 2;
			SpawnCell(middle, new Vector3(xPosition, positionY, 1), size);
		}
	}

	private void SpawnCarpetLine(float xPosition, int lenght, bool mirror, GameObject start, GameObject middle, GameObject finish)
	{
		Vector3 size = new(1, 1, 1);

		if (mirror)
		{
			size.x *= -1;
		}

		SpawnCell(start, new Vector3(xPosition, 1, 0), size);

		SpawnCell(finish, new Vector3(xPosition, lenght + 1, 0), size);

		if (lenght > 1)
		{
			float positionY = (2 + lenght) / 2f;
			size.y = lenght - 2;
			SpawnCell(middle, new Vector3(xPosition, positionY, 0), size + Vector3.up);
		}
	}
}
