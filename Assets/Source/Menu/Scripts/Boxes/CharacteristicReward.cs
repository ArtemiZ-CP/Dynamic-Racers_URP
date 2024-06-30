public class CharacteristicReward : Reward
{
    public CharacteristicType Type { get; }
    public int Value { get; }

    public CharacteristicReward(CharacteristicType type, int value)
    {
        Type = type;
        Value = value;
    }
}
