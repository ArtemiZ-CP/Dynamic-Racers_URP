using System;
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
            BoxRewardQueue = PlayerData.BoxRewardQueue.Select(boxReward => new ChestRewardSaveInfo
            {
                GadgetRewards = boxReward.Rewards.Select(reward => new PlayerGadgetSaveInfo
                {
                    GadgetName = reward.Gadget.Name,
                    Amount = reward.Amount,
                }).ToArray()
            }).ToArray(),

            BagRewardQueue = PlayerData.BagRewardQueue.Select(bagReward => new BagRewardSaveInfo
            {
                RewardsQueue = bagReward.RewardsQueue.Select(reward => new BagRewardSaveInfo.CharacteristicRewardSaveInfo
                {
                    TypeInt = (int)reward.Type,
                    Value = reward.Value
                }).ToArray()
            }).ToArray(),

            PlayerGadgets = PlayerData.PlayerGadgets.Select(gadget => new PlayerGadgetSaveInfo
            {
                GadgetName = gadget.GadgetScriptableObject.Name,
                Amount = gadget.GetAmount(),
                Level = gadget.Level
            }).ToArray(),

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
            IsHapticOn = PlayerData.IsHapticOn,
            LastTimeBattlePassBought = new MyData { DateTimeValue = PlayerData.LastTimeBattlePassBought },
            LastUpdateShopDay = new MyData { DateTimeValue = PlayerData.LastUpdateShopDay },
            ShopRandomSeed = PlayerData.ShopRandomSeed
        };

        Save(saveData);
    }

    public static void ResetData()
    {
        SaveData saveData = new()
        {
            BoxRewardQueue = new ChestRewardSaveInfo[0],
            BagRewardQueue = new BagRewardSaveInfo[0],
            PlayerGadgets = new PlayerGadgetSaveInfo[0],
            Experience = 0,
            Level = 0,
            Coins = 0,
            Diamonds = 0,
            Tickets = 0,
            PlayerRace = 0,
            PlayerDive = 0,
            PlayerAscend = 0,
            PlayerGlide = 0,
            TrainingsPassed = 0,
            FPS = 0,
            IsMusicOn = true,
            IsSoundsOn = true,
            IsHapticOn = true,
            LastTimeBattlePassBought = new MyData { DateTimeValue = DateTime.MinValue },
            LastUpdateShopDay = new MyData { DateTimeValue = DateTime.MinValue },
            ShopRandomSeed = (int)DateTime.Now.Ticks
        };

        Save(saveData);
        LoadData();
    }

    public static void LoadData()
    {
        SaveData loadedData = Load();

        PlayerData.LoadData(loadedData);
    }

    private static void Save(SaveData saveData)
    {
        try
        {
            BinaryFormatter bf = new();
            using FileStream file = File.Create(DataPath);
            bf.Serialize(file, saveData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save data: {ex.Message}");
        }
    }

    private static SaveData Load()
    {
        if (File.Exists(DataPath))
        {
            try
            {
                BinaryFormatter bf = new();
                using FileStream file = File.Open(DataPath, FileMode.Open);

                if (file.Length > 0)
                {
                    return (SaveData)bf.Deserialize(file);
                }
                else
                {
                    Debug.Log("Data file is empty.");
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to load data: {ex.Message}");
                return null;
            }
        }
        else
        {
            Debug.Log("Data file does not exist.");
        }

        return null;
    }
}
