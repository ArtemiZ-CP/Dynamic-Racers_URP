using UnityEngine;

public class FlyChunk : Chunk
{
	private const float _yOffset = 2.5f;

	[Header("Move Points")]
	[SerializeField] private Transform _startMovePoint;
	[SerializeField] private Transform _endMovePoint;

	public override int SetChunkLength(int length)
	{
		length = base.SetChunkLength(length);

		float fallAngle = GlobalSettings.Instance.FallAngle;
		int height = -(int)(length * Mathf.Tan(fallAngle * Mathf.Deg2Rad));

		if (_startMovePoint != null)
		{
			_startMovePoint.position = new Vector3(0, transform.position.y + _yOffset, transform.position.z);
		}

		if (_endMovePoint != null)
		{
			_endMovePoint.position = new Vector3(0, height + transform.position.y + _yOffset, length + transform.position.z);
		}
		
		base.SetChunkHeight(height);

		return length;
	}

	public override int SetChunkHeight(int height)
	{
		return Size.y;
	}
}
