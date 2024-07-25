using UnityEngine;

[RequireComponent(typeof(ActiveMenu))]
public class RewardChecker : MonoBehaviour
{
    [SerializeField] private RewardMenu _rewardMenu;

    private ActiveMenu _activeMenu;

    private void Awake()
    {
        _activeMenu = GetComponent<ActiveMenu>();
    }

    private void Update()
    {
        if (PlayerProgress.IsRewardQueueEmpty == false && _rewardMenu.gameObject.activeInHierarchy == false)
        {
            _activeMenu.SetActiveMenu(_rewardMenu.gameObject);
            _rewardMenu.GiveRewards();
        }
    }
}
