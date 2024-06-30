using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gadget", menuName = "Gadget")]
public class GadgetScriptableObject : ScriptableObject
{
	[Header("Settings")]
	[SerializeField] private List<GadgetAnimationInfo> _gadgetAnimationInfos = new();
	[SerializeField] private float _speedMultiplier;
	[SerializeField] private float _distanceToDisactive;
	[SerializeField] private bool _isInfiniteDistance;
	[SerializeField, Min(1)] private int _activeCount;
	[SerializeField] private bool _isInfiniteActiveCounts;
	[Header("Visuals")]
	[SerializeField] private string _name;
	[SerializeField] private GameObject _prefab;
	[SerializeField] private Sprite _icon;
	[Header("Info")]
	[SerializeField] private string _description;

	public GameObject Prefab => _prefab;
	public float SpeedMultiplier => _speedMultiplier;
	public float DistanceToDisactive => _isInfiniteDistance ? float.MaxValue : _distanceToDisactive;
	public int ActiveCount => _isInfiniteActiveCounts ? int.MaxValue : _activeCount;
	public Sprite Icon => _icon;
	public string Description => _description;
	public string Name => _name;

	public GadgetAnimationInfo ContainsChunkType(ChunkType chunkType)
	{
		return _gadgetAnimationInfos.Find(x => x.ChunkType == chunkType);
	}
}