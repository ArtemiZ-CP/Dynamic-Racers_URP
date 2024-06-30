using System;
using UnityEngine;

[Serializable]
public class GadgetReward : Reward
{
    [SerializeField] private GadgetScriptableObject _gadget;
    [SerializeField] private int _count;

    public GadgetScriptableObject Gadget => _gadget;
    public int Count => _count;

    public GadgetReward(GadgetScriptableObject gadget, int count)
    {
        _gadget = gadget;
        _count = count;
    }
}
