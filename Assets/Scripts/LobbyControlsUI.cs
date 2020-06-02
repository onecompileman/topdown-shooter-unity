using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void LateUpdate()
    {
        lifeText.text = $"{PlayerDataState.life}/{PlayerDataState.life}";
        manaText.text = $"{PlayerDataState.mana}/{PlayerDataState.mana}";
        coinsText.text = PlayerDataState.coins.ToString();
        gemsText.text = PlayerDataState.gems.ToString();
    }
}
