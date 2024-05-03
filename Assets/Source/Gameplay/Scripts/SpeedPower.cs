using System;
using UnityEngine;

[Serializable]
public struct SpeedPower
{
	[SerializeField, Range(0, 1)] private float _minDeviation;
	[SerializeField] private float _speedMultiplier;

	public float MinDeviation => _minDeviation;
	public float SpeedMultiplier => _speedMultiplier;
}