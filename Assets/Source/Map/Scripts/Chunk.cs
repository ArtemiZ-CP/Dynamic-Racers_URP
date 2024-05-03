using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public abstract class Chunk : MonoBehaviour
{
	[SerializeField] private ChunkMeshDrawer _chunkMeshSpawner;
	[SerializeField] private ChunkType _chunkType;
	[SerializeField] private Transform _movePoints;
	[SerializeField] private Transform _endPoint;
	[Header("Test Resize")]
	[SerializeField] private Vector3Int _testSize;

	private Vector3Int _chunkSize;

	public ChunkType Type => _chunkType;
	public Vector3Int Size => _chunkSize;
	public Transform EndPoint => _endPoint;
	public List<Transform> MovePoints => GetMovePoints();

	[ContextMenu("Set Chunk Size")]
	public void SetChunkSize()
	{
		SetChunkSize(_testSize);
	}

	public void SetChunkSize(Vector3Int size)
	{
		_chunkMeshSpawner?.SpawnMesh(size);

		size *= GlobalSettings.Instance.RoadsOffset;
		SetBasePosition();
		SetChunkWidth(size.x);
		SetChunkHeight(size.y);
		SetChunkLength(size.z);
		_chunkSize = size;
	}

	public virtual int SetChunkLength(int length)
	{
		if (_endPoint == null)
		{
			return length;
		}

		_endPoint.position = new Vector3(0, _endPoint.position.y, length + transform.position.z);

		return length;
	}

	public virtual int SetChunkHeight(int height)
	{
		if (_endPoint == null)
		{
			return height;
		}

		_endPoint.position = new Vector3(0, height + transform.position.y, _endPoint.position.z);

		return height;
	}

	public virtual int SetChunkWidth(int width)
	{
		if (_endPoint == null)
		{
			return width;
		}

		if (width < GlobalSettings.Instance.MinRoadsCount)
		{
			width = GlobalSettings.Instance.MinRoadsCount;
		}

		return width;
	}

	private List<Transform> GetMovePoints()
	{
		List<Transform> movePoints = new();

		foreach (Transform movePoint in _movePoints)
		{
			movePoints.Add(movePoint);
		}

		return movePoints;
	}

	private void SetBasePosition()
	{
		transform.position = new Vector3(0, transform.position.y, transform.position.z);
	}
}
