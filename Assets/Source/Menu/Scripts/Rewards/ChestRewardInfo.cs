using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestRewardInfo", menuName = "ChestRewardInfo", order = 1)]
public class ChestRewardInfo : ScriptableObject
{
    [Serializable]
    public class GadgetInfo
    {
        [SerializeField] private Rare _rare;
        [SerializeField, Min(0)] private float _chance;
        [SerializeField, Min(1)] private int _minAmount;
        [SerializeField, Min(1)] private int _maxAmount;
        [SerializeField, Min(1)] private int _amountMultiplier;

        public Rare Rare => _rare;
        public float Chance => _chance;
        public int MinAmount => _minAmount;
        public int MaxAmount => _maxAmount;
        public int AmountMultiplier => _amountMultiplier;

        public GadgetReward GetReward()
        {
            int amount = UnityEngine.Random.Range(_minAmount, _maxAmount + 1) * _amountMultiplier;

            return new GadgetReward(GlobalSettings.Instance.GetRandomGadget(_rare), amount);
        }
    }

    [SerializeField] private ChestReward.ChestType _chestType;
    [Header("Coins")]
    [SerializeField, Min(0)] private int _minCoins;
    [SerializeField, Min(0)] private int _maxCoins;
    [SerializeField, Min(0)] private int _coinsMultiplier;
    [Header("Diamonds")]
    [SerializeField, Min(0)] private int _minDiamonds;
    [SerializeField, Min(0)] private int _maxDiamonds;
    [SerializeField, Min(0)] private int _diamondsMultiplier;
    [Header("Characteristics")]
    [SerializeField, Min(0)] private int _minCharacteristics;
    [SerializeField, Min(0)] private int _maxCharacteristics;
    [SerializeField, Min(0)] private int _characteristicsMultiplier;
    [SerializeField, Min(0)] private int _minCharacteristicsCells;
    [SerializeField, Min(0)] private int _maxCharacteristicsCells;
    [Header("Gadgets")]
    [SerializeField] private GadgetInfo[] _gadgetInfos;
    [SerializeField] private Rare[] _garaunteedGadgets;
    [SerializeField, Min(0)] private int _minGadgetCells;
    [SerializeField, Min(0)] private int _maxGadgetCells;

    public ChestReward.ChestType ChestType => _chestType;
    public int MinCoins => _minCoins;
    public int MaxCoins => _maxCoins;
    public int CoinsMultiplier => _coinsMultiplier;
    public int MinDiamonds => _minDiamonds;
    public int MaxDiamonds => _maxDiamonds;
    public int DiamondsMultiplier => _diamondsMultiplier;
    public int MinCharacteristics => _minCharacteristics;
    public int MaxCharacteristics => _maxCharacteristics;
    public int CharacteristicsMultiplier => _characteristicsMultiplier;
    public int MinCharacteristicsCells => _minCharacteristicsCells;
    public int MaxCharacteristicsCells => _maxCharacteristicsCells;
    public GadgetInfo[] GadgetInfos => _gadgetInfos;
    public Rare[] GaraunteedGadgets => _garaunteedGadgets;
    public int MinGadgetCells => _minGadgetCells;
    public int MaxGadgetCells => _maxGadgetCells;

    public ChestReward GetRewards()
    {
        // return new ChestReward(_chestType);

        CoinsReward coinsReward = new(UnityEngine.Random.Range(_minCoins, _maxCoins + 1) * _coinsMultiplier);
        DiamondsReward diamondsReward = new(UnityEngine.Random.Range(_minDiamonds, _maxDiamonds + 1) * _diamondsMultiplier);

        int characteristicCells = UnityEngine.Random.Range(_minCharacteristicsCells, _maxCharacteristicsCells + 1);
        List<CharacteristicReward> characteristicRewards = new();

        for (int i = 0; i < characteristicCells; i++)
        {
            characteristicRewards.Add(GetCharacteristicReward());
        }

        int gadgetCells = UnityEngine.Random.Range(_minGadgetCells, _maxGadgetCells + 1);
        List<GadgetReward> gadgetRewards = new();

        for (int i = 0; i < gadgetCells; i++)
        {
            if (i < _garaunteedGadgets.Length)
            {
                gadgetRewards.Add(GetGadgetReward(_garaunteedGadgets[i]));
            }
            else
            {
                gadgetRewards.Add(GetGadgetReward());
            }
        }

        ChestReward chestReward = new(_chestType, gadgetRewards, characteristicRewards, coinsReward, diamondsReward);

        return chestReward;
    }

    private CharacteristicReward GetCharacteristicReward()
    {
        int randomIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(CharacteristicType)).Length);
        int amount = UnityEngine.Random.Range(_minCharacteristics, _maxCharacteristics + 1) * _characteristicsMultiplier;
        return new CharacteristicReward((CharacteristicType)randomIndex, amount);
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

        return new GadgetReward(GlobalSettings.Instance.GetRandomGadget(gadgetRare));
    }
}
