using System;
using UnityEngine;

[Serializable]
public struct MoveData
{
    [SerializeField] private Transform _transform;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isMoveImmediately;

    public readonly Transform Transform => _transform;
    public readonly float Speed => _speed;
    public readonly bool IsMoveImmediately => _isMoveImmediately;
}