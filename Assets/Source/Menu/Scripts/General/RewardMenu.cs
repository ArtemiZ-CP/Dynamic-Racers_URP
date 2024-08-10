using System.Collections;
using UnityEngine;

public class RewardMenu : MonoBehaviour
{
    private readonly int IsTouched = Animator.StringToHash(nameof(IsTouched));
    private readonly int Close = Animator.StringToHash(nameof(Close));

    [SerializeField] private Animator _box;
    [SerializeField] private Animator _bag;
    [SerializeField] private RewardWindow _rewardWindow;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private ActiveMenu _activeMenu;
    [SerializeField] private float _openingDelay = 0.5f;
    [SerializeField] private AudioSource _openBoxSound;

    private int _openStageIndex = 0;
    private Animator _currentAnimator;
    private RewardContainer _currentReward;

    public void GiveRewards()
    {
        if (PlayerData.IsRewardQueueEmpty)
        {
            CloseRewardMenu();
        }

        GiveReward(PlayerData.GetReward());
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
            case 0:
                OpenBox();
                break;
            case 1:
                ShowRewards();
                break;
            case 2:
                CloseBox();
                break;
        }
    }

    private void Awake()
    {
        _box.gameObject.SetActive(false);
        _bag.gameObject.SetActive(false);
        _rewardWindow.gameObject.SetActive(false);
    }

    private void GiveReward(RewardContainer reward)
    {
        _currentReward = reward;

        if (reward is BoxReward boxReward)
        {
            GiveReward(boxReward);
        }
        else if (reward is BagReward bagReward)
        {
            GiveReward(bagReward);
        }
    }

    private void GiveReward(BoxReward boxReward)
    {
        foreach (GadgetReward newBoxReward in boxReward.Rewards)
        {
            PlayerData.AddGadget(new Gadget(newBoxReward.Gadget, newBoxReward.Amount));
        }

        _box.gameObject.SetActive(true);
        _currentAnimator = _box; 
    }

    private void GiveReward(BagReward bagReward)
    {
        foreach (CharacteristicReward characteristic in bagReward.RewardsQueue)
        {
            PlayerData.AddCharacteristic(characteristic.Type, characteristic.Value);
        }

        _bag.gameObject.SetActive(true);
        _currentAnimator = _bag;
    }

    private void OpenBox()
    {
        _openStageIndex = 1;

        _currentAnimator.SetTrigger(IsTouched);

        StartCoroutine(ShowRewardsOnTime());

        _openBoxSound.Play();
    }

    private void ShowRewards()
    {
        _openStageIndex = 2;
        _rewardWindow.ShowReward(_currentReward);
        _currentAnimator.gameObject.SetActive(false);
    }

    private void CloseBox()
    {
        _openStageIndex = 0;
        _rewardWindow.Hide();

        _currentAnimator.SetTrigger(Close);

        GiveRewards();
    }

    private void CloseRewardMenu()
    {
        _activeMenu.SetActiveMenu(_mainMenu);
    }

    private IEnumerator ShowRewardsOnTime()
    {
        yield return new WaitForSeconds(_openingDelay);

        if (_openStageIndex == 1)
        {
            ShowRewards();
        }
    }
}
