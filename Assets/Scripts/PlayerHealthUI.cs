using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    public PlayerController player;

    [SerializeField]
    public Text lifeText;
    private RectTransform health;

    void Start()
    {
        health = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        var width = (player.life / player.originalLife) * 220;
        health.sizeDelta = new Vector2(width, 20);
        health.localPosition = new Vector3(-((220 - width) / 2), 0, 0);
        lifeText.text = $@"{player.life}/{player.originalLife}";
    }
}
