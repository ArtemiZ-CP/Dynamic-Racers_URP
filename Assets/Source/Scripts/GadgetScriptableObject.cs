using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gadget", menuName = "Gadget")]
[Serializable]
public class GadgetScriptableObject : ScriptableObject
{
	[Header("Settings")]
	[SerializeField] private GameObject _prefab;
	[SerializeField] private Group _group;
	[SerializeField] private Rare _rare;
	[SerializeField] private List<GadgetChunkInfo> _gadgetAnimationInfos = new();
	[SerializeField] private float _speedMultiplier;
	[SerializeField] private bool _isSpeedIncreasing;
	[SerializeField] private float _distanceToDisactive;
	[SerializeField] private bool _isInfiniteDistance;
	[SerializeField, Min(1)] private int _activeCount;
	[SerializeField] private bool _isInfiniteActiveCounts;
	[SerializeField] private bool _isUsageSplited;
	[Header("Visuals")]
	[SerializeField] private string _name;
	[SerializeField] private Sprite _sprite;
	[Header("Info")]
	[SerializeField] private string _description;

	public GameObject Prefab => _prefab;
	public Group Group => _group;
	public Rare Rare => _rare;
	public float SpeedMultiplier => _speedMultiplier;
	public bool IsSpeedIncreasing => _isSpeedIncreasing;
	public float DistanceToDisactive => _isInfiniteDistance ? float.MaxValue : _distanceToDisactive;
	public int UsageCount => _isInfiniteActiveCounts ? int.MaxValue : _activeCount;
	public bool IsUsageSplited => _isUsageSplited;
	public Sprite Sprite => _sprite;
	public string Description => _description;
	public string Name => _name;

	public GadgetChunkInfo GetChunkInfo(ChunkType chunkType)
	{
		return _gadgetAnimationInfos.Find(x => x.ChunkType == chunkType);
	}
}