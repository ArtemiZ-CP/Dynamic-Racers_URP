using UnityEngine;

public class SpeedGame : SpeedGameBase
{
	override protected void Start()
	{
		base.Start();
		Invoke(nameof(StartRunning), GlobalSettings.Instance.TimeToStartRun);
	}

	private void Update()
	{
		if (IsTouchGown() && IsGameActive == false)
		{
			SetStartTouchPosition();
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
			FinishSpeedGame();
		}
	}
}
