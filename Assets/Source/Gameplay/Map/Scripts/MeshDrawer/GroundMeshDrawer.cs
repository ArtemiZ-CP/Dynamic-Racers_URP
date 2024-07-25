using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(BasisMeshDrawer))]
public class GroundMeshDrawer : ChunkMeshDrawer
{
	private BasisMeshDrawer _basisMeshDrawer;

	public override Vector3Int SpawnMesh(Vector3Int size, MapCellsContainer mapCellsContainer, bool emptyBefore, bool emptyAfter)
	{
		_basisMeshDrawer.SpawnMesh(size, mapCellsContainer, emptyBefore, emptyAfter);
		size = base.SpawnMesh(size, mapCellsContainer, emptyBefore, emptyAfter);
		SpawnGround(size, mapCellsContainer.GroundCellsContainer, mapCellsContainer.MarginCellsContainer, emptyBefore, emptyAfter);

		return size;
	}

	private void Awake()
	{
		_basisMeshDrawer = GetComponent<BasisMeshDrawer>();
	}

	private void SpawnGround(Vector3Int size, GroundCellsContainer groundCellsContainer, MarginCellsContainer marginCellsContainer, bool emptyBefore, bool emptyAfter)
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
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Side, mirror: false, marginCellsContainer, emptyBefore, emptyAfter);
				}
				else if (x == min - 1)
				{
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Road, mirror: false, marginCellsContainer, emptyBefore, emptyAfter);
				}
				else
				{
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Middle, mirror: false, marginCellsContainer, emptyBefore, emptyAfter);
				}
			}
			else if (x > max)
			{
				if (x == size.x - 1)
				{
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Side, mirror: true, marginCellsContainer, emptyBefore, emptyAfter);
				}
				else if (x == max + 1)
				{
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Road, mirror: true, marginCellsContainer, emptyBefore, emptyAfter);
				}
				else
				{
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Middle, mirror: true, marginCellsContainer, emptyBefore, emptyAfter);
				}
			}
			else
			{
				if (x == min)
				{
					SpawnLine(xPosition, size.z, mirror: false,
					groundCellsContainer.StartSide,
					groundCellsContainer.MiddleSide, emptyBefore, emptyAfter);
				}
				else if (x == max)
				{
					SpawnLine(xPosition, size.z, mirror: true,
					groundCellsContainer.StartSide,
					groundCellsContainer.MiddleSide, emptyBefore, emptyAfter);
				}
				else
				{
					SpawnLine(xPosition, size.z, mirror: false,
					groundCellsContainer.StartMiddle,
					groundCellsContainer.MiddleMiddle, emptyBefore, emptyAfter);
				}
			}
		}
	}

	private void SpawnMarginLine(float xPosition, int lenght, MarginsOffset marginsOffset, bool mirror, MarginCellsContainer marginCellsContainer, bool emptyBefore, bool emptyAfter)
	{
		GameObject start = null;
		GameObject middle = null;

		if (marginsOffset == MarginsOffset.Road)
		{
			start = marginCellsContainer.StartRoadMargin;
			middle = marginCellsContainer.MiddleRoadMargin;
		}
		else if (marginsOffset == MarginsOffset.Middle)
		{
			start = marginCellsContainer.StartMiddleMargin;
			middle = marginCellsContainer.MiddleMiddleMargin;
		}
		else if (marginsOffset == MarginsOffset.Side)
		{
			start = marginCellsContainer.StartSideMargin;
			middle = marginCellsContainer.MiddleSideMargin;
		}

		SpawnLine(xPosition, lenght, mirror, start, middle, emptyBefore, emptyAfter);
	}
}
