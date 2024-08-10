using System;
using UnityEngine;

[Serializable]
public class GadgetChunkInfo
{
    [SerializeField] private ChunkType _chunkType;
    [SerializeField] private ChunkType _powerUsing;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _speedMultiplierOnDelay;
    [SerializeField] private bool _isAccelerates;
    [SerializeField] private string _animationTriggerName;

    public GadgetChunkInfo(ChunkType chunkType)
    {
        _chunkType = chunkType;
        _isAccelerates = true;
        _animationTriggerName = chunkType.ToString();
    }

    public ChunkType ChunkType => _chunkType;
    public ChunkType PowerUsing => _powerUsing;
    public float StartDelay => _startDelay;
    public float SpeedMultiplierOnDelay => _speedMultiplierOnDelay;
    public bool IsAccelerates => _isAccelerates;
    public string AnimationTriggerName => _animationTriggerName;
}
