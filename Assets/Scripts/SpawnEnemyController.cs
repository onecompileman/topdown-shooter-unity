using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyController : EnemyController
{

    [SerializeField]
    public bool canSpawn = true;

    [SerializeField]
    public int spawnLevel = 3;

    [SerializeField]
    public float scaleMultiplier = 0.7f;

    [SerializeField]
    public int noOfSpawns = 2;

    [SerializeField]
    public Transform healthRed;

    [HideInInspector]
    public RoomManager room;

    public override void OnDeath()
    {

        if (spawnLevel > 0)
        {
            for (int i = 0; i < noOfSpawns; i++)
            {
                var spawn = Instantiate(gameObject, transform.position + new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f)), Quaternion.identity);
                spawn.transform.localScale = transform.localScale * scaleMultiplier;

                var spawnScript = spawn.GetComponent<SpawnEnemyController>();
                spawnScript.spawnLevel = spawnLevel - 1;
                spawnScript.enemySkin.color = originalColor;
                spawnScript.life = originalLife / 2;
                spawnScript.healthRed.localScale = new Vector3(healthRed.localScale.x * scaleMultiplier, healthRed.localScale.y, healthRed.localScale.z);
                spawnScript.speed = speed * 1.2f;

                spawnScript.enemyRoomIndex = room.enemies.Count;
                spawnScript.onDeathDelegate += room.RemoveEnemy;

                room.enemies.Add(spawnScript);
            }
        }

        Instantiate(deathEffects, transform.position, Quaternion.identity);

        if (canDropHealth)
        {
            var healthObject = Instantiate(health, transform.position, Quaternion.identity);

            healthObject.GetComponent<HealthPowerup>().lifeToAdd = lifeToAdd;
        }

        if (mana)
        {
            AddMana();
        }

        CallOnDeathDelegate();

        AudioSource.PlayClipAtPoint(enemyExplode, transform.position);

        Destroy(gameObject);
    }
}
