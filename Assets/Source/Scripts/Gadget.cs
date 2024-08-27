using System;

[Serializable]
public class Gadget
{
    private GadgetScriptableObject _gadgetScriptableObject;
    private int _amount;
    private int _level;

    public GadgetScriptableObject GadgetScriptableObject => _gadgetScriptableObject;
    public int Level => _level;

    public Gadget(GadgetScriptableObject gadgetScriptableObject, int amount = 1, int level = 0)
    {
        _gadgetScriptableObject = gadgetScriptableObject;
        _amount = amount;
        _level = level;
    }

    public Gadget(Gadget gadget)
    {
        _gadgetScriptableObject = gadget.GadgetScriptableObject;
        _amount = gadget.GetAmount();
    }

    public bool TryLevelUp()
    {
        if (GlobalSettings.Instance.TryGetGadgetsLevelProgression(_level, out int gadgetsToLevelUp, out int coinsCost))
        {
            if (_amount >= gadgetsToLevelUp && PlayerData.TryToSpendCoins(coinsCost))
            {
                _amount -= gadgetsToLevelUp;
                _level++;
                DataSaver.SaveData();

                return true;
            }
        }

        return false;
    }

    public void AddAmount(int amount)
    {
        _amount += amount;
    }

    public int GetAmount()
    {
        return _amount;
    }
}
