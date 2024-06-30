using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private RewardMenu _rewardMenu;
    [SerializeField] private float _XPToLevelUp;
    [SerializeField] private List<BoxReward> _levelRewards = new();

    private int _currentLevel;

    private void Update()
    {
        CalculateLevel();
    }

    private void CalculateLevel()
    {
        _currentLevel = (int)(PlayerProgress.Experience / _XPToLevelUp);
        _slider.value = PlayerProgress.Experience % _XPToLevelUp / _XPToLevelUp;

        if (_currentLevel > PlayerProgress.Level)
        {
            GiveRewards(_currentLevel);
            PlayerProgress.Level = _currentLevel;
        }
    }

    private void GiveRewards(int newLevel)
    {
        for (int i = PlayerProgress.Level; i < newLevel; i++)
        {
            if (i < _levelRewards.Count)
            {
                GiveReward(_levelRewards[i]);
            }
        }
    }

    private void GiveReward(BoxReward reward)
    {
        if (reward == null)
        {
            return;
        }

        _rewardMenu.AddRewardToQueue(reward);
    }
}
