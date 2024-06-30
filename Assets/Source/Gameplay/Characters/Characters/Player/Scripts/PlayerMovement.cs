public class PlayerMovement : CharacterMovement
{
	protected override int GetUpgradeAmount(ChunkType chunkType)
	{
		return PlayerCharacteristics.GetUpgradeAmount(chunkType);
	}
}
