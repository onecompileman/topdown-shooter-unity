using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    public PlayerController player;

    private RectTransform health;
    private float playerHealth;

    void Start()
    {
        health = GetComponent<RectTransform>();
        playerHealth = player.life;
    }

    // Update is called once per frame
    void Update()
    {
        var width = (player.life / playerHealth) * 220;
        health.sizeDelta = new Vector2(width, 15);
        health.localPosition = new Vector3(-((220 - width) / 2), 0, 0);
    }
}
