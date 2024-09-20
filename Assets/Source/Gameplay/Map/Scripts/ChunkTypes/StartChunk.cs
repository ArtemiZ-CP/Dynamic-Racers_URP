using UnityEngine;

public class StartChunk : Chunk
{
	[Header("Move Points")]
	[SerializeField] private Transform _startMovePoint;
	[SerializeField] private Transform _endMovePoint;

	public override int SetChunkLength(int length)
	{
		length = base.SetChunkLength(length);

		if (_endMovePoint != null)
		{
			_endMovePoint.position = new Vector3(0, _endMovePoint.position.y, length + transform.position.z);
		}

		return length;
	}

	public override int SetChunkHeight(int height)
	{
		base.SetChunkHeight(0);

		return 0;
	}

	public override int SetChunkWidth(int width)
	{
		width = base.SetChunkWidth(width);

		return width;
	}
}
