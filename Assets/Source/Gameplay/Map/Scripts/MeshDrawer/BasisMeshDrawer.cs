using UnityEngine;

public class BasisMeshDrawer : ChunkMeshDrawer
{
    private const int OffsetX = 1;

    [SerializeField] private BasisCellsContainer _basisCellsContainer;

    public override Vector3Int SpawnMesh(Vector3Int size)
    {
        SetSetting();
        size = base.SpawnMesh(size);
        SpawnBasis(size);

        return size;
    }

    private void SetSetting()
    {
        MapCellsContainer mapCellsContainer = RunSettings.MapSetting;

        if (mapCellsContainer != null)
        {
            _basisCellsContainer = RunSettings.MapSetting.BasisCellsContainer;
        }
    }

    private void SpawnBasis(Vector3Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            float xPosition = -size.x / 2f + x;

            if (x == 0 || x == size.x - 1)
            {
                SpawnLine(xPosition, size.z, x == 0, _basisCellsContainer.EdgeCorner, _basisCellsContainer.EdgeMiddle);
            }
            else
            {
                SpawnLine(xPosition, size.z, true, _basisCellsContainer.Corner, _basisCellsContainer.Middle);
            }
        }
    }
}
