public class EnemyMovement : CharacterMovement
{
	private float _raceUpgradesAmount = 0;
	private float _diveUpgradesAmount = 0;
	private float _ascendUpgradesAmount = 0;
	private float _glideUpgradesAmount = 0;

	public void SetUpgrades(float raceUpgradesAmount, float diveUpgradesAmount, float ascendUpgradesAmount, float glideUpgradesAmount)
	{
		_raceUpgradesAmount = raceUpgradesAmount;
		_diveUpgradesAmount = diveUpgradesAmount;
		_ascendUpgradesAmount = ascendUpgradesAmount;
		_glideUpgradesAmount = glideUpgradesAmount;
	}

	protected override float GetUpgradeAmount(ChunkType chunkType)
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
