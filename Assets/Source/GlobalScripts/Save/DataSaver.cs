using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataSaver
{
    private static readonly string SaveDataKey = "SaveData";

    private static string DataPath => Application.persistentDataPath + $"/{SaveDataKey}.dat";

    public static void SaveData()
    {
        SaveData saveData = new()
        {
            BoxRewardQueue = (Queue<BoxReward>)PlayerData.BoxRewardQueue,
            BagRewardQueue = (Queue<BagReward>)PlayerData.BagRewardQueue,
            PlayerGadgets = PlayerData.PlayerGadgets.ToList(),
            Experience = PlayerData.Experience,
            Level = PlayerData.Level,
            Coins = PlayerData.Coins,
            Diamonds = PlayerData.Diamonds,
            Tickets = PlayerData.Tickets,
            PlayerRace = PlayerData.PlayerRace,
            PlayerDive = PlayerData.PlayerDive,
            PlayerAscend = PlayerData.PlayerAscend,
            PlayerGlide = PlayerData.PlayerGlide,
            TrainingsPassed = PlayerData.PassedTrainings,
            FPS = PlayerData.FPS,
            IsMusicOn = PlayerData.IsMusicOn,
            IsSoundsOn = PlayerData.IsSoundsOn,
            IsHapticOn = PlayerData.IsHapticOn
        };

        Save(saveData);
    }

    public static void LoadData()
    {
        PlayerData.LoadData(Load());
    }

    private static void Save(SaveData saveData)
    {
        BinaryFormatter bf = new();
        FileStream file = File.Create(DataPath);
        bf.Serialize(file, saveData);
        file.Close();
    }

    private static SaveData Load()
    {
        if (File.Exists(DataPath))
        {
            BinaryFormatter bf = new();
            FileStream file = File.Open(DataPath, FileMode.Open);
            SaveData saveData = (SaveData)bf.Deserialize(file);
            file.Close();
            return saveData;
        }

        return null;
    }
}
