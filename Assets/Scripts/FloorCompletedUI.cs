using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FloorCompletedUI : AboutUIController
{

    [SerializeField]
    public AudioSource audioSource;

    [SerializeField]
    public GameObject progressIndicator;

    [SerializeField]
    public Text coinsText;

    [SerializeField]
    public Text gemsText;

    [SerializeField]
    public Text panelHeader;

    [SerializeField]
    public AboutUIController loading;

    [SerializeField]
    public PlayerController player;

    private float progressIndicatorStartX = -293.4f;

    private float progressIncrementX = 33.5f;


    void Start()
    {
    }

    public void PlaceProgress()
    {
        progressIndicator.GetComponentInChildren<Text>().text = $"{PlayerDataState.currentLevel}-{PlayerDataState.currentFloor}";

        float? positionX = progressIndicatorStartX + (((((PlayerDataState.currentLevel - 1) * 5) + PlayerDataState.currentFloor) - 1) * progressIncrementX);
        progressIndicator.transform.position.Set(progressIndicatorStartX, progressIndicator.transform.position.y, progressIndicator.transform.position.z);

        LeanTween.moveLocal(progressIndicator, new Vector3(positionX == null ? 0 : (float)positionX, progressIndicator.transform.localPosition.y, progressIndicator.transform.localPosition.z), 1.5f);
    }


    public void UpdateUI()
    {
        PlaceProgress();

        PlayerDataState.currentGemsCollected = player.gems;
        PlayerDataState.currentCoinsCollected = player.coins;

        coinsText.text = PlayerDataState.currentCoinsCollected.ToString();
        gemsText.text = PlayerDataState.currentGemsCollected.ToString();

        var isLastFloor = (PlayerDataState.currentLevel == 4 && PlayerDataState.currentFloor == 5);
        var panelText = isLastFloor ? "Game Completed" : "Floor Completed";
        panelHeader.text = $"{panelText}: {PlayerDataState.currentLevel}-{PlayerDataState.currentFloor}";
    }

    public void Proceed()
    {
        if (PlayerDataState.currentFloor == 5 && PlayerDataState.currentLevel == 4)
        {
            PlayerDataState.currentLevel = null;
            PlayerDataState.currentFloor = null;
            PlayerDataState.gems += (int)PlayerDataState.currentGemsCollected;
            PlayerDataState.coins += (int)PlayerDataState.currentCoinsCollected;
            SaveSystem.SavePlayerData();
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            PlayerDataState.currentLevel += (PlayerDataState.currentFloor == 5) ? 1 : 0;
            PlayerDataState.currentFloor = (PlayerDataState.currentFloor == 5) ? 1 : PlayerDataState.currentFloor + 1;
            PlayerDataState.gems += (int)PlayerDataState.currentGemsCollected;
            PlayerDataState.coins += (int)PlayerDataState.currentCoinsCollected;
            loading.gameObject.SetActive(true);
            SaveSystem.SavePlayerData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void QuitGame()
    {
        PlayerDataState.gems += (int)PlayerDataState.currentGemsCollected;
        PlayerDataState.coins += (int)PlayerDataState.currentCoinsCollected;
        SaveSystem.SavePlayerData();
        SceneManager.LoadScene("Lobby");
    }

    public void PlaySound()
    {
        audioSource.Play();
    }



}
