using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUIController : AboutUIController
{

    [SerializeField]
    public Text coinsText;

    [SerializeField]
    public Text gemsText;

    [SerializeField]
    public GameObject progressIndicator;

    [SerializeField]
    public AboutUIController loading;

    [SerializeField]
    public ConfirmationUI confirmation;

    [SerializeField]
    public OnScreenControlsUI onScreenControls;

    private float progressIndicatorStartX = -293.4f;

    private float progressIncrementX = 33.5f;

    void Update()
    {

    }

    public void PlaceProgress()
    {
        progressIndicator.GetComponentInChildren<Text>().text = $"{PlayerDataState.currentLevel}-{PlayerDataState.currentFloor}";

        coinsText.text = PlayerDataState.currentCoinsCollected.ToString();
        gemsText.text = PlayerDataState.currentGemsCollected.ToString();

        float? positionX = progressIndicatorStartX + (((((PlayerDataState.currentLevel - 1) * 5) + PlayerDataState.currentFloor) - 1) * progressIncrementX);
        progressIndicator.transform.position.Set(progressIndicatorStartX, progressIndicator.transform.position.y, progressIndicator.transform.position.z);

        LeanTween.moveLocal(progressIndicator, new Vector3(positionX == null ? 0 : (float)positionX, progressIndicator.transform.localPosition.y, progressIndicator.transform.localPosition.z), 1.5f);
    }

    public void QuitGame()
    {
        confirmation.gameObject.SetActive(true);
        string message = "Are you sure to quit the game?";
        Action confirmCallback = () =>
        {
            PlayCloseAnimation();
            SceneManager.LoadScene("Lobby");
        };
        confirmation.ShowConfirmation(message, confirmCallback, () => { });
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        PlayCloseAnimation();
        onScreenControls.gameObject.SetActive(true);
        Time.timeScale = 1;
    }
}
