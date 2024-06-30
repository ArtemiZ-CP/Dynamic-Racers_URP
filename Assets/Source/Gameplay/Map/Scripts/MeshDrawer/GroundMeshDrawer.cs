using UnityEngine;

public class GroundMeshDrawer : ChunkMeshDrawer
{
	[SerializeField] private BasisMeshDrawer _basisMeshDrawer;
	[SerializeField] private GroundCellsContainer _groundCellsContainer;

	public override Vector3Int SpawnMesh(Vector3Int size)
	{
		SetSetting();
		_basisMeshDrawer.SpawnMesh(size);
		size = base.SpawnMesh(size);
		SpawnGround(size);

		return size;
	}

	private void SetSetting()
	{
		MapCellsContainer mapCellsContainer = RunSettings.MapSetting;

		if (mapCellsContainer != null)
		{
			_groundCellsContainer = RunSettings.MapSetting.GroundCellsContainer;
		}
	}

	private void SpawnGround(Vector3Int size)
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
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Side, mirror: false);
				}
				else if (x == min - 1)
				{
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Road, mirror: false);
				}
				else
				{
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Middle, mirror: false);
				}
			}
			else if (x > max)
			{
				if (x == size.x - 1)
				{
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Side, mirror: true);
				}
				else if (x == max + 1)
				{
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Road, mirror: true);
				}
				else
				{
					SpawnMarginLine(xPosition, size.z, MarginsOffset.Middle, mirror: true);
				}
			}
			else
			{
				if (x == min)
				{
					SpawnLine(xPosition, size.z, mirror: false,
					_groundCellsContainer.StartSide,
					_groundCellsContainer.MiddleSide);
				}
				else if (x == max)
				{
					SpawnLine(xPosition, size.z, mirror: true,
					_groundCellsContainer.StartSide,
					_groundCellsContainer.MiddleSide);
				}
				else
				{
					SpawnLine(xPosition, size.z, mirror: false,
					_groundCellsContainer.StartMiddle,
					_groundCellsContainer.MiddleMiddle);
				}
			}
		}
	}

	private void SpawnMarginLine(float xPosition, int lenght, MarginsOffset marginsOffset, bool mirror)
	{
		GameObject start = null;
		GameObject middle = null;

		if (marginsOffset == MarginsOffset.Road)
		{
			start = _groundCellsContainer.StartRoadMargin;
			middle = _groundCellsContainer.RoadMargin;
		}
		else if (marginsOffset == MarginsOffset.Middle)
		{
			start = _groundCellsContainer.StartMiddleMargin;
			middle = _groundCellsContainer.MiddleMargin;
		}
		else if (marginsOffset == MarginsOffset.Side)
		{
			start = _groundCellsContainer.StartSideMargin;
			middle = _groundCellsContainer.SideMargin;
		}
		
		Vector3 size = new(1, 1, 1);

		if (mirror)
		{
			size.x *= -1;
		}

		for (int z = 0; z < lenght; z++)
		{
			if (z == 0)
			{
				SpawnCell(start, new Vector3(xPosition, 0, z), size);
			}
			else if (z == lenght - 1)
			{
				size.z *= -1;
				SpawnCell(start, new Vector3(xPosition, 0, z), size);
				size.z *= -1;
			}
			else
			{
				SpawnCell(middle, new Vector3(xPosition, 0, z), size);
			}
		}
	}
}
