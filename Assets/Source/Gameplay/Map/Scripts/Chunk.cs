using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(EnvironmentSpawner))]
public abstract class Chunk : MonoBehaviour
{
	[SerializeField] private ChunkMeshDrawer _chunkMeshDrawer;
	[SerializeField] private ChunkType _chunkType;
	[SerializeField] private Transform _movePoints;
	[SerializeField] private Transform _endPoint;
	[SerializeField] private Transform _changeAnimationPoint;
	[Header("Debug")]
    [SerializeField] private MapCellsContainer _mapCellsContainer;
	[SerializeField] private Vector3Int _testSize;

	private Vector3Int _chunkSize;
	private GlobalSettings _globalSettings;
	private EnvironmentSpawner _environmentSpawner;

	public ChunkType Type => _chunkType;
	public Vector3Int Size => _chunkSize;
	public Transform EndPoint => _endPoint;
	public List<Transform> MovePoints => GetMovePoints();
	public Transform ChangeAnimationPoint => _changeAnimationPoint;

	private GlobalSettings GlobalSettings
	{
		get
		{
			if (_globalSettings == null)
			{
				_globalSettings = GlobalSettings.Instance;
			}

			return _globalSettings;
		}
	}

	[ContextMenu("Set Chunk Size")]
	public void SetChunkSize()
	{
		SetupChunk(_testSize, _mapCellsContainer);
		
#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(this);
#endif
	}

	public void SetupChunk(Vector3Int size, MapCellsContainer mapCellsContainer, bool emptyBefore = false, bool emptyAfter = false)
	{
		_chunkMeshDrawer?.SpawnMesh(size, mapCellsContainer, emptyBefore, emptyAfter);
		_environmentSpawner.SpawnEnvironment(size, _chunkType, mapCellsContainer);

		size *= GlobalSettings.RoadsOffset;
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

		if (width < GlobalSettings.MinRoadsCount)
		{
			width = GlobalSettings.MinRoadsCount;
		}

		return width;
	}

	private void Awake()
	{
		_environmentSpawner = GetComponent<EnvironmentSpawner>();

		if (_changeAnimationPoint == null)
		{
			_changeAnimationPoint = _movePoints.GetChild(_movePoints.childCount - 1);
		}
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
