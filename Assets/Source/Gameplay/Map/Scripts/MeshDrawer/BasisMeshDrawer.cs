using UnityEngine;

public class BasisMeshDrawer : ChunkMeshDrawer
{
    public override Vector3Int SpawnMesh(Vector3Int size, MapCellsContainer mapCellsContainer, bool emptyBefore, bool emptyAfter)
    {
        size = base.SpawnMesh(size, mapCellsContainer, emptyBefore, emptyAfter);
        SpawnBasis(size, mapCellsContainer.BasisCellsContainer, emptyBefore, emptyAfter);

        return size;
    }

    private void SpawnBasis(Vector3Int size, BasisCellsContainer basisCellsContainer, bool emptyBefore, bool emptyAfter)
    {
        for (int x = 0; x < size.x; x++)
        {
            float xPosition = -size.x / 2f + x;

            if (x == 0 || x == size.x - 1)
            {
                SpawnLine(xPosition, size.z, x == 0, basisCellsContainer.EdgeCorner, basisCellsContainer.EdgeMiddle, emptyBefore, emptyAfter);
            }
            else
            {
                SpawnLine(xPosition, size.z, true, basisCellsContainer.Corner, basisCellsContainer.Middle, emptyBefore, emptyAfter);
            }
        }
    }
}
