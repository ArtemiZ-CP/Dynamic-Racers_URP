using System.Collections.Generic;
using UnityEngine;

public class RewardWindow : MonoBehaviour
{
    [SerializeField] private RewardGadgetMenu _rewardGadget;
    [SerializeField] private RewardCharacteristicMenu _rewardCharacteristic;
    [SerializeField] private RewardCoinsMenu _rewardCoins;
    [SerializeField] private RewardDiamondsMenu _rewardDiamonds;
    [SerializeField] private RewardMenu _rewardMenu;

    private Queue<Reward> _rewards = new();
    private ShowRewardMenu _currentShowMenu = null;

    public bool IsActive { get; private set; }

    public void ShowReward(Reward reward)
    {
        IsActive = true;
        gameObject.SetActive(true);

        _rewards = new Queue<Reward>(reward.GetSimpleRewards());

        HandleTouch();
    }

    public void HandleTouch()
    {
        if (_rewards.Count == 0)
        {
            IsActive = false;
            Hide();
            _rewardMenu.HandleTouch();
            return;
        }

        if (_currentShowMenu != null)
        {
            _currentShowMenu.Hide();
            _currentShowMenu = null;
        }

        Reward reward = _rewards.Dequeue();

        if (reward is GadgetReward gadgetReward)
        {
            _currentShowMenu = _rewardGadget;
            _rewardGadget.Show(gadgetReward);
        }
        else if (reward is CharacteristicReward characteristicReward)
        {
            _currentShowMenu = _rewardCharacteristic;
            _rewardCharacteristic.Show(characteristicReward);
        }
        else if (reward is CoinsReward coinsReward)
        {
            _currentShowMenu = _rewardCoins;
            _rewardCoins.Show(coinsReward);
        }
        else if (reward is DiamondsReward diamondsReward)
        {
            _currentShowMenu = _rewardDiamonds;
            _rewardDiamonds.Show(diamondsReward);
        }
    }

    public void Hide()
    {
        _rewardGadget.Hide();
        _rewardCharacteristic.Hide();
        _rewardCoins.Hide();
        _rewardDiamonds.Hide();
    }
}
