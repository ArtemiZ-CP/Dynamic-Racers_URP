using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    [SerializeField] private GameObject _coinsReward;
    [SerializeField] private GameObject _diamondsReward;
    [SerializeField] private Image _chestReward;
    [SerializeField] private TMP_Text _rewardAmount;

    public void Initialize(Reward reward)
    {
        if (reward == null)
        {
            return;
        }

        if (reward is CoinsReward coinsReward)
        {
            _coinsReward.SetActive(true);
            _rewardAmount.text = coinsReward.Amount.ToString();
            _rewardAmount.gameObject.SetActive(true);
        }
        else
        {
            _coinsReward.SetActive(false);
        }

        if (reward is DiamondsReward diamondsReward)
        {
            _diamondsReward.SetActive(true);
            _rewardAmount.text = diamondsReward.Amount.ToString();
            _rewardAmount.gameObject.SetActive(true);
        }
        else
        {
            _diamondsReward.SetActive(false);
        }

        if (reward is ChestReward chestReward)
        {
            _chestReward.gameObject.SetActive(true);
            _chestReward.sprite = GlobalSettings.Instance.GetChestSprite(chestReward.Type);
            _rewardAmount.gameObject.SetActive(false);
        }
        else
        {
            _chestReward.gameObject.SetActive(false);
        }

        if (reward is GadgetReward)
        {
            throw new System.NotImplementedException();
        }
    }
}
