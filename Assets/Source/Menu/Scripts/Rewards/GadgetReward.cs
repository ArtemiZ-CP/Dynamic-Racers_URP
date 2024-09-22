using System;
using UnityEngine;

[Serializable]
public class GadgetReward : Reward
{
    [SerializeField] private GadgetScriptableObject _gadget;
    [SerializeField] private int _amount;

    public GadgetScriptableObject ScriptableObject => _gadget;
    public int Amount => _amount;

    public GadgetReward(GadgetScriptableObject gadget, int count = 1)
    {
        _gadget = gadget;
        _amount = count;
    }

    public GadgetReward(GadgetReward gadgetReward)
    {
        _gadget = gadgetReward.ScriptableObject;
        _amount = gadgetReward.Amount;
    }

    public GadgetReward(GadgetSaveInfo saveInfo)
    {
        _gadget = GlobalSettings.Instance.GetGadgetByName(saveInfo.GadgetName);
        _amount = saveInfo.Amount;
    }

    public override void ApplyReward()
    {
        PlayerData.AddGadget(new Gadget(_gadget, _amount));
    }

    public override Reward[] GetSimpleRewards()
    {
        return new Reward[] { this };
    }
}
