using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectWeaponDisplayItem : MonoBehaviour
{

    [SerializeField]
    public WeaponItemShop weaponItem;

    [SerializeField]
    public Image weaponImage;

    [SerializeField]
    public Text manaText;

    private SelectWeaponUIController selectWeapon;

    void Start()
    {
        if (weaponItem != null)
        {
            if (!PlayerDataState.weapons.Contains(weaponItem.name))
            {
                gameObject.SetActive(false);
            }
            weaponImage.sprite = weaponItem.image;
            manaText.text = weaponItem.manaCost.ToString();
        }
        selectWeapon = GameObject.Find("SelectWeaponPanel").GetComponent<SelectWeaponUIController>();
    }

    public void OnSelectWeapon()
    {
        selectWeapon.OnWeaponSelect(weaponItem);
    }

}
