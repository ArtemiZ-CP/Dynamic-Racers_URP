using UnityEngine;

public static class GameData
{
	#region PlayerUpgrades

	private const string PlayerDiveKey = "PlayerDive";
	private const string PlayerGlideKey = "PlayerGlide";
	private const string PlayerAscendKey = "PlayerAscend";
	private const string PlayerRaceKey = "PlayerRace";

	#endregion

	public static int PlayerRace
	{
		get => PlayerPrefs.GetInt(PlayerRaceKey, 0);
		set => PlayerPrefs.SetInt(PlayerRaceKey, value);
	}

	public static int PlayerDive
	{
		get => PlayerPrefs.GetInt(PlayerDiveKey, 0);
		set => PlayerPrefs.SetInt(PlayerDiveKey, value);
	}

	public static int PlayerAscend
	{
		get => PlayerPrefs.GetInt(PlayerAscendKey, 0);
		set => PlayerPrefs.SetInt(PlayerAscendKey, value);
	}

	public static int PlayerGlide
	{
		get => PlayerPrefs.GetInt(PlayerGlideKey, 0);
		set => PlayerPrefs.SetInt(PlayerGlideKey, value);
	}

	public static int GetUpgradeAmount(ChunkType chunkType)
	{
		switch (chunkType)
		{
			case ChunkType.Ground:
				return PlayerRace;
			case ChunkType.Water:
				return PlayerDive;
			case ChunkType.Wall:
				return PlayerAscend;
			case ChunkType.Fly:
				return PlayerGlide;
			default:
				return 0;
		}
	}

	public static void SaveData()
	{
		PlayerPrefs.Save();
	}
}
