using UnityEngine;

public class SetUpgrades : MonoBehaviour
{
	[SerializeField] private int _race;
	[SerializeField] private int _dive;
	[SerializeField] private int _ascend;
	[SerializeField] private int _glide;

	private void Start()
	{
		PlayerCharacteristics.PlayerRace = _race;
		PlayerCharacteristics.PlayerDive = _dive;
		PlayerCharacteristics.PlayerAscend = _ascend;
		PlayerCharacteristics.PlayerGlide = _glide;
	}
}
