using System.Collections.Generic;
using UnityEngine;

public class RunRewards : MonoBehaviour
{
    [SerializeField] private GameObject _1;
    [SerializeField] private GameObject _2;
    [SerializeField] private GameObject _3;
    [SerializeField] private Transform _rewardsParent;
    [SerializeField] private RewardItem _rewardItemPrefab;

    public void Show(List<Reward> rewards)
    {
        int place = PlayerData.PlayerPlace;

        _1.SetActive(place == 1);
        _2.SetActive(place == 2);
        _3.SetActive(place == 3);

        ClearRewards();

        foreach (Reward reward in rewards)
        {
            SpawnReward(reward);
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ClearRewards()
    {
        if (_rewardsParent == null)
        {
            return;
        }

        foreach (Transform child in _rewardsParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void SpawnReward(Reward reward)
    {
        RewardItem rewardItem = Instantiate(_rewardItemPrefab, _rewardsParent);
        rewardItem.Initialize(reward);
    }
}
