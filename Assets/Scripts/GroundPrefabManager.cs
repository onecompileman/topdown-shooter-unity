using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPrefabManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> level1Rooms;
    [SerializeField]
    public List<GameObject> level2Rooms;

    [SerializeField]
    public List<GameObject> level3Rooms;

    [SerializeField]
    public List<GameObject> level4Rooms;

    public GameObject GetRandomRoom(int level)
    {
        var randomIndex = Random.Range(0, 7);

        switch (level)
        {
            case 1:
                return level1Rooms[randomIndex];
            case 2:
                return level2Rooms[randomIndex];
            case 3:
                return level3Rooms[randomIndex];
            case 4:
                return level4Rooms[randomIndex];
            default:
                return level1Rooms[randomIndex];
        }
    }
}
