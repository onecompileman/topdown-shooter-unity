using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerContinueUIController : AboutUIController
{

    [SerializeField]
    public List<GameObject> weaponSlots;

    [SerializeField]
    public List<GameObject> companionSlots;

    [SerializeField]
    public List<WeaponItemShop> weaponItemShops;

    [SerializeField]
    public GameObject confirmationUI;

    [SerializeField]
    public Text coinsCollectedText;

    [SerializeField]
    public Text gemsCollectedText;

    [SerializeField]
    public GameObject progressIndicator;

    [SerializeField]
    public AboutUIController loading;

    [SerializeField]
    public PlayerNewUIController playerNewUI;

    [SerializeField]
    public CameraFollow camera;

    [SerializeField]
    public GameObject lobbyControls;

    private float progressIndicatorStartX = -293.4f;

    private float progressIncrementX = 33.5f;

    void Start()
    {
        var weaponUsed = PlayerDataState.currentWeapons.Count();
        var companionUsed = PlayerDataState.currentCompanions.Count();

        for (int i = 0; i < weaponSlots.Count; i++)
        {
            if (i <= weaponUsed - 1)
            {
                var weaponItem = weaponItemShops.FirstOrDefault(weaponItems => weaponItems.name == PlayerDataState.currentWeapons[i]);
                if (weaponItem)
                {
                    weaponSlots[i].SetActive(true);
                    weaponSlots[i].GetComponentInChildren<Image>().color = new Color(1, 1, 1);
                    weaponSlots[i].GetComponentInChildren<Image>().sprite = weaponItem.image;
                }
                else
                {
                    weaponSlots[i].SetActive(false);
                    weaponSlots[i].GetComponentInChildren<Image>().color = new Color(0.19f, 0.19f, 0.19f);
                }
            }
            else
            {
                weaponSlots[i].SetActive(false);
                weaponSlots[i].GetComponentInChildren<Image>().color = new Color(0.19f, 0.19f, 0.19f);
            }
        }

        for (int i = 0; i < companionSlots.Count; i++)
        {
            if (i <= companionUsed - 1)
            {
                companionSlots[i].SetActive(true);
            }
            else
            {
                companionSlots[i].SetActive(false);
            }
        }
    }

    public void StartNew()
    {
        confirmationUI.SetActive(true);
        var confirmationUIScript = confirmationUI.GetComponent<ConfirmationUI>();
        confirmationUIScript.PlayOpenAnimation();

        Action confirmationCallback = () =>
        {
            PlayerDataState.currentFloor = 0;
            PlayerDataState.currentLevel = 0;
            PlayerDataState.currentWeapons = new List<string>();
            PlayerDataState.currentCompanions = new List<string>();
            PlayerDataState.currentCoinsCollected = 0;
            PlayerDataState.currentGemsCollected = 0;
            PlayCloseAnimation();
            playerNewUI.gameObject.SetActive(true);
            playerNewUI.PlayOpenAnimation();
            SaveSystem.SavePlayerData();
        };

        confirmationUIScript.ShowConfirmation("Are you sure to start a new game? Doing this will lose your current game progress", confirmationCallback, () => { });
    }

    public void PlaceProgress()
    {
        progressIndicator.GetComponentInChildren<Text>().text = $"{PlayerDataState.currentLevel}-{PlayerDataState.currentFloor}";

        coinsCollectedText.text = PlayerDataState.currentCoinsCollected.ToString();
        gemsCollectedText.text = PlayerDataState.currentGemsCollected.ToString();

        float? positionX = progressIndicatorStartX + (((((PlayerDataState.currentLevel - 1) * 5) + PlayerDataState.currentFloor) - 1) * progressIncrementX);
        progressIndicator.transform.position.Set(progressIndicatorStartX, progressIndicator.transform.position.y, progressIndicator.transform.position.z);

        LeanTween.moveLocal(progressIndicator, new Vector3(positionX == null ? 0 : (float)positionX, progressIndicator.transform.localPosition.y, progressIndicator.transform.localPosition.z), 1.5f);
    }

    public void Continue()
    {
        loading.gameObject.SetActive(true);
        loading.PlayOpenAnimation();
        SceneManager.LoadScene("Game");
    }

    public override void Close()
    {
        camera.isFollowingPlayer = true;
        lobbyControls.SetActive(true);
        gameObject.SetActive(false);
    }
}