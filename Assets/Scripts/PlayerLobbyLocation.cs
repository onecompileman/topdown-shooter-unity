using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLobbyLocation : MonoBehaviour
{
    [SerializeField]
    public RectTransform playerImage;

    [SerializeField]
    public RectTransform uiMap;

    [SerializeField]
    public GameObject player;
    void Update()
    {
        var mapSizeHalf = (80 / 2) - 3f;
        var roomSizeHalf = 74.47f / 2;

        var xPlayerImage = Map(player.transform.position.x, -roomSizeHalf, roomSizeHalf, uiMap.transform.position.x - mapSizeHalf, uiMap.transform.position.x + mapSizeHalf);
        var yPlayerImage = Map(player.transform.position.z, -roomSizeHalf, roomSizeHalf, uiMap.transform.position.y - mapSizeHalf, uiMap.transform.position.y + mapSizeHalf);

        playerImage.position = new Vector3(xPlayerImage, yPlayerImage, 0);
    }

    private float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
