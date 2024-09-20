using TMPro;
using UnityEngine;

public class RewardCharacteristicMenu : ShowRewardMenu
{
    [SerializeField] private TMP_Text _characteristicText;

    public void Show(CharacteristicReward characteristicReward)
    {
        gameObject.SetActive(true);
        _characteristicText.text = $"<sprite=\"{characteristicReward.Type}\" index=0> {characteristicReward.Amount}";
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
