public class PlayerMovement : CharacterMovement
{
	protected override int GetUpgradeAmount(ChunkType chunkType)
	{
		return GameData.GetUpgradeAmount(chunkType);
	}
}
