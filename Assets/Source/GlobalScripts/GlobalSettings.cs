using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "GlobalSettings/Global", order = 1)]
public class GlobalSettings : ScriptableObject
{
	public static readonly int ChunksTypeCount = 5;

	private static GlobalSettings _instance;

	[Serializable]
	private class ChestOpeningInfo
	{
		[SerializeField] private ChestReward.ChestType _type;
		[SerializeField] private int _openingTimeInSeconds;
		[SerializeField] private int _openingTimeInMinuts;
		[SerializeField] private int _openingTimeInHours;

		public ChestReward.ChestType Type => _type;
		public TimeSpan GetOpeningTime()
		{
			return new TimeSpan(0, _openingTimeInHours, _openingTimeInMinuts, _openingTimeInSeconds, 0);
		}
	}

	[SerializeField, Min(1)] private int _XPToLevelUp;
	[SerializeField] private int _gameplayPlayersCount;
	[SerializeField] private int _maxTickets;
	[SerializeField] private int _minFPS = 30;
	[SerializeField] private int _maxFPS = 30;
	[Header("Opening Chests")]
	[SerializeField] private List<ChestOpeningInfo> _openingChests = new();
	[Header("Sprites")]
	[SerializeField] private ChestReward.ChestSprite[] _boxSprites;
	[SerializeField] private Sprite[] _characteristicRareBackgrounds;
	[SerializeField] private Sprite _coinsSprite;
	[SerializeField] private Sprite _diamondsSprite;
	[Header("Bioms")]
    [SerializeField] private Biom[] _bioms;
	[SerializeField] private int[] _starsRewards;
	[Header("Training")]
	[SerializeField] private int _trainingLevelsCount;
	[SerializeField] private GadgetReward _trainingGadgetReward;
	[SerializeField] private List<GameObject> _skins;
	[Header("Character Speed")]
	[SerializeField, Min(0)] private float _baseSpeed = 1;
	[SerializeField, Min(0)] private float _additionalSpeedByUpgrade = 1;
	[SerializeField] private float _reduseUpgradesByLevel = 1;
	[Header("Before Start")]
	[SerializeField, Min(0)] private float _timeToStartRun = 5;
	[SerializeField] private float _distanceToStartPower;
	[SerializeField] private float _characterStartOffset;
	[SerializeField] private float _playerOffsetSmoothing;
	[SerializeField] private float _dragOffset;
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
	public GadgetReward TrainingGadgetReward => _trainingGadgetReward;
	public IReadOnlyCollection<Biom> Bioms => _bioms;
	public IReadOnlyCollection<int> StarsRewards => _starsRewards;
	public float FallAngle => _fallAngle;
	public float BaseSpeed => _baseSpeed;
	public float AdditionalSpeedByUpgrade => _additionalSpeedByUpgrade;
	public float ReduseUpgradesByLevel => _reduseUpgradesByLevel;
	public float TimeToStartRun => _timeToStartRun;
	public float DistanceToStartPower => _distanceToStartPower;
	public float CharacterStartOffset => _characterStartOffset;
	public float PlayerOffsetSmoothing => _playerOffsetSmoothing;
	public float DragOffset => _dragOffset;
	public AnimationCurve StartPowerCurve => _startPowerCurve;
	public int ChunkMargin => _chunkMargin;
	public int AdditionalRoadWidht => _additionalRoadWidht;
	public int RoadsOffset => _roadsOffset;
	public int MinRoadLength => _minRoadLength;
	public int MinRoadsCount => _minRoadsCount;
	public IReadOnlyList<GameObject> Skins => _skins;
	public Sprite CoinsSprite => _coinsSprite;
	public Sprite DiamondsSprite => _diamondsSprite;

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

	public GameObject GetRandomSkin()
	{
		return _skins[UnityEngine.Random.Range(0, _skins.Count)];
	}

	public TimeSpan GetOpeningTime(ChestReward.ChestType chestType)
	{
		ChestOpeningInfo chestOpeningInfo = _openingChests.Find(info => info.Type == chestType);

		if (chestOpeningInfo == null)
		{
			return TimeSpan.Zero;
		}

		return chestOpeningInfo.GetOpeningTime();
	}

	public Sprite GetCharacteristicRareBackground(Rare rare)
	{
		return _characteristicRareBackgrounds[(int)rare - (int)Rare.Common];
	}

	public Sprite GetChestSprite(ChestReward.ChestType chestType)
	{
		foreach (ChestReward.ChestSprite chestSprite in _boxSprites)
		{
			if (chestSprite.ChestType == chestType)
			{
				return chestSprite.Sprite;
			}
		}

		return null;
	}
}
