using System.Collections.Generic;
using UnityEngine;

public class RewardWindow : MonoBehaviour
{
    [SerializeField] private GadgetSelectionCell _gadgetCell;
    [SerializeField] private CharacteristicCell _characteristicCell;
    [SerializeField] private RewardMenu _rewardMenu;
    [SerializeField] private AudioSource _rewardSound;

    private Queue<Reward> _rewards = new();

    public bool IsActive { get; private set; }

    public void ShowReward(ChestReward reward)
    {
        IsActive = true;
        gameObject.SetActive(true);

        _rewards = new Queue<Reward>(reward.GadgetRewards);

        HandleTouch();
    }

    public void HandleTouch()
    {
        Reward reward = _rewards.Dequeue();

        if (reward is GadgetReward gadgetReward)
        {
            ShowGadget(gadgetReward);
        }
        else if (reward is CharacteristicReward characteristicReward)
        {
            ShowCharacteristic(characteristicReward);
        }

        if (_rewards.Count == 0)
        {
            IsActive = false;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _gadgetCell.gameObject.SetActive(false);
        _characteristicCell.gameObject.SetActive(false);
    }

    private void Awake()
    {
        Hide();
    }

    private void ShowGadget(GadgetReward gadgetReward)
    {
        Gadget gadget = new(gadgetReward.Gadget, gadgetReward.Amount);

        _gadgetCell.gameObject.SetActive(true);
        _gadgetCell.Init(gadget);
        _rewardSound.Play();
    }

    private void ShowCharacteristic(CharacteristicReward characteristicReward)
    {
        _characteristicCell.gameObject.SetActive(true);
        _characteristicCell.Init(characteristicReward);
    }
}
