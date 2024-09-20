using UnityEngine;

public class PlayerMovement : CharacterMovement
{
	protected override float GetUpgradeAmount(ChunkType chunkType)
	{
		if (RunSettings.Type == RunSettings.RunType.Company)
		{
			return Mathf.Max(PlayerData.GetUpgradeAmount(chunkType) - RunSettings.ReduseUpgrades, -50);
		}
		else if (RunSettings.Type == RunSettings.RunType.Ranked)
		{
			return Mathf.Max(PlayerData.GetUpgradeAmount(chunkType) - _globalSettings.ReduseUpgradesByLevel * PlayerData.Level, -50);
		}
		
		return PlayerData.GetUpgradeAmount(chunkType);
	}
}
