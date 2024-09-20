using System;

[Serializable]
public class Gadget
{
    private GadgetScriptableObject _scriptableObject;
    private int _amount;
    private int _level;

    public GadgetScriptableObject ScriptableObject => _scriptableObject;
    public int Level => _level;
    public float SpeedMultiplier => _scriptableObject.SpeedMultiplier * (1 + 0.1f * _level);

    public Gadget(Gadget gadget, int level = 0)
    {
        _scriptableObject = gadget.ScriptableObject;
        _amount = gadget.GetAmount();
        _level = level;
    }

    public Gadget(GadgetScriptableObject gadgetScriptableObject, int amount = 1, int level = 0)
    {
        _scriptableObject = gadgetScriptableObject;
        _amount = amount;
        _level = level;
    }

    public bool TryGetAdditionalSpeed(out float additionalSpeed, out bool isAbleToUpgrade)
    {
        if (GlobalSettings.Instance.TryGetGadgetsLevelProgression(this, out int gadgetsToLevelUp, out int coinsCost))
        {
            isAbleToUpgrade = _amount >= gadgetsToLevelUp && PlayerData.Coins >= coinsCost;
            additionalSpeed = _scriptableObject.SpeedMultiplier * 0.1f;
            return true;
        }

        isAbleToUpgrade = false;
        additionalSpeed = 0;
        return false;
    }

    public bool TryLevelUp()
    {
        if (GlobalSettings.Instance.TryGetGadgetsLevelProgression(this, out int gadgetsToLevelUp, out int coinsCost))
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
