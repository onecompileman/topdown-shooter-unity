using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponItemShop : ScriptableObject
{
    public new string name;
    public int price;
    public string description;
    public Sprite image;
}
