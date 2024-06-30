using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RewardMenu : MonoBehaviour
{
    private readonly int IsTouched = Animator.StringToHash(nameof(IsTouched));
    private readonly int Close = Animator.StringToHash(nameof(Close));

    [SerializeField] private Box _box;
    [SerializeField] private Bag _bag;
    [SerializeField] private RewardWindow _rewardWindow;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private ActiveMenu _activeMenu;
    [SerializeField] private float _openingDelay = 0.5f;

    private Queue<BoxReward> _boxRewardQueue = new();
    private Queue<BagReward> _bagRewardQueue = new();
    private int _openStageIndex = 0;
    private bool _isBoxOpening = false;
    private Animator _boxAnimator;
    private Animator _bagAnimator;
    private RewardContainer _currentReward;

    public void AddRewardToQueue(BoxReward levelReward)
    {
        _activeMenu.SetActiveMenu(gameObject);
        _boxRewardQueue.Enqueue(levelReward);
    }

    public void AddRewardToQueue(BagReward bagReward)
    {
        _activeMenu.SetActiveMenu(gameObject);
        _bagRewardQueue.Enqueue(bagReward);
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
        _boxAnimator = _box.GetComponent<Animator>();
        _bagAnimator = _bag.GetComponent<Animator>();

        _box.gameObject.SetActive(false);
        _bag.gameObject.SetActive(false);
        _rewardWindow.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isBoxOpening == false && _boxRewardQueue.Count > 0)
        {
            _isBoxOpening = true;
            GiveReward(_boxRewardQueue.Dequeue());
        }
        else if (_isBoxOpening == false && _bagRewardQueue.Count > 0)
        {
            _isBoxOpening = true;
            GiveReward(_bagRewardQueue.Dequeue());
        }
        else if (_isBoxOpening == false && _boxRewardQueue.Count == 0 && _bagRewardQueue.Count == 0)
        {
            CloseRewardMenu();
        }
    }

    private void GiveReward(BoxReward levelReward)
    {
        _currentReward = levelReward;

        foreach (GadgetReward boxReward in levelReward.RewardsQueue)
        {
            PlayerProgress.AddGadget(boxReward.Gadget, boxReward.Count);
        }

        _box.Init();
    }

    private void GiveReward(BagReward bagReward)
    {
        _currentReward = bagReward;

        foreach (CharacteristicReward characteristic in bagReward.RewardsQueue)
        {
            PlayerProgress.AddCharacteristic(characteristic.Type, characteristic.Value);
        }

        _bag.Init();
    }

    private void OpenBox()
    {
        _openStageIndex = 1;

        if (_currentReward is BoxReward)
        {
            _boxAnimator.SetTrigger(IsTouched);
        }
        else if (_currentReward is BagReward)
        {
            _bagAnimator.SetTrigger(IsTouched);
        }

        StartCoroutine(ShowRewardsOnTime());
    }

    private void ShowRewards()
    {
        _openStageIndex = 2;
        _rewardWindow.ShowReward(_currentReward);
        _box.Hide();
        _bag.Hide();
    }

    private void CloseBox()
    {
        _openStageIndex = 0;
        _rewardWindow.Hide();

        if (_currentReward is BoxReward)
        {
            _boxAnimator.SetTrigger(Close);
        }
        else if (_currentReward is BagReward)
        {
            _bagAnimator.SetTrigger(Close);
        }

        _isBoxOpening = false;
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
