using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public int life = 100;
    public int mana = 150;
    public int maxWeaponSlots = 2;
    public int maxCompanionSlots = 0;
    public int coins = 1000;
    public int gems = 100;

    public List<string> weapons = new List<string>(new string[] { "Pistol" });

    public List<string> companions = new List<string>();

    public int? currentLevel;
    public int? currentFloor;
    public int? currentCoinsCollected;
    public int? currentGemsCollected;
    public List<string> currentWeapons = new List<string>();
    public List<string> currentCompanions = new List<string>();
}
