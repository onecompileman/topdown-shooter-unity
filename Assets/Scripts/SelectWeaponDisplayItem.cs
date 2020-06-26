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

        selectWeapon = GameObject.Find("SelectWeaponPanel").GetComponent<SelectWeaponUIController>();
    }

    void Update()
    {
        if (weaponItem != null)
        {
            if (!PlayerDataState.weapons.Contains(weaponItem.name))
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                weaponImage.sprite = weaponItem.image;
                manaText.text = weaponItem.manaCost.ToString();
            }
        }
    }

    public void OnSelectWeapon()
    {
        selectWeapon.OnWeaponSelect(weaponItem);
    }

}
