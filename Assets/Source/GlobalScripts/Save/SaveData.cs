using System;
using System.Linq;
using UnityEditor;

[Serializable]
public class SaveData
{
    public RewardSaveInfo[] Rewards;
    public RewardSaveInfo[] RunRewards;
    public int PlayerPlace;
    public PlayerGadgetSaveInfo[] PlayerGadgets;
    public OpeningChestSaveInfo[] OpeningChests;
    public CompanyBiomSaveInfo[] CompanyBiomInfos;
    public int Experience;
    public int Level;
    public int Coins;
    public int Diamonds;
    public int Tickets;
    public int PlayerRace;
    public int PlayerDive;
    public int PlayerAscend;
    public int PlayerGlide;
    public int TrainingsPassed;
    public int FPS;
    public bool IsMusicOn;
    public bool IsSoundsOn;
    public bool IsHapticOn;
    public DateToSave LastTimeBattlePassBought;
    public DateToSave LastUpdateShopDay;
    public int ShopRandomSeed;
}

[Serializable]
public class CompanyBiomSaveInfo
{
    public int ID;
    public int CurrentStars;

    public CompanyBiomSaveInfo(ICompanyBiomInfoReadOnly companyBiomInfo)
    {
        ID = companyBiomInfo.ID;
        CurrentStars = companyBiomInfo.CurrentStars;
    }
}

[Serializable]
public class OpeningChestSaveInfo
{
    public bool IsOpening;
    public bool HasChest;
    public int ChestRareInt;
    public DateToSave StartOpeningTime;
    public double OpeningTimeInSeconds;

    public OpeningChestSaveInfo(OpeningChest openingChest)
    {
        IsOpening = openingChest.IsOpening;
        HasChest = openingChest.HasChest;
        ChestRareInt = (int)openingChest.ChestRare;
        StartOpeningTime = new DateToSave { DateTimeValue = openingChest.StartOpeningTime };
        OpeningTimeInSeconds = openingChest.OpeningTimeInSeconds;
    }
}

[Serializable]
public class PlayerGadgetSaveInfo
{
    public string GadgetName;
    public int Amount;
    public int Level;

    public PlayerGadgetSaveInfo(Gadget gadget)
    {
        GadgetName = gadget.ScriptableObject.Name;
        Amount = gadget.GetAmount();
        Level = gadget.Level;
    }
}

[Serializable]
public abstract class RewardSaveInfo
{
}

[Serializable]
public class GadgetSaveInfo : RewardSaveInfo
{
    public string GadgetName;
    public int Amount;

    public GadgetSaveInfo(GadgetReward reward)
    {
        GadgetName = reward.ScriptableObject.Name;
        Amount = reward.Amount;
    }
}

[Serializable]
public class CharacteristicSaveInfo : RewardSaveInfo
{
    public int TypeInt;
    public int Amount;

    public CharacteristicSaveInfo(CharacteristicReward reward)
    {
        TypeInt = (int)reward.Type;
        Amount = reward.Amount;
    }
}

[Serializable]
public class ChestSaveInfo : RewardSaveInfo
{
    public int ChestTypeInt;
    public GadgetSaveInfo[] GadgetRewards;
    public CharacteristicSaveInfo[] CharacteristicRewards;
    public CoinsSaveInfo CoinsReward;
    public DiamondsSaveInfo DiamondsReward;

    public ChestSaveInfo(ChestReward reward)
    {
        ChestTypeInt = (int)reward.Type;

        if (reward.GadgetRewards != null)
        {
            GadgetRewards = reward.GadgetRewards.Select(gadgetReward => new GadgetSaveInfo(gadgetReward)).ToArray();
        }

        if (reward.CharacteristicRewards != null)
        {
            CharacteristicRewards = reward.CharacteristicRewards.Select(characteristicReward => new CharacteristicSaveInfo(characteristicReward)).ToArray();
        }

        if (reward.CoinsReward != null)
        {
            CoinsReward = new CoinsSaveInfo(reward.CoinsReward);
        }

        if (reward.DiamondsReward != null)
        {
            DiamondsReward = new DiamondsSaveInfo(reward.DiamondsReward);
        }
    }
}

[Serializable]
public class CoinsSaveInfo : RewardSaveInfo
{
    public int Amount;

    public CoinsSaveInfo(CoinsReward reward)
    {
        Amount = reward.Amount;
    }
}

[Serializable]
public class DiamondsSaveInfo : RewardSaveInfo
{
    public int Amount;

    public DiamondsSaveInfo(DiamondsReward reward)
    {
        Amount = reward.Amount;
    }
}

[Serializable]
public class DateToSave
{
    public DateTime DateTimeValue { get; set; }
}
