using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(BasisMeshDrawer))]
public class WallMeshDrawer : ChunkMeshDrawer
{
	private BasisMeshDrawer _basisMeshDrawer;
	
	public override Vector3Int SpawnMesh(Vector3Int size, MapCellsContainer mapCellsContainer, bool emptyBefore, bool emptyAfter)
	{
		size.z = 3;

		_basisMeshDrawer.SpawnMesh(size, mapCellsContainer, emptyBefore, emptyAfter);
		size = base.SpawnMesh(size, mapCellsContainer, emptyBefore, emptyAfter);
		SpawnWall(size, mapCellsContainer.WallCellsContainer, mapCellsContainer.WallMarginCellsContainer, emptyBefore, emptyAfter);

		return size;
	}

	private void Awake()
	{
		_basisMeshDrawer = GetComponent<BasisMeshDrawer>();
	}

	private void SpawnWall(Vector3Int size, WallCellsContainer wallCellsContainer, WallMarginCellsContainer wallMarginCellsContainer, bool emptyBefore, bool emptyAfter)
	{
		int min = GlobalSettings.Instance.ChunkMargin;
		int max = size.x - 1 - min;

		for (int x = 0; x < size.x; x++)
		{
			float xPosition = -size.x / 2f + x;

			if (x < min)
			{
				if (x == 0)
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Side, mirror: false, wallMarginCellsContainer, emptyBefore, emptyAfter);
				}
				else if (x == min - 1)
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Road, mirror: false, wallMarginCellsContainer, emptyBefore, emptyAfter);
				}
				else
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Middle, mirror: false, wallMarginCellsContainer, emptyBefore, emptyAfter);
				}
			}
			else if (x > max)
			{
				if (x == size.x - 1)
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Side, mirror: true, wallMarginCellsContainer, emptyBefore, emptyAfter);
				}
				else if (x == max + 1)
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Road, mirror: true, wallMarginCellsContainer, emptyBefore, emptyAfter);
				}
				else
				{
					SpawnMarginLine(xPosition, size.y, MarginsOffset.Middle, mirror: true, wallMarginCellsContainer, emptyBefore, emptyAfter);
				}
			}
			else
			{
				if (x == min)
				{
					SpawnWallLine(xPosition, size.y, mirror: false,
					wallCellsContainer.StartSide,
					wallCellsContainer.MiddleSide,
					wallCellsContainer.EndSide, emptyBefore, emptyAfter);
				}
				else if (x == max)
				{
					SpawnWallLine(xPosition, size.y, mirror: true,
					wallCellsContainer.StartSide,
					wallCellsContainer.MiddleSide,
					wallCellsContainer.EndSide, emptyBefore, emptyAfter);
				}
				else
				{
					SpawnWallLine(xPosition, size.y, mirror: false,
					wallCellsContainer.StartMiddle,
					wallCellsContainer.MiddleMiddle,
					wallCellsContainer.EndMiddle, emptyBefore, emptyAfter);
				}
			}
		}
	}

	private void SpawnMarginLine(float xPosition, int lenght, MarginsOffset marginsOffset, bool mirror, WallMarginCellsContainer wallMarginCellsContainer, bool emptyBefore, bool emptyAfter)
	{
		if (wallMarginCellsContainer == null)
		{
			return;
		}

		GameObject start = null;
		GameObject middle = null;
		GameObject end = null;

		if (marginsOffset == MarginsOffset.Road)
		{
			start = wallMarginCellsContainer.StartRoadMargin;
			middle = wallMarginCellsContainer.MiddleRoadMargin;
			end = wallMarginCellsContainer.EndRoadMargin;
		}
		else if (marginsOffset == MarginsOffset.Middle)
		{
			start = wallMarginCellsContainer.StartMiddleMargin;
			middle = wallMarginCellsContainer.MiddleMiddleMargin;
			end = wallMarginCellsContainer.EndMiddleMargin;
		}
		else if (marginsOffset == MarginsOffset.Side)
		{
			start = wallMarginCellsContainer.StartSideMargin;
			middle = wallMarginCellsContainer.MiddleSideMargin;
			end = wallMarginCellsContainer.EndSideMargin;
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
				SpawnCell(start, new Vector3(xPosition, y, 1), size, emptyBefore, emptyAfter);
			}
			else if (y == lenght)
			{
				SpawnCell(end, new Vector3(xPosition, y, 1), size, emptyBefore, emptyAfter);
			}
			else
			{
				SpawnCell(middle, new Vector3(xPosition, y, 1), size, emptyBefore, emptyAfter);
			}
		}
	}
}
