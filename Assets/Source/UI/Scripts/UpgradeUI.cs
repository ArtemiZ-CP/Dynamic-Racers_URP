using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
	[SerializeField] private TMP_Text _raceText;
	[SerializeField] private TMP_Text _diveText;
	[SerializeField] private TMP_Text _ascendText;
	[SerializeField] private TMP_Text _glideText;

	private void Update()
	{
		SetText();
	}

	private void SetText()
	{
		_raceText.text = PlayerCharacteristics.PlayerRace.ToString();
		_diveText.text = PlayerCharacteristics.PlayerDive.ToString();
		_ascendText.text = PlayerCharacteristics.PlayerAscend.ToString();
		_glideText.text = PlayerCharacteristics.PlayerGlide.ToString();
	}
}
