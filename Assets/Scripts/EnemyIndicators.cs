using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicators : MonoBehaviour
{

    [SerializeField]
    public List<GameObject> enemyIndicators;

    [SerializeField]
    public Camera camera;

    [SerializeField]
    public PlayerController player;

    void Update()
    {
        foreach (var indicator in enemyIndicators)
        {
            indicator.SetActive(false);
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy").Where(enemy => !IsTargetVisible(camera, enemy)).ToList();

        for (int i = 0; i < enemies.Count(); i++)
        {
            if (i <= enemyIndicators.Count() - 1)
            {
                var indicator = enemyIndicators.ElementAt(i);

                indicator.SetActive(true);

                var targetVector = enemies.ElementAt(i).transform.position - player.transform.position;
                indicator.transform.rotation = Quaternion.Euler(0, -Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg + 90, 0);
            }
        }

    }

    bool IsTargetVisible(Camera c, GameObject go)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var point = go.transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
                return false;
        }
        return true;
    }
}
