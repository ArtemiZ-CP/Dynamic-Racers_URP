using System;

public class OpeningChest
{
    private bool _isOpening = false;
    private bool _hasChest = false;
    private DateTime _startOpeningTime;
    private ChestReward.ChestType _chestRare;
    private double _openingTimeInSeconds;

    public bool IsOpening => _isOpening;
    public bool HasChest => _hasChest;
    public DateTime StartOpeningTime => _startOpeningTime;
    public ChestReward.ChestType ChestRare => _chestRare;
    public double OpeningTimeInSeconds => _openingTimeInSeconds;
    public bool IsOpeningCompleted => _openingTimeInSeconds < (DateTime.Now - _startOpeningTime).Seconds;
    public TimeSpan TimeToOpen => TimeSpan.FromSeconds(_openingTimeInSeconds + 1) - (DateTime.Now - _startOpeningTime);

    public OpeningChest(ChestReward.ChestType chestRare)
    {
        _isOpening = false;
        _hasChest = true;
        _chestRare = chestRare;
        _openingTimeInSeconds = GlobalSettings.Instance.GetOpeningTime(chestRare).TotalSeconds;
    }

    public OpeningChest(OpeningChestSaveInfo saveInfo)
    {
        if (saveInfo == null)
        {
            return;
        }

        _isOpening = saveInfo.IsOpening;
        _hasChest = saveInfo.HasChest;
        _chestRare = (ChestReward.ChestType)saveInfo.ChestRareInt;
        _startOpeningTime = saveInfo.StartOpeningTime.DateTimeValue;
        _openingTimeInSeconds = saveInfo.OpeningTimeInSeconds;
    }

    public void StartOpening()
    {
        _isOpening = true;
        _startOpeningTime = DateTime.Now;
        DataSaver.SaveData();
    }

    public bool TryOpen()
    {
        if (_hasChest && _isOpening)
        {
            if (IsOpend())
            {
                _hasChest = false;
                PlayerData.AddReward(new ChestReward(_chestRare));
                DataSaver.SaveData();

                return true;
            }
        }

        return false;
    }

    public bool IsOpend()
    {
        TimeSpan timeSpan = DateTime.Now - _startOpeningTime;

        if (timeSpan.TotalSeconds >= _openingTimeInSeconds)
        {
            return true;
        }

        return false;
    }
}
