using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public static class PlayerDataState
{
    public static int life;
    public static int mana;
    public static int maxWeaponSlots;
    public static int maxCompanionSlots = 0;
    public static int coins;
    public static int gems;

    public static List<string> weapons = new List<string>();

    public static List<string> companions = new List<string>();

    public static int? currentLevel;
    public static int? currentFloor;
    public static int? currentCoinsCollected;
    public static int? currentGemsCollected;
    public static List<string> currentWeapons = new List<string>();
    public static List<string> currentCompanions = new List<string>();

    public static void ReadPlayerData(PlayerData data)
    {
        life = data.life;
        mana = data.mana;
        maxWeaponSlots = data.maxWeaponSlots;
        coins = data.coins;
        gems = data.gems;
        weapons = data.weapons;
        companions = data.companions;
        currentLevel = data.currentLevel;
        currentFloor = data.currentFloor;
        currentCoinsCollected = data.currentCoinsCollected;
        currentGemsCollected = data.currentGemsCollected;
        currentWeapons = data.currentWeapons;
        currentCompanions = data.currentCompanions;
    }
}
