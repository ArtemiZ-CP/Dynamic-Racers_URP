using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class PlayerProgress
{
    public static ReadOnlyCollection<GadgetScriptableObject> PlayerGadgets => _playerGadgets.AsReadOnly();
    public static float Experience { get; set; }
    public static int Level { get; set; }

    private static List<GadgetScriptableObject> _playerGadgets = new();

    public static void AddGadget(GadgetScriptableObject gadget, int amount)
    {
        if (_playerGadgets.Contains(gadget) == false)
        {
            _playerGadgets.Add(gadget);
        }
    }

    public static void AddCharacteristic(CharacteristicType characteristicType, int amount)
    {
        switch (characteristicType)
        {
            case CharacteristicType.Ascend:
                PlayerCharacteristics.PlayerAscend += amount;
                break;
            case CharacteristicType.Dive:
                PlayerCharacteristics.PlayerDive += amount;
                break;
            case CharacteristicType.Glide:
                PlayerCharacteristics.PlayerGlide += amount;
                break;
            case CharacteristicType.Race:
                PlayerCharacteristics.PlayerRace += amount;
                break;
        }
    }
}
