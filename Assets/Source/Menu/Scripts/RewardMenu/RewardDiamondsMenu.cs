using TMPro;
using UnityEngine;

public class RewardDiamondsMenu : ShowRewardMenu
{
    [SerializeField] private TMP_Text _rewardAmount;

    public void Show(DiamondsReward diamondsReward)
    {
        _rewardAmount.text = diamondsReward.Amount.ToString();
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
