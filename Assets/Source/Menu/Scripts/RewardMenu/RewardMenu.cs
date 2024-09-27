using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardMenu : MonoBehaviour
{
    private readonly int IsTouched = Animator.StringToHash(nameof(IsTouched));
    private readonly int Close = Animator.StringToHash(nameof(Close));

    [Serializable]
    private class ChestAnimationInfo
    {
        public ChestReward.ChestType ChestType;
        public Animator ChestAnimator;
        public float AnimationDuration;
    }

    [SerializeField] private ChestAnimationInfo[] _chestsAnimationInfo;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private ActiveMenu _activeMenu;
    [SerializeField] private RunRewards _runRewardsWindow;
    [SerializeField] private RewardWindow _rewardWindow;
    [SerializeField] private GameObject _pulsingText;

    private int _openStageIndex = 0;
    private Animator _currentAnimator;
    private float _rewardAnimationDuration;
    private List<Reward> _rewards = new();
    private Reward _currentReward;

    private void OnEnable()
    {
        _runRewardsWindow.Hide();
        _rewardWindow.Hide();
        ActiveChest(null);
        _pulsingText.SetActive(false);
    }

    public void GiveRunRewards()
    {
        _activeMenu.SetActiveMenu(gameObject);
        ApplyRewards(PlayerData.GetRunRewards());
        ShowRunRewards();
    }

    public void GiveRewards()
    {
        _activeMenu.SetActiveMenu(gameObject);
        ApplyRewards(PlayerData.GetRewards());
        GiveReward();
    }

    public void HandleTouch()
    {
        if (_rewardWindow.IsActive)
        {
            _rewardWindow.HandleTouch();
            return;
        }

        switch (_openStageIndex)
        {
            case -1:
                GiveReward();
                break;
            case 0:
                OpenBox();
                break;
            case 1:
                ShowRewards();
                break;
            case 2:
                HideRewards();
                break;
        }
    }

    public void CloseRewardMenu()
    {
        _activeMenu.SetActiveMenu(_mainMenu);
    }

    private void ActiveChest(ChestReward.ChestType? chestType)
    {
        foreach (ChestAnimationInfo chestAnimationInfo in _chestsAnimationInfo)
        {
            chestAnimationInfo.ChestAnimator.gameObject.SetActive(chestAnimationInfo.ChestType == chestType);

            if (chestAnimationInfo.ChestType == chestType)
            {
                _currentAnimator = chestAnimationInfo.ChestAnimator;
                _rewardAnimationDuration = chestAnimationInfo.AnimationDuration;
            }
        }

        _pulsingText.SetActive(true);
    }

    private void ApplyRewards(List<Reward> rewards)
    {
        _rewards = rewards;

        foreach (Reward reward in _rewards)
        {
            reward.ApplyReward();
        }
    }

    private void ShowRunRewards()
    {
        _openStageIndex = -1;
        _runRewardsWindow.Show(_rewards);
    }

    private void GiveReward()
    {
        if (_rewards.Count == 0)
        {
            CloseRewardMenu();
            return;
        }

        _runRewardsWindow.Hide();
        _currentReward = _rewards[0];
        _rewards.RemoveAt(0);

        if (_currentReward is ChestReward chestReward)
        {
            _openStageIndex = 0;
            ActiveChest(chestReward.Type);
        }
        else
        {
            ShowRewards();
        }
    }

    private void OpenBox()
    {
        _openStageIndex = 1;
        _currentAnimator.SetTrigger(IsTouched);
        StartCoroutine(ShowRewardsOnTime());
        _pulsingText.SetActive(false);
    }

    private void ShowRewards()
    {
        _openStageIndex = 2;
        _rewardWindow.ShowReward(_currentReward);
        _currentAnimator?.gameObject.SetActive(false);
    }

    private void HideRewards()
    {
        _openStageIndex = -1;
        _rewardWindow.Hide();
        GiveReward();
    }

    private IEnumerator ShowRewardsOnTime()
    {
        yield return new WaitForSeconds(_rewardAnimationDuration);

        if (_openStageIndex == 1)
        {
            ShowRewards();
        }
    }
}
