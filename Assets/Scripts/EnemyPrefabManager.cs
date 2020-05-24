using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefabManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> enemyPrefabs;

    public GameObject GetEnemy(int type)
    {
        return enemyPrefabs[type - 1];
    }
}
