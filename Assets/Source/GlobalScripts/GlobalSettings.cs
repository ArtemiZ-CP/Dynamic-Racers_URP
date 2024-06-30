using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "Settings/Global", order = 1)]
public class GlobalSettings : ScriptableObject
{
	private static GlobalSettings _instance;

	[Header("Character Speed")]
	[SerializeField, Min(0)] private float _baseSpeed = 1;
	[SerializeField, Min(0)] private float _additionalSpeedByUpgrade = 1;
	[Header("Before Start")]
	[SerializeField, Min(0)] private float _timeToStartRun = 5;
	[SerializeField] private float _distanceToStartPower;
	[SerializeField] private AnimationCurve _startPowerCurve;
	[Header("Map settings")]
	[SerializeField, Range(0, 90)] private float _fallAngle = 45;
	[SerializeField, Min(1)] private int _chunkMargin = 1;
	[SerializeField, Min(1)] private int _roadsOffset = 1;
	[SerializeField, Min(1)] private int _minRoadLength = 1;
	[SerializeField, Min(1)] private int _minRoadsCount = 1;
	[SerializeField, Min(1)] private int _halfBoardLength = 1; 

	public float FallAngle => _fallAngle;
	public float BaseSpeed => _baseSpeed;
	public float AdditionalSpeedByUpgrade => _additionalSpeedByUpgrade;
	public float TimeToStartRun => _timeToStartRun;
	public float DistanceToStartPower => _distanceToStartPower;
	public AnimationCurve StartPowerCurve => _startPowerCurve;
	public int ChunkMargin => _chunkMargin;
	public int RoadsOffset => _roadsOffset;
	public int MinRoadLength => _minRoadLength;
	public int MinRoadsCount => _minRoadsCount;
	public int HalfBoardLength => _halfBoardLength;

	public static GlobalSettings Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Resources.Load<GlobalSettings>("GlobalSettings");
			}

			return _instance;
		}
	}
}
