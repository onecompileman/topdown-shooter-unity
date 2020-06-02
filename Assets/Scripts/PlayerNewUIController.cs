using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerNewUIController : AboutUIController
{

    [SerializeField]
    public AboutUIController loading;

    [SerializeField]
    public List<GameObject> weapons;

    [SerializeField]
    public List<GameObject> companions;

    [SerializeField]
    public Text life;

    [SerializeField]
    public Text mana;

    [SerializeField]
    public CameraFollow camera;

    [SerializeField]
    public GameObject lobbyControls;

    [HideInInspector]
    public List<string> weaponsString;

    [HideInInspector]
    public List<string> companionString;

    [SerializeField]
    public GameObject selectWeapon;

    [SerializeField]
    public GameObject selectWeaponError;

    void Start()
    {
        life.text = PlayerDataState.life.ToString();
        mana.text = PlayerDataState.mana.ToString();

        var maxWeaponsSlots = PlayerDataState.maxWeaponSlots;
        var maxCompanionSlots = PlayerDataState.maxCompanionSlots;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (i <= maxWeaponsSlots - 1)
            {
                weaponsString.Add(null);
            }
            weapons[i].SetActive(i <= maxWeaponsSlots - 1);
        }

        for (int i = 0; i < companions.Count; i++)
        {
            companions[i].SetActive(i <= maxCompanionSlots - 1);
        }
    }

    public void SelectWeapon(int slotIndex)
    {
        selectWeapon.SetActive(true);
        var selectWeaponScript = selectWeapon.GetComponent<SelectWeaponUIController>();
        selectWeaponScript.PlayOpenAnimation();
        selectWeaponScript.onWeaponSelectCallback = (WeaponItemShop weaponItem) =>
        {

            if (weaponItem != null)
            {
                weaponsString[slotIndex] = weaponItem.name;
                weapons[slotIndex].GetComponentInChildren<WeaponSlotUI>().weaponSlotImage.sprite = weaponItem.image;
                weapons[slotIndex].GetComponentInChildren<WeaponSlotUI>().weaponSlotImage.color = new Color(1, 1, 1);
            }
            else
            {
                weaponsString[slotIndex] = null;
                weapons[slotIndex].GetComponentInChildren<WeaponSlotUI>().weaponSlotImage.sprite = null;
                weapons[slotIndex].GetComponentInChildren<WeaponSlotUI>().weaponSlotImage.color = new Color(0.18f, 0.18f, 0.18f);
            }
        };

    }

    public override void Close()
    {
        camera.isFollowingPlayer = true;
        lobbyControls.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Play()
    {
        var hasWeapon = weaponsString.Where(weapon => weapon != null).Count() > 0;
        if (hasWeapon)
        {
            loading.gameObject.SetActive(true);
            loading.PlayOpenAnimation();
            PlayerDataState.currentWeapons = weaponsString;
            PlayerDataState.currentCompanions = companionString;
            PlayerDataState.currentLevel = 1;
            PlayerDataState.currentFloor = 1;
            SaveSystem.SavePlayerData();
            SceneManager.LoadScene("Game");
        }
        else
        {
            selectWeaponError.SetActive(true);
        }
    }
}
