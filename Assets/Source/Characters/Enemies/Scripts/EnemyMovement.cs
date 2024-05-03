using UnityEngine;

public class EnemyMovement : CharacterMovement
{
	[SerializeField] private int _raceUpgradesAmount = 0;
	[SerializeField] private int _diveUpgradesAmount = 0;
	[SerializeField] private int _ascendUpgradesAmount = 0;
	[SerializeField] private int _glideUpgradesAmount = 0;

	protected override int GetUpgradeAmount(ChunkType chunkType)
	{
		return chunkType switch
		{
			ChunkType.Ground => _raceUpgradesAmount,
			ChunkType.Water => _diveUpgradesAmount,
			ChunkType.Wall => _ascendUpgradesAmount,
			ChunkType.Fly => _glideUpgradesAmount,
			_ => 0,
		};
	}
}
