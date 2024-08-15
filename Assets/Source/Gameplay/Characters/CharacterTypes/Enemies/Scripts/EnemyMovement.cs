public class EnemyMovement : CharacterMovement
{
	private int _raceUpgradesAmount = 0;
	private int _diveUpgradesAmount = 0;
	private int _ascendUpgradesAmount = 0;
	private int _glideUpgradesAmount = 0;

	public void SetUpgrades(int raceUpgradesAmount, int diveUpgradesAmount, int ascendUpgradesAmount, int glideUpgradesAmount)
	{
		_raceUpgradesAmount = raceUpgradesAmount;
		_diveUpgradesAmount = diveUpgradesAmount;
		_ascendUpgradesAmount = ascendUpgradesAmount;
		_glideUpgradesAmount = glideUpgradesAmount;
	}

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
