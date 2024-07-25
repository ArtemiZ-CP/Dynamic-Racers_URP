using System.Collections.Generic;

public class Gadget
{
    private GadgetScriptableObject _gadgetScriptableObject;
    private int _amount;
    private int _level;

    public GadgetScriptableObject GadgetScriptableObject => _gadgetScriptableObject;
    public int Level => _level;

    public Gadget(GadgetScriptableObject gadgetScriptableObject, int amount = 1)
    {
        _gadgetScriptableObject = gadgetScriptableObject;
        _amount = amount;
        _level = 0;
    }

    public Gadget(Gadget gadget)
    {
        _gadgetScriptableObject = gadget.GadgetScriptableObject;
        _amount = gadget.GetAmount();
    }

    public void AddAmount(int amount)
    {
        _amount += amount;
    }

    public int GetAmount()
    {
        int amount = _amount;
        IReadOnlyList<int> progression = GlobalSettings.Instance.GadgetsLevelProgression;

        for (int i = 0; i < _level; i++)
        {
            amount -= progression[i];
        }

        return amount;
    }
}
