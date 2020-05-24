using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    public TextAsset levelJSON;

    [SerializeField]
    public FloorManager floorManager;

    [SerializeField]
    public GameObject loading;

    [HideInInspector]
    public LevelsJSON levels;

    [HideInInspector]
    public int currentLevelIndex = 0;

    [HideInInspector]
    public int currentFloorIndex = 0;

    void Start()
    {
        levels = JsonUtility.FromJson<LevelsJSON>(levelJSON.text);
        StartCoroutine("LateStart");
    }


    private IEnumerator LateStart()
    {
        loading.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        floorManager.GenerateRooms(levels.levels[currentLevelIndex].floors[currentFloorIndex]);
        yield return new WaitForSeconds(1f);
        loading.SetActive(false);
    }

}
