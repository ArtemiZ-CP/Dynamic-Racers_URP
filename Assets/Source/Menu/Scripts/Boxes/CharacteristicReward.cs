public class CharacteristicReward : Reward
{
    public CharacteristicType Type { get; }
    public int Amount { get; }

    public CharacteristicReward(CharacteristicType type, int amount)
    {
        Type = type;
        Amount = amount;
    }
}
