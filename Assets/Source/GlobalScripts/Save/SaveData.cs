using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    #region PlayerProgress

    public Queue<BoxReward> BoxRewardQueue;
    public Queue<BagReward> BagRewardQueue;
    public List<Gadget> PlayerGadgets;
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

    #endregion
}
