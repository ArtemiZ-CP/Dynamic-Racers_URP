using System;
using UnityEngine;

[Serializable]
public class GadgetAnimationInfo
{
    [SerializeField] private ChunkType _chunkType;
    [SerializeField] private string _animationTrigger;
    [SerializeField] private GameObject _prefab;

    public GadgetAnimationInfo(ChunkType chunkType)
    {
        _chunkType = chunkType;
        _animationTrigger = chunkType.ToString();
    }

    public ChunkType ChunkType => _chunkType;
    public string AnimationTrigger => _animationTrigger;
    public GameObject Prefab => _prefab;
}
