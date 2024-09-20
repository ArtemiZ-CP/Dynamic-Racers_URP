using UnityEngine;

public class TrainingSpeedGame : SpeedGameBase
{
    [Header("Training")]
    [SerializeField] private GameObject _fingerHint;
    [SerializeField] private float _showHintDelay;
    [SerializeField] private float _startDelay;

    override protected void Awake()
    {
        HideFingerHint();
        base.Awake();
    }

    override protected void Start()
    {
        base.Start();
        Invoke(nameof(ShowFingerHint), _showHintDelay);
        ShowHint();
    }

    protected override void SetStartTouchPosition()
    {
        base.SetStartTouchPosition();
        CancelInvoke(nameof(ShowFingerHint));
        HideFingerHint();
    }

    protected override void FinishSpeedGame()
    {
        Invoke(nameof(StartRunning), _startDelay);
        base.FinishSpeedGame();
    }

    private void HideFingerHint()
    {
        _fingerHint.SetActive(false);
    }

    private void ShowFingerHint()
    {
        _fingerHint.SetActive(true);
    }
}
