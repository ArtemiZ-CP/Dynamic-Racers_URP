using TMPro;
using UnityEngine;

public class AddCharacteristic : MonoBehaviour
{
    [SerializeField] private CharacteristicType _characteristicType;
    [SerializeField] private int _upgradeAmount;
    
    [ContextMenu("Give Upgrades")]
    public void GiveUpgrades()
    {
        GiveUpgrades(_characteristicType, _upgradeAmount);
    }

    public void GiveRunUpgradesWithButton(TMP_InputField inputField)
    {
        GiveUpgradesWithButton(CharacteristicType.Run, inputField);
    }
    
    public void GiveSwimUpgradesWithButton(TMP_InputField inputField)
    {
        GiveUpgradesWithButton(CharacteristicType.Swim, inputField);
    }

    public void GiveFlyUpgradesWithButton(TMP_InputField inputField)
    {
        GiveUpgradesWithButton(CharacteristicType.Fly, inputField);
    }

    public void GiveClimbUpgradesWithButton(TMP_InputField inputField)
    {
        GiveUpgradesWithButton(CharacteristicType.Climb, inputField);
    }

    private void GiveUpgradesWithButton(CharacteristicType characteristicType, TMP_InputField inputField)
    {
        if (int.TryParse(inputField.text, out int upgrades))
        {
            GiveUpgrades(characteristicType, upgrades);
        }
        else
        {
            GiveUpgrades(characteristicType, 1);
        }
    }

    private void GiveUpgrades(CharacteristicType characteristicType, int upgrades)
    {
        PlayerData.AddCharacteristic(characteristicType, upgrades);
    }
}
