using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SavePlayerData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, GetCurrentPlayerState());

        stream.Close();
    }

    public static void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/player.data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            PlayerDataState.ReadPlayerData(data);
        }
        else
        {
            SaveDefaultPlayerData();
        }

    }

    private static void SaveDefaultPlayerData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, new PlayerData());

        stream.Close();
    }

    private static PlayerData GetCurrentPlayerState()
    {
        return new PlayerData
        {
            life = PlayerDataState.life,
            mana = PlayerDataState.mana,
            maxWeaponSlots = PlayerDataState.maxWeaponSlots,
            coins = PlayerDataState.coins,
            gems = PlayerDataState.gems,
            weapons = PlayerDataState.weapons,
            companions = PlayerDataState.companions,
            currentLevel = PlayerDataState.currentLevel,
            currentFloor = PlayerDataState.currentFloor,
            currentCoinsCollected = PlayerDataState.currentCoinsCollected,
            currentGemsCollected = PlayerDataState.currentGemsCollected,
            currentWeapons = PlayerDataState.currentWeapons,
            currentCompanions = PlayerDataState.currentCompanions
        };
    }

}
