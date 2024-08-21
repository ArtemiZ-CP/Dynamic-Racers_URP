using System;

[Serializable]
public class SaveData
{
    public ChestRewardSaveInfo[] BoxRewardQueue;
    public BagRewardSaveInfo[] BagRewardQueue;
    public PlayerGadgetSaveInfo[] PlayerGadgets;
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
    public MyData LastTimeBattlePassBought;
    public MyData LastUpdateShopDay;
    public int ShopRandomSeed;
}

[Serializable]
public class PlayerGadgetSaveInfo
{
    public string GadgetName;
    public int Amount;
    public int Level;
}

[Serializable]
public class BagRewardSaveInfo
{
    [Serializable]
    public class CharacteristicRewardSaveInfo
    {
        public int TypeInt;
        public int Value;
    }

    public CharacteristicRewardSaveInfo[] RewardsQueue;
}

[Serializable]
public class ChestRewardSaveInfo
{
    public PlayerGadgetSaveInfo[] GadgetRewards;
}

[Serializable]
public class MyData
{
    public DateTime DateTimeValue { get; set; }
}