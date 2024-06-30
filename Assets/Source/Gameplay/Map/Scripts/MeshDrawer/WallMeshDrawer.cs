using UnityEngine;

public class WallMeshDrawer : ChunkMeshDrawer
{
	private const int CarpetOffset = 1;

	[SerializeField] private BasisMeshDrawer _basisMeshDrawer;
	[SerializeField] private WallCellsContainer _wallCellsContainer;

	public override Vector3Int SpawnMesh(Vector3Int size)
	{
		size.z = 2;
		
		SetSetting();
		_basisMeshDrawer.SpawnMesh(size);
		size = base.SpawnMesh(size);
		SpawnWall(size);

		return size;
	}

    private void SetSetting()
    {
        MapCellsContainer mapCellsContainer = RunSettings.MapSetting;

        if (mapCellsContainer != null)
        {
            _wallCellsContainer = RunSettings.MapSetting.WallCellsContainer;
        }
    }

	private void SpawnWall(Vector3Int size)
	{
		int min = GlobalSettings.Instance.ChunkMargin - 1;
		int max = size.x - 1 - min;

		for (int x = 0; x < size.x; x++)
		{
			float xPosition = -size.x / 2f + x;

			if (x < min)
			{
				if (x == 0)
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Side, mirror: false);
				}
				else if (x == min - 1)
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Road, mirror: false);
				}
				else
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Middle, mirror: false);
				}
			}
			else if (x > max)
			{
				if (x == size.x - 1)
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Side, mirror: true);
				}
				else if (x == max + 1)
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Road, mirror: true);
				}
				else
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Middle, mirror: true);
				}
			}
			else
			{
				if (x == min)
				{
					SpawnWallLine(xPosition, size.y, mirror: false,
					_wallCellsContainer.StartSide,
					_wallCellsContainer.MiddleSide,
					_wallCellsContainer.EndSide);
				}
				else if (x == max)
				{
					SpawnWallLine(xPosition, size.y, mirror: true,
					_wallCellsContainer.StartSide,
					_wallCellsContainer.MiddleSide,
					_wallCellsContainer.EndSide);
				}
				else
				{
					SpawnWallLine(xPosition, size.y, mirror: false,
					_wallCellsContainer.StartMiddle,
					_wallCellsContainer.MiddleMiddle,
					_wallCellsContainer.EndMiddle);
				}
			}
		}
	}

	private void SpawnMarginLine(float xPosition, int lenght, MarginsOffset marginsOffset, bool mirror)
	{
		GameObject start = null;
		GameObject middle = null;
		GameObject end = null;

		if (marginsOffset == MarginsOffset.Road)
		{
			start = _wallCellsContainer.StartRoadMargin;
			middle = _wallCellsContainer.RoadMargin;
			end = _wallCellsContainer.EndRoadMargin;
		}
		else if (marginsOffset == MarginsOffset.Middle)
		{
			start = _wallCellsContainer.StartMiddleMargin;
			middle = _wallCellsContainer.MiddleMargin;
			end = _wallCellsContainer.EndMiddleMargin;
		}
		else if (marginsOffset == MarginsOffset.Side)
		{
			start = _wallCellsContainer.StartSideMargin;
			middle = _wallCellsContainer.SideMargin;
			end = _wallCellsContainer.EndSideMargin;
		}
		
		Vector3 size = new(1, 1, 1);

		if (mirror)
		{
			size.x *= -1;
		}

		for (int y = 1; y <= lenght; y++)
		{
			if (y == 1)
			{
				SpawnCell(start, new Vector3(xPosition, y, 1), size);
			}
			else if (y == lenght)
			{
				SpawnCell(end, new Vector3(xPosition, y, 1), size);
			}
			else
			{
				SpawnCell(middle, new Vector3(xPosition, y, 1), size);
			}
		}
	}
}
