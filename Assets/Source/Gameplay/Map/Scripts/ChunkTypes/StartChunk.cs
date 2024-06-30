using UnityEngine;

public class StartChunk : Chunk
{
	[Header("Move Points")]
	[SerializeField] private Transform _startMovePoint;

	public override int SetChunkLength(int length)
	{
		length = base.SetChunkLength(length);

		if (_startMovePoint != null)
		{
			_startMovePoint.position = new Vector3(0, _startMovePoint.position.y, length + transform.position.z);
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
