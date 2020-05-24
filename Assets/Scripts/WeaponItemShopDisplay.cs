using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItemShopDisplay : MonoBehaviour
{
    [SerializeField]
    public Image image;

    [SerializeField]
    public Text price;

    [SerializeField]
    public Text name;

    [SerializeField]
    public WeaponItemShop weaponItem;

    void Start()
    {
        image.sprite = weaponItem.image;
        price.text = weaponItem.price.ToString();
        name.text = weaponItem.name;
    }


}
