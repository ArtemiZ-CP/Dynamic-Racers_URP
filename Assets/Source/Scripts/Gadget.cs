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

    public bool TryLevelUp()
    {
        if (GlobalSettings.Instance.TryGetGadgetsLevelProgression(_level, out int gadgetsToLevelUp))
        {
            if (_amount >= gadgetsToLevelUp)
            {
                _amount -= gadgetsToLevelUp;
                _level++;
                
                return true;
            }
            else
            {
                return false;
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
