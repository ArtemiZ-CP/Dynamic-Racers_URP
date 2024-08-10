using System.Collections.Generic;
using UnityEngine;

public class WallChunk : Chunk
{
	[SerializeField] private int _minChunkHeight;
	[SerializeField] private float _heightOffset;
	[Header("Move Points")]
	[SerializeField] private Transform _startMovePoint;
	[SerializeField] private List<Transform> _startMovePoints;
	[SerializeField] private Transform _endMovePoint;

	public override int SetChunkLength(int length)
	{
		return 1;
	}

	public override int SetChunkHeight(int height)
	{
		if (height < _minChunkHeight)
		{
			height = _minChunkHeight;
		}

		height = base.SetChunkHeight(height);

		if (_startMovePoint != null)
		{
			Vector3 startMovePointPosition = new(0, _startMovePoint.position.y, transform.position.z);

			if (_startMovePoints != null && _startMovePoints.Count > 0)
			{
				foreach (Transform startMovePoint in _startMovePoints)
				{
					Vector3 delta = startMovePoint.position - _startMovePoint.position;
					startMovePoint.position = delta + startMovePointPosition;
				}
			}

			_startMovePoint.position = startMovePointPosition;
		}

		if (_endMovePoint != null)
		{
			Vector3 endMovePointPosition = new(0, height + transform.position.y + _heightOffset, _endMovePoint.position.z);

			_endMovePoint.position = endMovePointPosition;
		}

		return _minChunkHeight;
	}

	public override int SetChunkWidth(int width)
	{
		width = base.SetChunkWidth(width);

		return width;
	}
}
