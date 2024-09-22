using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChestReward : Reward
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
        Wood = 0,
        Small,
        Medium,
        Big,
        Legendary
    }

    [SerializeField] private List<GadgetReward> _gadgetRewards = new();
    [SerializeField] private List<CharacteristicReward> _characteristicRewards = new();
    [SerializeField] private CoinsReward _coinsReward;
    [SerializeField] private DiamondsReward _diamondsReward;

    private ChestType _chestType;

    public ChestType Type => _chestType;
    public List<GadgetReward> GadgetRewards => _gadgetRewards;
    public List<CharacteristicReward> CharacteristicRewards => _characteristicRewards;
    public CoinsReward CoinsReward => _coinsReward;
    public DiamondsReward DiamondsReward => _diamondsReward;

    public ChestReward(ChestType chestType)
    {
        _chestType = chestType;
        SetRewards(chestType);
    }

    public ChestReward(ChestType chestType,
        List<GadgetReward> gadgetRewards, List<CharacteristicReward> characteristicRewards,
        CoinsReward coinsReward, DiamondsReward diamondsReward)
    {
        _chestType = chestType;
        if (gadgetRewards != null) _gadgetRewards = new List<GadgetReward>(gadgetRewards);
        if (characteristicRewards != null) _characteristicRewards = new List<CharacteristicReward>(characteristicRewards);
        if (coinsReward != null) _coinsReward = coinsReward;
        if (diamondsReward != null) _diamondsReward = diamondsReward;
    }

    public ChestReward(ChestSaveInfo chestSaveInfo)
    {
        _chestType = (ChestType)chestSaveInfo.ChestTypeInt;

        if (chestSaveInfo.GadgetRewards != null)
        {
            _gadgetRewards = new List<GadgetReward>();
            
            foreach (GadgetSaveInfo gadgetSaveInfo in chestSaveInfo.GadgetRewards)
            {
                _gadgetRewards.Add(new GadgetReward(gadgetSaveInfo));
            }
        }

        if (chestSaveInfo.CharacteristicRewards != null)
        {
            _characteristicRewards = new List<CharacteristicReward>();
            
            foreach (CharacteristicSaveInfo characteristicSaveInfo in chestSaveInfo.CharacteristicRewards)
            {
                _characteristicRewards.Add(new CharacteristicReward(characteristicSaveInfo));
            }
        }

        if (chestSaveInfo.CoinsReward != null && chestSaveInfo.CoinsReward.Amount > 0)
        {
            _coinsReward = new CoinsReward(chestSaveInfo.CoinsReward);
        }

        if (chestSaveInfo.DiamondsReward != null && chestSaveInfo.DiamondsReward.Amount > 0)
        {
            _diamondsReward = new DiamondsReward(chestSaveInfo.DiamondsReward);
        }
    }

    public override void ApplyReward()
    {
        if (_coinsReward != null)
        {
            _coinsReward.ApplyReward();
        }

        if (_diamondsReward != null)
        {
            _diamondsReward.ApplyReward();
        }

        if (_gadgetRewards.Count > 0)
        {
            foreach (var reward in _gadgetRewards)
            {
                reward.ApplyReward();
            }
        }

        if (_characteristicRewards.Count > 0)
        {
            foreach (var reward in _characteristicRewards)
            {
                reward.ApplyReward();
            }
        }
    }

    public override Reward[] GetSimpleRewards()
    {
        List<Reward> rewards = new();

        if (_gadgetRewards.Count > 0)
        {
            rewards.AddRange(_gadgetRewards);
        }

        if (_characteristicRewards.Count > 0)
        {
            rewards.AddRange(_characteristicRewards);
        }

        if (_coinsReward != null && _coinsReward.Amount > 0)
        {
            rewards.Add(_coinsReward);
        }

        if (_diamondsReward != null && _diamondsReward.Amount > 0)
        {
            rewards.Add(_diamondsReward);
        }

        return rewards.ToArray();
    }

    private void SetRewards(ChestType chestType)
    {
        SetGadgetReward(chestType);
        SetCharacteristicReward(chestType);
        SetDiamondsReward(chestType);
        SetCoinsReward(chestType);
    }

    private void SetGadgetReward(ChestType chestType)
    {
        List<Gadget> gadgets = GlobalSettings.Instance.GetAllGadgets();

        int randomIndex = UnityEngine.Random.Range(0, gadgets.Count);
        int amount = (int)Mathf.Pow(3, (int)chestType + 1);
        GadgetReward reward = new(gadgets[randomIndex].ScriptableObject, amount);
        _gadgetRewards = new List<GadgetReward>() { reward };
    }

    private void SetCharacteristicReward(ChestType chestType)
    {
        int randomIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(CharacteristicType)).Length);
        int amount = (int)chestType + 1;
        CharacteristicReward reward = new((CharacteristicType)randomIndex, amount);
        _characteristicRewards = new List<CharacteristicReward>() { reward };
    }

    private void SetCoinsReward(ChestType chestType)
    {
        int amount = (int)Mathf.Pow(10, (int)chestType + 1);
        _coinsReward = new CoinsReward(amount);
    }

    private void SetDiamondsReward(ChestType chestType)
    {
        int amount = (int)Mathf.Pow(2, (int)chestType + 1);
        _diamondsReward = new DiamondsReward(amount);
    }
}