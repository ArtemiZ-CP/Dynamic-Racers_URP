using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
	[SerializeField] private TMP_Text _raceText;
	[SerializeField] private TMP_Text _diveText;
	[SerializeField] private TMP_Text _ascendText;
	[SerializeField] private TMP_Text _glideText;

	private void Start()
	{
		SetText();
	}

	private void SetText()
	{
		_raceText.text = GameData.PlayerRace.ToString();
		_diveText.text = GameData.PlayerDive.ToString();
		_ascendText.text = GameData.PlayerAscend.ToString();
		_glideText.text = GameData.PlayerGlide.ToString();
	}
}
