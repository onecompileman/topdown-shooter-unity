using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> weapons;

    [SerializeField]
    public List<GameObject> weaponImages;

    private int activeWeaponIndex = 0;

    void Update()
    {

    }

    public void ChangeWeapons()
    {
        activeWeaponIndex = activeWeaponIndex < weapons.Count - 1 ? activeWeaponIndex + 1 : 0;

        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        foreach (GameObject weaponImage in weaponImages)
        {
            weaponImage.SetActive(false);
        }

        weapons[activeWeaponIndex].SetActive(true);
        weapons[activeWeaponIndex].GetComponent<BaseWeapon>().Start();
        weaponImages[activeWeaponIndex].SetActive(true);
    }
}
