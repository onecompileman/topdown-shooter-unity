using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> weapons;

    [SerializeField]
    public List<GameObject> weaponImages;

    [SerializeField]
    public List<GameObject> filteredWeapons = new List<GameObject>();

    [SerializeField]
    public List<GameObject> filteredWeaponImages = new List<GameObject>();

    [SerializeField]
    public List<string> weaponsString = new List<string>{
        "Pistol", "Rifle", "Shotgun", "Emp", "Grenade", "Star", "Wave", "SMG", "Ice shards", "Multi"
    };

    private int activeWeaponIndex = 0;

    void Start()
    {
        var availableWeaponsIndexes = weaponsString.Select((weapon, index) => new { weapon, index })
            .Where(weaponObj => PlayerDataState.currentWeapons.Contains(weaponObj.weapon))
            .Select(weaponObj => weaponObj.index);

        foreach (var weaponIndex in availableWeaponsIndexes)
        {
            filteredWeapons.Add(weapons[weaponIndex]);
            filteredWeaponImages.Add(weaponImages[weaponIndex]);
        }
    }


    public void ChangeWeapons()
    {
        activeWeaponIndex = activeWeaponIndex < filteredWeapons.Count - 1 ? activeWeaponIndex + 1 : 0;

        foreach (GameObject weapon in filteredWeapons)
        {
            weapon.SetActive(false);
        }

        foreach (GameObject weaponImage in filteredWeaponImages)
        {
            weaponImage.SetActive(false);
        }

        filteredWeapons[activeWeaponIndex].SetActive(true);
        filteredWeapons[activeWeaponIndex].GetComponent<BaseWeapon>().Start();
        filteredWeaponImages[activeWeaponIndex].SetActive(true);
    }
}
