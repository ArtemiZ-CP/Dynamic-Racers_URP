using UnityEngine;

public class PlayerMovement : CharacterMovement
{
	protected override float GetUpgradeAmount(ChunkType chunkType)
	{
		return Mathf.Max(PlayerData.GetUpgradeAmount(chunkType) - _globalSettings.ReduseUpgradesByLevel * PlayerData.Level, 0);
	}
}
