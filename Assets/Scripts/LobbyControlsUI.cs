using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyControlsUI : MonoBehaviour
{
    [SerializeField]
    public Text lifeText;

    [SerializeField]
    public Text manaText;

    [SerializeField]
    public Text coinsText;

    [SerializeField]
    public Text gemsText;

    [SerializeField]
    public ConfirmationUI confirmation;

    void LateUpdate()
    {
        lifeText.text = $"{PlayerDataState.life}/{PlayerDataState.life}";
        manaText.text = $"{PlayerDataState.mana}/{PlayerDataState.mana}";
        coinsText.text = PlayerDataState.coins.ToString();
        gemsText.text = PlayerDataState.gems.ToString();
    }

    public void Exit()
    {
        confirmation.gameObject.SetActive(true);
        string message = "Are you sure you want to exit the game?";
        Action confirmCallback = () =>
        {
            confirmation.PlayCloseAnimation();
            SceneManager.LoadScene("Menu");
        };
        confirmation.ShowConfirmation(message, confirmCallback, () => { });
    }
}
