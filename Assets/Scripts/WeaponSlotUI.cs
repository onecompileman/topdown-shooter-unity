using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    [SerializeField]
    public PlayerNewUIController playerNewUI;

    [SerializeField]
    public Image weaponSlotImage;

    [SerializeField]
    public int index;

    public void Select()
    {
        playerNewUI.SelectWeapon(index);
    }
}
