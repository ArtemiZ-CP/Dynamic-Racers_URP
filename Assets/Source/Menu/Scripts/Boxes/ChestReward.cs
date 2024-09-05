using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChestReward
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

    [SerializeField] private List<GadgetReward> _gadgetRewards;
    [SerializeField] private List<CharacteristicReward> _characteristicRewards;
    [SerializeField] private int _coinsReward;

    public List<GadgetReward> GadgetRewards => _gadgetRewards;
    public List<CharacteristicReward> CharacteristicRewards => _characteristicRewards;
    public int CoinsReward => _coinsReward;

    public ChestReward(ChestType chestType)
    {
        AddChest(chestType);
    }
    
    public ChestReward(List<GadgetReward> gadgetRewards, List<CharacteristicReward> characteristicRewards, int coinsReward)
    {
        _gadgetRewards = new List<GadgetReward>(gadgetRewards);
        _characteristicRewards = new List<CharacteristicReward>(characteristicRewards);
        _coinsReward = coinsReward;
    }

    private void AddChest(ChestType chestType)
    {
        GlobalSettings globalSettings = GlobalSettings.Instance;
        
        List<Gadget> gadgets = globalSettings.GetAllGadgets();

        int randomIndex = UnityEngine.Random.Range(0, gadgets.Count);
        int amount = (int)Mathf.Pow(2, (int)chestType);
        GadgetReward reward = new(gadgets[randomIndex].ScriptableObject, amount);
        _gadgetRewards = new List<GadgetReward>() { reward };
    }
}