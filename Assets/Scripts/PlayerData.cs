using System;

[Serializable]
public class PlayerData
{
    public int life = 100;
    public int mana = 150;
    public int maxWeaponSlots = 2;
    public int coins = 0;
    public int gems = 0;

    public string[] weapons = { "Pistol" };

    public string[] companions = { };

    public int currentLevel;
    public int currentFloor;
    public int currentCoinsCollected;
    public int currentGemsCollected;
    public string[] currentWeapons;
    public string[] currentCompanions;
}
