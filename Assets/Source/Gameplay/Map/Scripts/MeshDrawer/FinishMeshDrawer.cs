using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(BasisMeshDrawer))]
public class FinishMeshDrawer : ChunkMeshDrawer
{
    private BasisMeshDrawer _basisMeshDrawer;

    public override Vector3Int SpawnMesh(Vector3Int size, MapCellsContainer mapCellsContainer, bool emptyBefore, bool emptyAfter)
    {
        _basisMeshDrawer.SpawnMesh(size, mapCellsContainer, emptyBefore, emptyAfter);
        size = base.SpawnMesh(size, mapCellsContainer, emptyBefore, emptyAfter);
        SpawnGround(size, mapCellsContainer.GroundCellsContainer, mapCellsContainer.MarginCellsContainer, emptyBefore, emptyAfter);
        SpawnFinish(mapCellsContainer.FinishPrefab);

        return size;
    }

    private void Awake()
    {
        _basisMeshDrawer = GetComponent<BasisMeshDrawer>();
    }

    private void SpawnFinish(GameObject finishObject)
    {
        Instantiate(finishObject, MeshParent);
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
                    marginCellsContainer.RoadCornerMargin,
                    marginCellsContainer.MiddleMiddleMargin,
                    marginCellsContainer.StartMiddleMargin, emptyBefore, emptyAfter);
                }
                else if (x == max)
                {
                    SpawnLine(xPosition, size.z, mirror: true,
                    groundCellsContainer.StartSide,
                    marginCellsContainer.RoadCornerMargin,
                    marginCellsContainer.MiddleMiddleMargin,
                    marginCellsContainer.StartMiddleMargin, emptyBefore, emptyAfter);
                }
                else
                {
                    SpawnLine(xPosition, size.z, mirror: false,
                    groundCellsContainer.StartMiddle,
                    marginCellsContainer.SecondMiddleMargin,
                    marginCellsContainer.MiddleMiddleMargin,
                    marginCellsContainer.StartMiddleMargin, emptyBefore, emptyAfter);
                }
            }
        }
    }

    private void SpawnMarginLine(float xPosition, int lenght, MarginsOffset marginsOffset, bool mirror, MarginCellsContainer marginCellsContainer, bool emptyBefore, bool emptyAfter)
    {
        GameObject start = null;
        GameObject second = null;
        GameObject middle = null;
        GameObject end = null;

        if (marginsOffset == MarginsOffset.Road)
        {
            start = marginCellsContainer.StartRoadMargin;
            second = marginCellsContainer.SecondRoadMargin;
            middle = marginCellsContainer.MiddleMiddleMargin;
            end = marginCellsContainer.StartMiddleMargin;
        }
        else if (marginsOffset == MarginsOffset.Middle)
        {
            start = marginCellsContainer.StartMiddleMargin;
            second = marginCellsContainer.MiddleMiddleMargin;
            middle = marginCellsContainer.MiddleMiddleMargin;
        }
        else if (marginsOffset == MarginsOffset.Side)
        {
            start = marginCellsContainer.StartSideMargin;
            second = marginCellsContainer.MiddleSideMargin;
            middle = marginCellsContainer.MiddleSideMargin;
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
                SpawnCell(start, new Vector3(xPosition, 0, z), size, emptyBefore, emptyAfter);
            }
            else if (z == 1)
            {
                SpawnCell(second, new Vector3(xPosition, 0, z), size, emptyBefore, emptyAfter);
            }
            else if (z == lenght - 1)
            {
                size.z *= -1;

                if (marginsOffset == MarginsOffset.Road)
                {
                    SpawnCell(end, new Vector3(xPosition, 0, z), size, emptyBefore, emptyAfter);
                }
                else
                {
                    SpawnCell(start, new Vector3(xPosition, 0, z), size, emptyBefore, emptyAfter);
                }

                size.z *= -1;
            }
            else
            {
                SpawnCell(middle, new Vector3(xPosition, 0, z), size, emptyBefore, emptyAfter);
            }
        }
    }
}
