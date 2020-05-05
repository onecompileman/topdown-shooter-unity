using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    public ParticleSystem collideEffects;
    public float damage = 20;

    public float speed = 30;

    public float maxDistance = 30;

    public Vector3 velocity = new Vector3(0, 0, 0);

    private float distanceTravelled = 0;

    void Update()
    {
        transform.Translate(velocity * speed * Time.deltaTime);
        distanceTravelled += speed * Time.deltaTime;
        if (distanceTravelled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && gameObject.tag == "PlayerBullet")
        {
            Instantiate(collideEffects, transform.position, Quaternion.identity);

            // Apply damage 
            var enemyScript = col.GetComponent<EnemyController>();
            enemyScript.life -= damage;
            enemyScript.life = enemyScript.life < 0 ? 0 : enemyScript.life;
            enemyScript.TakeDamageEffect();

            var oppositeForce = gameObject.transform.forward.normalized * -1 * 0.2f;

            col.gameObject.transform.Translate(oppositeForce);

            Destroy(gameObject);

        }

        if (col.gameObject.tag == "Player" && gameObject.tag == "EnemyBullet")
        {
            Instantiate(collideEffects, transform.position, Quaternion.identity);

            // Apply damage 
            var playerScript = col.GetComponent<PlayerController>();
            playerScript.life -= damage;
            playerScript.life = playerScript.life < 0 ? 0 : playerScript.life;
            playerScript.TakeDamageEffect();

            Destroy(gameObject);

        }

        if (col.gameObject.tag == "Blocker")
        {
            Instantiate(collideEffects, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
