using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "Settings/Global", order = 1)]
public class GlobalSettings : ScriptableObject
{
	public static readonly int ChunksTypeCount = 5;

	private static GlobalSettings _instance;

    [SerializeField, Min(1)] private int _XPToLevelUp;
	[SerializeField] private int _gameplayPlayersCount; 
	[SerializeField] private int _maxTickets;
	[SerializeField] private int _minFPS = 30;
	[SerializeField] private int _maxFPS = 30;
	[Header("Shop")]
	[SerializeField] private BoxReward.ChestSprite[] _boxSprites;
	[Header("Training")]
	[SerializeField] private int _trainingLevelsCount; 
	[SerializeField] private int _trainingPlayersCount; 
	[Header("Gadgets")]
	[SerializeField] private int[] _gadgetsLevelProgression;
	[SerializeField] private GadgetScriptableObject[] _allGadgets;
	[SerializeField] private Sprite[] _gadgetRareBackgrounds;
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
	public int MinFPS => _minFPS;
	public int MaxFPS => _maxFPS;
	public int TrainingLevelsCount => _trainingLevelsCount;
	public int TrainingPlayersCount => _trainingPlayersCount;
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

	public List<Gadget> GetAllGadgets()
	{
		return _allGadgets.Select(gadget => new Gadget(gadget)).ToList();
	}

	public List<Gadget> GetNotFoundGadgets()
	{
		List<Gadget> foundGadgets = PlayerData.PlayerGadgets.ToList();
		List<Gadget> notFoundGadgets = _allGadgets.Where(gadget => 
			!foundGadgets.Any(foundGadget => foundGadget.GadgetScriptableObject == gadget)).
			Select(gadget => new Gadget(gadget)).ToList();

		return notFoundGadgets;
	}

	public Sprite GetGadgetRareBackground(Rare rare)
	{
		return _gadgetRareBackgrounds[(int) rare];
	}

	public GadgetScriptableObject GetRandomGadget()
	{
		return _allGadgets[UnityEngine.Random.Range(0, _allGadgets.Length)];
	}

	public GadgetScriptableObject GetRandomGadget(Rare rare)
	{
		List<GadgetScriptableObject> gadgets = _allGadgets.Where(g => 
			g.Rare == rare).ToList();

		if (gadgets.Count == 0)
		{
			return null;
		}

		return gadgets[UnityEngine.Random.Range(0, gadgets.Count)];
	}

	public bool TryGetGadgetsLevelProgression(int gadgetLevel, out int gadgetsToLevelUp)
	{
		if (gadgetLevel < _gadgetsLevelProgression.Length)
		{
			gadgetsToLevelUp = _gadgetsLevelProgression[gadgetLevel];
			return true;
		}

		gadgetsToLevelUp = 0;
		return false;
	}

	public Sprite GetChestSprite(BoxReward.ChestType chestType)
	{
		foreach (BoxReward.ChestSprite chestSprite in _boxSprites)
		{
			if (chestSprite.ChestType == chestType)
			{
				return chestSprite.Sprite;
			}
		}

		return null;
	}
}
