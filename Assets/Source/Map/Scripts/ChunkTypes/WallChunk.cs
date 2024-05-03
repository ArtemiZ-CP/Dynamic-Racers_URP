using UnityEngine;

public class WallChunk : Chunk
{
	[SerializeField] private int _minChunkHeight;
	[SerializeField] private float _heightOffset;
	[Header("Move Points")]
	[SerializeField] private Transform _startMovePoint;
	[SerializeField] private Transform _midlMovePoint;
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
			_startMovePoint.position = new Vector3(0, _startMovePoint.position.y, _startMovePoint.position.z);
		}

		if (_endMovePoint != null)
		{
			Vector3 endMovePointPosition = new(0, height + transform.position.y + _heightOffset, _endMovePoint.position.z);

			if (_midlMovePoint != null)
			{
				Vector3 delta = _midlMovePoint.position - _endMovePoint.position;
				_midlMovePoint.position = delta + endMovePointPosition;
			}

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
