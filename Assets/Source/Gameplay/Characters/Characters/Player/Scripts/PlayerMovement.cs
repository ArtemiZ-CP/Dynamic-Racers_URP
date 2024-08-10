public class PlayerMovement : CharacterMovement
{
	protected override int GetUpgradeAmount(ChunkType chunkType)
	{
		return PlayerData.GetUpgradeAmount(chunkType);
	}
}
