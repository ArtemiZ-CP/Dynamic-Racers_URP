using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "Settings/Global", order = 1)]
public class GlobalSettings : ScriptableObject
{
	private static GlobalSettings _instance;

    [SerializeField, Min(1)] private int _XPToLevelUp;
	[SerializeField] private int _gameplayPlayersCount; 
	[SerializeField] private int _maxTickets;
	[Header("Training")]
	[SerializeField] private int _trainingLevelsCount; 
	[SerializeField] private int _trainingPlayersCount; 
	[Header("Gadgets")]
	[SerializeField] private List<int> _gadgetsLevelProgression;
	[SerializeField] private List<GadgetScriptableObject> _allGadgets;
	[Header("Character Speed")]
	[SerializeField, Min(0)] private float _baseSpeed = 1;
	[SerializeField, Min(0)] private float _additionalSpeedByUpgrade = 1;
	[Header("Before Start")]
	[SerializeField, Min(0)] private float _timeToStartRun = 5;
	[SerializeField] private float _distanceToStartPower;
	[SerializeField] private float _characterStartOffset;
	[SerializeField] private AnimationCurve _startPowerCurve;
	[Header("Map settings")]
	[SerializeField, Range(0, 90)] private float _fallAngle = 45;
	[SerializeField, Min(1)] private int _chunkMargin = 1;
	[SerializeField, Min(0)] private int _additionalRoadWidht = 1;
	[SerializeField, Min(1)] private int _roadsOffset = 1;
	[SerializeField, Min(1)] private int _minRoadLength = 1;
	[SerializeField, Min(1)] private int _minRoadsCount = 1;
	
	public int XPToLevelUp => _XPToLevelUp;
	public int GameplayPlayersCount => _gameplayPlayersCount;
	public int MaxTickets => _maxTickets;
	public int TrainingLevelsCount => _trainingLevelsCount;
	public int TrainingPlayersCount => _trainingPlayersCount;
	public IReadOnlyList<int> GadgetsLevelProgression => _gadgetsLevelProgression;
	public IReadOnlyList<GadgetScriptableObject> AllGadgets => _allGadgets;
	public float FallAngle => _fallAngle;
	public float BaseSpeed => _baseSpeed;
	public float AdditionalSpeedByUpgrade => _additionalSpeedByUpgrade;
	public float TimeToStartRun => _timeToStartRun;
	public float DistanceToStartPower => _distanceToStartPower;
	public float CharacterStartOffset => _characterStartOffset;
	public AnimationCurve StartPowerCurve => _startPowerCurve;
	public int ChunkMargin => _chunkMargin;
	public int AdditionalRoadWidht => _additionalRoadWidht;
	public int RoadsOffset => _roadsOffset;
	public int MinRoadLength => _minRoadLength;
	public int MinRoadsCount => _minRoadsCount;

	public static GlobalSettings Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Resources.Load<GlobalSettings>(nameof(GlobalSettings));
			}

			return _instance;
		}
	}
}
