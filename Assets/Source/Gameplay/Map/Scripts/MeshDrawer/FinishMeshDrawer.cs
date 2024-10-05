using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(BasisMeshDrawer))]
public class FinishMeshDrawer : ChunkMeshDrawer
{
    private BasisMeshDrawer _basisMeshDrawer;
    private List<FinishConfetti> _finishParticles = new();

    public void ActiveFinishParticles()
    {
        if (_finishParticles == null || _finishParticles.Count == 0)
        {
            return;
        }

        foreach (FinishConfetti finishConfetti in _finishParticles)
        {
            finishConfetti.gameObject.SetActive(true);
        }
    }

    public override Vector3Int SpawnMesh(Vector3Int size, MapCellsContainer mapCellsContainer, bool emptyBefore, bool emptyAfter)
    {
        _basisMeshDrawer.SpawnMesh(size, mapCellsContainer, emptyBefore, emptyAfter);
        size = base.SpawnMesh(size, mapCellsContainer, emptyBefore, emptyAfter);
        SpawnGround(size, mapCellsContainer.MarginCellsContainer, emptyBefore, emptyAfter);
        SpawnFinish(size, mapCellsContainer.FinishCellsContainer, emptyBefore, emptyAfter);

        return size;
    }

    private void Awake()
    {
        _basisMeshDrawer = GetComponent<BasisMeshDrawer>();
    }

    private void SpawnFinish(Vector3Int size, FinishCellsContainer finishCellsContainer, bool emptyBefore, bool emptyAfter)
    {
        Vector3 sizeVector = Vector3.one;
        int min = GlobalSettings.Instance.ChunkMargin;
        int max = size.x - 1 - min;
        _finishParticles.Clear();
        GameObject finishCell = null;

        for (int x = 0; x < size.x; x++)
        {
            float xPosition = -size.x / 2f + x;

            if (x >= min && x <= max)
            {
                if (x == min)
                {
                    finishCell = SpawnCell(finishCellsContainer.LeftLine, new Vector3(xPosition, 0, 0), sizeVector, emptyBefore, emptyAfter);
                }
                else if (x == max)
                {
                    finishCell = SpawnCell(finishCellsContainer.RightLine, new Vector3(xPosition, 0, 0), sizeVector, emptyBefore, emptyAfter);
                }
                else
                {
                    finishCell = SpawnCell(finishCellsContainer.MiddleLine, new Vector3(xPosition, 0, 0), sizeVector, emptyBefore, emptyAfter);
                }
            }

            TryAddFinishParticles(finishCell);

            finishCell = SpawnCell(finishCellsContainer.Middle, new Vector3(xPosition, 0, 1), sizeVector, emptyBefore, emptyAfter);

            TryAddFinishParticles(finishCell);
        }

        if (finishCellsContainer.FinishObject != null)
        {
            SpawnCell(finishCellsContainer.FinishObject, Vector3.zero, Vector3.one, emptyBefore, emptyAfter);
        }
    }

    private bool TryAddFinishParticles(GameObject finishCell)
    {
        if (finishCell == null)
        {
            return false;
        }

        if (finishCell.GetComponentInChildren<FinishConfetti>(includeInactive: true) is FinishConfetti finishConfetti)
        {
            _finishParticles.Add(finishConfetti);
            return true;
        }

        return false;
    }

    private void SpawnGround(Vector3Int size, MarginCellsContainer marginCellsContainer, bool emptyBefore, bool emptyAfter)
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
                    null,
                    marginCellsContainer.RoadCornerMargin,
                    marginCellsContainer.MiddleMiddleMargin,
                    marginCellsContainer.StartMiddleMargin, emptyBefore, emptyAfter);
                }
                else if (x == max)
                {
                    SpawnLine(xPosition, size.z, mirror: true,
                    null,
                    marginCellsContainer.RoadCornerMargin,
                    marginCellsContainer.MiddleMiddleMargin,
                    marginCellsContainer.StartMiddleMargin, emptyBefore, emptyAfter);
                }
                else
                {
                    SpawnLine(xPosition, size.z, mirror: false,
                    null,
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
