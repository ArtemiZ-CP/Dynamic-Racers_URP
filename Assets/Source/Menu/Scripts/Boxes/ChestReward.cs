using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChestReward : RewardContainer
{
    [Serializable]
    public class ChestSprite
    {
        public ChestType ChestType;
        public Sprite Sprite;
    }

    [Serializable]
    public enum ChestType
    {
        Wood,
        Small,
        Medium,
        Big,
        Legendary
    }

    [SerializeField] private List<GadgetReward> _rewards;

    public List<GadgetReward> Rewards => _rewards;
    
    public ChestReward(ChestType chestType)
    {
        GlobalSettings globalSettings = GlobalSettings.Instance;
        
        List<Gadget> gadgets = globalSettings.GetAllGadgets();

        int randomIndex = UnityEngine.Random.Range(0, gadgets.Count);
        int amount = (int)Mathf.Pow(2, (int)chestType);
        GadgetReward reward = new(gadgets[randomIndex].ScriptableObject, amount);
        _rewards = new List<GadgetReward>() { reward };
    }

    public ChestReward(List<GadgetReward> rewards)
    {
        _rewards = new List<GadgetReward>(rewards);
    }
}