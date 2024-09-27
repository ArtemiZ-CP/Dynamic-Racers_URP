using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomReward", menuName = "BiomReward", order = 1)]
public class BiomReward : ScriptableObject
{
    [Serializable]
    public enum RewardType
    {
        Coins,
        Diamonds,
        Characteristics,
        Gadget,
        Chest
    }

    [Serializable]
    public class GadgetInfo
    {
        [SerializeField] private Rare _rare;
        [SerializeField, Min(0)] private float _chance;
        [SerializeField, Min(1)] private int _minAmount;
        [SerializeField, Min(1)] private int _maxAmount;
        [SerializeField, Min(1)] private int _amount;

        public Rare Rare => _rare;
        public float Chance => _chance;
        public int Amount => _amount;

        public GadgetReward GetReward()
        {
            int amount = UnityEngine.Random.Range(_minAmount, _maxAmount + 1) * _amount;

            return new GadgetReward(GadgetSettings.Instance.GetRandomGadget(_rare), amount);
        }
    }

    [SerializeField] private RewardType _rewardType;
    [SerializeField] private Rare _rare;
    [Header("Coins")]
    [SerializeField, Min(0)] private int _coinsCount;
    [Header("Diamonds")]
    [SerializeField, Min(0)] private int _diamondsCount;
    [Header("Characteristics")]
    [SerializeField, Min(0)] private int[] _characteristics;
    [Header("Gadgets")]
    [SerializeField] private GadgetInfo[] _gadgetInfos;
    [SerializeField] private Rare[] _garaunteedGadgetsByRarity;
    [SerializeField] private GadgetReward[] _garaunteedGadgets;
    [SerializeField, Min(0)] private int _gadgetCells;

    public ChestReward.ChestType ChestType => (ChestReward.ChestType)(int)_rare;
    public RewardType Type => _rewardType;
    public Rare Rare => _rare;
    public int CoinsCount => _coinsCount;
    public int DiamondsCount => _diamondsCount;
    public int[] Characteristics => _characteristics;
    public IReadOnlyCollection<GadgetInfo> GadgetInfos => _gadgetInfos;
    public IReadOnlyCollection<Rare> GaraunteedGadgetsByRarity => _garaunteedGadgetsByRarity;
    public IReadOnlyCollection<GadgetReward> GaraunteedGadgets => _garaunteedGadgets;
    public int GadgetCells => _gadgetCells;

    public Sprite GetRewardSprite()
    {
        return null;
    }

    public Reward[] GetRewards()
    {
        switch (_rewardType)
        {
            case RewardType.Coins:
                return GetCoinsRewards();
            case RewardType.Diamonds:
                return GetDiamondsRewards();
            case RewardType.Characteristics:
                return GetCharacteristicRewards();
            case RewardType.Gadget:
                return GetGadgetRewards();
            case RewardType.Chest:
                return GetChestReward();
            default:
                return null;
        }
    }

    private ChestReward[] GetChestReward()
    {
        List<CoinsReward> coinsReward = GetCoinsRewards().ToList();
        List<DiamondsReward> diamondsReward = GetDiamondsRewards().ToList();
        List<CharacteristicReward> characteristicRewards = GetCharacteristicRewards().ToList();
        List<GadgetReward> gadgetRewards = GetGadgetRewards().ToList();

        ChestReward chestReward = new(ChestType, gadgetRewards, characteristicRewards, coinsReward, diamondsReward);

        return new ChestReward[] { chestReward };
    }

    private CoinsReward[] GetCoinsRewards()
    {
        return new CoinsReward[] { new(_coinsCount) };
    }

    private DiamondsReward[] GetDiamondsRewards()
    {
        return new DiamondsReward[] { new(_diamondsCount) };
    }

    private CharacteristicReward[] GetCharacteristicRewards()
    {
        CharacteristicReward[] characteristicRewards = new CharacteristicReward[_characteristics.Length];

        for (int i = 0; i < _characteristics.Length; i++)
        {
            characteristicRewards[i] = GetCharacteristicReward(characteristicRewards, _characteristics[i]);
        }

        return characteristicRewards;
    }

    private CharacteristicReward GetCharacteristicReward(CharacteristicReward[] currentRewards, int rewardAmount)
    {
        int CharacteristicTypesCount = Enum.GetNames(typeof(CharacteristicType)).Length;
        int randomIndex = UnityEngine.Random.Range(0, CharacteristicTypesCount);

        for (int i = 0; i < CharacteristicTypesCount; i++)
        {
            if (currentRewards.Any(r => r != null && r.Type == (CharacteristicType)randomIndex))
            {
                randomIndex++;
                randomIndex %= CharacteristicTypesCount;
                continue;
            }

            return new CharacteristicReward((CharacteristicType)randomIndex, rewardAmount);
        }

        return null;
    }

    private GadgetReward[] GetGadgetRewards()
    {
        GadgetReward[] gadgetRewards = new GadgetReward[_gadgetCells];

        for (int i = 0; i < _gadgetCells; i++)
        {
            if (i < _garaunteedGadgets.Length)
            {
                gadgetRewards[i] = new GadgetReward(_garaunteedGadgets[i]);
            }
            else if (i < _garaunteedGadgets.Length + _garaunteedGadgetsByRarity.Length)
            {
                gadgetRewards[i] = GetGadgetReward(_garaunteedGadgetsByRarity[i - _garaunteedGadgets.Length]);
            }
            else
            {
                gadgetRewards[i] = GetGadgetReward();
            }
        }

        return gadgetRewards;
    }

    private GadgetReward GetGadgetReward()
    {
        float totalChance = 0;

        foreach (GadgetInfo gadgetInfo in _gadgetInfos)
        {
            totalChance += gadgetInfo.Chance;
        }

        float randomValue = UnityEngine.Random.Range(0, totalChance);

        for (int i = 0; i < _gadgetInfos.Length; i++)
        {
            randomValue -= _gadgetInfos[i].Chance;

            if (randomValue <= 0)
            {
                return _gadgetInfos[i].GetReward();
            }
        }

        return null;
    }

    private GadgetReward GetGadgetReward(Rare gadgetRare)
    {
        float totalChance = 0;

        foreach (GadgetInfo gadgetInfo in _gadgetInfos)
        {
            if ((int)gadgetInfo.Rare < (int)gadgetRare)
            {
                continue;
            }

            totalChance += gadgetInfo.Chance;
        }

        float randomValue = UnityEngine.Random.Range(0, totalChance);

        for (int i = 0; i < _gadgetInfos.Length; i++)
        {
            if ((int)_gadgetInfos[i].Rare < (int)gadgetRare)
            {
                continue;
            }

            randomValue -= _gadgetInfos[i].Chance;

            if (randomValue <= 0)
            {
                return _gadgetInfos[i].GetReward();
            }
        }

        return new GadgetReward(GadgetSettings.Instance.GetRandomGadget(gadgetRare));
    }
}
