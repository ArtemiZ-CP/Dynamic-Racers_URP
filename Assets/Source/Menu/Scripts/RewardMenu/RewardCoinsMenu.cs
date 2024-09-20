using TMPro;
using UnityEngine;

public class RewardCoinsMenu : ShowRewardMenu
{
    [SerializeField] private TMP_Text _rewardAmount;

    public void Show(CoinsReward coinsReward)
    {
        _rewardAmount.text = coinsReward.Amount.ToString();
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
