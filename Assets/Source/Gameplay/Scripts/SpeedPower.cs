using System;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public struct SpeedPower
{
	[SerializeField] private Transform _left;
	[SerializeField] private Transform _right;
	[SerializeField] private float _speedMultiplier;

	public readonly Vector3 LeftPosition => _left.position;
	public readonly Vector3 RightPosition => _right.position;
	public readonly float SpeedMultiplier => _speedMultiplier;

	public static float GetRotationZ(Vector3 pointPosition, Vector3 arrowPosition)
	{
		Vector2 direction = pointPosition - arrowPosition;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		return angle;
	}

	public readonly bool IsBetween(float rotationZ, Vector3 arrowPosition)
	{
		float leftRotation = GetRotationZ(LeftPosition, arrowPosition);
		float rightRotation = GetRotationZ(RightPosition, arrowPosition);

		return rotationZ <= leftRotation && rotationZ >= rightRotation;
	}
}