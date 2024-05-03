using UnityEngine;

public class SetUpgrades : MonoBehaviour
{
	[SerializeField] private int _race;
	[SerializeField] private int _dive;
	[SerializeField] private int _ascend;
	[SerializeField] private int _glide;

	private void Start()
	{
		GameData.PlayerRace = _race;
		GameData.PlayerDive = _dive;
		GameData.PlayerAscend = _ascend;
		GameData.PlayerGlide = _glide;
	}
}
