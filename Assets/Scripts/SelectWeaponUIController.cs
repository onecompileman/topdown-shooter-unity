using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWeaponUIController : AboutUIController
{
    [HideInInspector]
    public Action<WeaponItemShop> onWeaponSelectCallback;


    public void OnWeaponSelect(WeaponItemShop weapon)
    {
        onWeaponSelectCallback(weapon);
        PlayCloseAnimation();
    }


}
