using System;
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
        SaveData saveData = null;

        try
        {
            saveData = new()
            {
                Rewards = SaveRewards(PlayerData.Rewards.ToList()),
                RunRewards = SaveRewards(PlayerData.RunRewards.ToList()),
                PlayerPlace = PlayerData.PlayerPlace,
                PlayerGadgets = PlayerData.PlayerGadgets.Select(gadget => new PlayerGadgetSaveInfo(gadget)).ToArray(),
                OpeningChests = PlayerData.OpeningChests.Select(openingChest => new OpeningChestSaveInfo(openingChest)).ToArray(),
                CompanyBiomInfos = PlayerData.CompanyBiomInfos.Select(companyBiomInfo => new CompanyBiomSaveInfo(companyBiomInfo)).ToArray(),
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
                LastTimeBattlePassBought = new DateToSave { DateTimeValue = PlayerData.LastTimeBattlePassBought },
                LastUpdateShopDay = new DateToSave { DateTimeValue = PlayerData.LastUpdateShopDay },
                ShopRandomSeed = PlayerData.ShopRandomSeed
            };
        }
        catch
        {
        }

        Save(saveData);
    }

    public static void ResetData()
    {
        GlobalSettings globalSettings = GlobalSettings.Instance;

        SaveData saveData = new()
        {
            Rewards = null,
            RunRewards = null,
            PlayerGadgets = new PlayerGadgetSaveInfo[0],
            OpeningChests = new OpeningChestSaveInfo[4],
            CompanyBiomInfos = null,
            Experience = 0,
            Level = 0,
            Coins = 0,
            Diamonds = 0,
            Tickets = globalSettings.MaxTickets,
            PlayerRace = 0,
            PlayerDive = 0,
            PlayerAscend = 0,
            PlayerGlide = 0,
            TrainingsPassed = 0,
            FPS = globalSettings.MaxFPS,
            IsMusicOn = true,
            IsSoundsOn = true,
            IsHapticOn = true,
            LastTimeBattlePassBought = new DateToSave { DateTimeValue = DateTime.MinValue },
            LastUpdateShopDay = new DateToSave { DateTimeValue = DateTime.MinValue },
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

    private static RewardSaveInfo[] SaveRewards(List<Reward> rewards)
    {
        if (rewards == null)
        {
            return null;
        }

        RewardSaveInfo[] rewardSaveInfo = new RewardSaveInfo[rewards.Count];

        for (int i = 0; i < rewards.Count; i++)
        {
            Reward reward = rewards.ToList()[i];

            if (reward is ChestReward chestReward)
            {
                rewardSaveInfo[i] = new ChestSaveInfo(chestReward);
            }
            else if (reward is GadgetReward gadgetReward)
            {
                rewardSaveInfo[i] = new GadgetSaveInfo(gadgetReward);
            }
            else if (reward is CharacteristicReward characteristicReward)
            {
                rewardSaveInfo[i] = new CharacteristicSaveInfo(characteristicReward);
            }
            else if (reward is CoinsReward coinsReward)
            {
                rewardSaveInfo[i] = new CoinsSaveInfo(coinsReward);
            }
            else if (reward is DiamondsReward diamondsReward)
            {
                rewardSaveInfo[i] = new DiamondsSaveInfo(diamondsReward);
            }
        }

        return rewardSaveInfo;
    }
}
