using UnityEngine;

public class CharacteristicReward : Reward
{
    [SerializeField] private CharacteristicType _type;
    [SerializeField] private int _amount;

    public CharacteristicType Type => _type;
    public int Amount => _amount;

    public CharacteristicReward(CharacteristicType type, int amount)
    {
        _type = type;
        _amount = amount;
    }

    public CharacteristicReward(CharacteristicSaveInfo saveInfo)
    {
        _type = (CharacteristicType)saveInfo.TypeInt;
        _amount = saveInfo.Amount;
    }

    public override void ApplyReward()
    {
        PlayerData.AddCharacteristic(_type, _amount);
    }

    public override Reward[] GetSimpleRewards()
    {
        return new Reward[] { this };
    }
}
