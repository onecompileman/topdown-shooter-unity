using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerManaUI : MonoBehaviour
{
    [SerializeField]
    public PlayerController player;

    [SerializeField]
    public Text manaText;
    private RectTransform mana;
    void Start()
    {
        mana = GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        var width = (player.mana / player.originalMana) * 220;
        mana.sizeDelta = new Vector2(width, 15);
        mana.localPosition = new Vector3(-((220 - width) / 2), 0, 0);
        manaText.text = $@"{player.mana}/{player.originalMana}";
    }
}
