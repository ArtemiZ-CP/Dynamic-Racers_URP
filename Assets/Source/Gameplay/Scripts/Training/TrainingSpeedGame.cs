using UnityEngine;

public class TrainingSpeedGame : SpeedGameBase
{
    [Header("Training")]
    [SerializeField] private GameObject _fingerHint;
    [SerializeField] private float _showHintDelay;
    [SerializeField] private float _startDelay;

    override protected void Awake()
    {
        _fingerHint.SetActive(false);
        base.Awake();
    }

    override protected void Start()
    {
        base.Start();
        Invoke(nameof(ShowFingerHint), _showHintDelay);
    }

    private void Update()
    {
        if (IsTouchGown() && IsGameActive == false)
        {
            CancelInvoke(nameof(ShowFingerHint));
            SetStartTouchPosition();
            _fingerHint.SetActive(false);
        }
        else if (IsTouch() && IsGameActive == false)
        {
            float t = Mathf.Clamp01(GetTouchOffset() / DragOffset);
            Player.position = StartPlayerPosition + MaxPlayerOffset * t * Vector3.back;

            if (GetTouchOffset() > DragOffset)
            {
                StartGame();
            }
        }
        else if (IsTouchUp() && IsGameActive)
        {
            Invoke(nameof(StartRunning), _startDelay);
            FinishSpeedGame();
        }
    }

    private void ShowFingerHint()
    {
        _fingerHint.SetActive(true);
    }
}
