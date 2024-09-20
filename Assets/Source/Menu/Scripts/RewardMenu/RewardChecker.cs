using UnityEngine;

public class RewardChecker : MonoBehaviour
{
    [SerializeField] private RewardMenu _rewardMenu;

    private void Update()
    {
        if (PlayerData.IsRunRewardsEmpty == false && _rewardMenu.gameObject.activeInHierarchy == false)
        {
            _rewardMenu.GiveRunRewards();
        }
        else if (PlayerData.IsRewardsEmpty == false && _rewardMenu.gameObject.activeInHierarchy == false)
        {
            _rewardMenu.GiveRewards();
        }
    }
}
