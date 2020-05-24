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

        var usedIndicatorIndex = 0;

        for (int i = 0; i < enemies.Count(); i++)
        {
            if (usedIndicatorIndex <= enemyIndicators.Count() - 1)
            {
                var indicator = enemyIndicators.ElementAt(usedIndicatorIndex++);

                indicator.SetActive(true);

                var targetVector = enemies.ElementAt(i).transform.position - player.transform.position;
                indicator.transform.rotation = Quaternion.Euler(0, -Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg + 90, 0);
            }
        }
        if (usedIndicatorIndex <= enemyIndicators.Count() - 1)
        {

            var floorGems = GameObject.Find("FloorGem (clone)");
            if (floorGems != null)
            {
                var indicator = enemyIndicators.ElementAt(usedIndicatorIndex++);

                indicator.SetActive(true);
                var targetVector = floorGems.transform.position - player.transform.position;
                indicator.transform.rotation = Quaternion.Euler(new Vector3(0, -Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg + 90, 0));
            }
        }

        if (usedIndicatorIndex <= enemyIndicators.Count() - 1)
        {

            var chests = GameObject.FindGameObjectsWithTag("Chest").Where(chest => !chest.GetComponent<Chest>().opened && !IsTargetVisible(camera, chest));

            foreach (var chest in chests)
            {
                {
                    if (usedIndicatorIndex <= enemyIndicators.Count() - 1)
                    {
                        var indicator = enemyIndicators.ElementAt(usedIndicatorIndex++);

                        indicator.SetActive(true);

                        var targetVector = chest.transform.position - player.transform.position;
                        indicator.transform.rotation = Quaternion.Euler(0, -Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg + 90, 0);
                    }
                }
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
