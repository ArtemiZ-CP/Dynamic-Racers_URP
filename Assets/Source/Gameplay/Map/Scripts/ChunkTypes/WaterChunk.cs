using System.Collections.Generic;
using UnityEngine;

public class WaterChunk : Chunk
{
	[Header("Move Points")]
	[SerializeField] private Transform _startMovePoint;
	[SerializeField] private List<Transform> _startMovePoints;
	[SerializeField] private Transform _endMovePoint;
	[SerializeField] private List<Transform> _endMovePoints;

	public override int SetChunkLength(int length)
	{
		length = base.SetChunkLength(length);

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
			Vector3 endMovePointPosition = new(0, _endMovePoint.position.y, length + transform.position.z);

			if (_endMovePoints != null && _endMovePoints.Count > 0)
			{
				foreach (Transform endMovePoint in _endMovePoints)
				{
					Vector3 delta = endMovePoint.position - _endMovePoint.position;
					endMovePoint.position = delta + endMovePointPosition;
				}
			}

			_endMovePoint.position = endMovePointPosition;
		}

		return length;
	}

	public override int SetChunkHeight(int height)
	{
		return 0;
	}

	public override int SetChunkWidth(int width)
	{
		width = base.SetChunkWidth(width);

		return width;
	}
}
