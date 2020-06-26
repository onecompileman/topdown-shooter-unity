using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : AboutUIController {

    private PlayerController player;

    void Start () {
        player = GameObject.Find ("Player").GetComponent<PlayerController> ();
    }

    public void WatchAds () {
        player.life = player.originalLife;
        player.mana = player.originalMana;

        PlayCloseAnimation ();
        Time.timeScale = 1;
    }

    public void Quit () {
        PlayerDataState.currentLevel = null;
        PlayerDataState.currentFloor = null;
        PlayerDataState.gems += (int) PlayerDataState.currentGemsCollected;
        PlayerDataState.coins += (int) PlayerDataState.currentCoinsCollected;
        SaveSystem.SavePlayerData ();
        Time.timeScale = 1;

        SceneManager.LoadScene ("Lobby");
    }

}