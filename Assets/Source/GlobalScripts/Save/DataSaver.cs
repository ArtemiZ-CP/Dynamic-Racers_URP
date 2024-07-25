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
            BoxRewardQueue = (Queue<BoxReward>)PlayerProgress.BoxRewardQueue,
            BagRewardQueue = (Queue<BagReward>)PlayerProgress.BagRewardQueue,
            PlayerGadgets = PlayerProgress.PlayerGadgets.ToList(),
            Experience = PlayerProgress.Experience,
            Level = PlayerProgress.Level,
            Coins = PlayerProgress.Coins,
            Diamonds = PlayerProgress.Diamonds,
            Tickets = PlayerProgress.Tickets,
            PlayerRace = PlayerProgress.PlayerRace,
            PlayerDive = PlayerProgress.PlayerDive,
            PlayerAscend = PlayerProgress.PlayerAscend,
            PlayerGlide = PlayerProgress.PlayerGlide,
            TrainingsPassed = PlayerProgress.PassedTrainings
        };

        Save(saveData);
    }

    public static void LoadData()
    {
        PlayerProgress.LoadData(Load());
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
