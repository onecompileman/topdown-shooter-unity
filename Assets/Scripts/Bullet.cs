using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    public ParticleSystem collideEffects;

    [SerializeField]
    public bool isExploding = false;

    [SerializeField]
    public float explosionRadius;

    [SerializeField]
    public bool canStun = false;

    [SerializeField]
    public float stunDuration;

    public float damage = 20;

    public float speed = 30;

    public float maxDistance = 30;

    public Vector3 velocity = new Vector3(0, 0, 0);

    private float distanceTravelled = 0;


    void Update()
    {
        transform.rotation = Quaternion.Euler(0, -Mathf.Atan2(velocity.z, velocity.x) * Mathf.Rad2Deg + 90, 0);
        // Debug.Log(-Mathf.Atan2(velocity.z, velocity.x) * Mathf.Rad2Deg + 90);


        transform.position = Vector3.Lerp(transform.position, transform.position + (velocity * speed), Time.deltaTime);
        // transform.Translate(velocity * speed * Time.deltaTime);
        distanceTravelled += speed * Time.deltaTime;
        if (distanceTravelled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Mine" && gameObject.tag == "PlayerBullet")
        {
            Instantiate(collideEffects, transform.position, Quaternion.identity);

            // Apply damage 
            var mine = col.GetComponent<Mine>();

            mine.life -= damage;
            mine.TakeDamage();


        }
        else if (col.gameObject.tag == "Enemy" && gameObject.tag == "PlayerBullet")
        {
            Instantiate(collideEffects, transform.position, Quaternion.identity);

            // Apply damage 
            var enemyScript = col.GetComponent<EnemyController>();


            enemyScript.life -= damage;
            enemyScript.life = enemyScript.life < 0 ? 0 : enemyScript.life;
            enemyScript.TakeDamageEffect();

            if (canStun)
            {
                enemyScript.StunEnemy(stunDuration);
            }

            if (isExploding)
            {

                Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

                foreach (var collider in colliders)
                {
                    if (collider.gameObject.tag == "Enemy" && collider.gameObject.GetInstanceID() != col.gameObject.GetInstanceID())
                    {
                        var es = collider.gameObject.GetComponent<EnemyController>();

                        es.life -= damage;
                        es.TakeDamageEffect();

                        if (canStun)
                        {
                            es.StunEnemy(stunDuration);
                        }
                    }
                }
            }

            var oppositeForce = gameObject.transform.forward.normalized * 0.2f;


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

            if (canStun)
            {
                playerScript.StunPlayer(stunDuration);
            }

            playerScript.TakeDamageEffect();

            Destroy(gameObject);

        }

        if (col.gameObject.tag == "Blocker")
        {
            Instantiate(collideEffects, transform.position, Quaternion.identity);

            if (isExploding)
            {

                Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

                foreach (var collider in colliders)
                {
                    if (collider.gameObject.tag == "Player" && gameObject.tag == "EnemyBullet")
                    {
                        var playerScript = collider.gameObject.GetComponent<PlayerController>();

                        playerScript.life -= damage;
                        playerScript.TakeDamageEffect();

                        if (canStun)
                        {
                            playerScript.StunPlayer(stunDuration);
                        }
                    }
                    else if (collider.gameObject.tag == "Enemy" && gameObject.tag == "PlayerBullet")
                    {

                        var enemyScript = collider.gameObject.GetComponent<EnemyController>();

                        enemyScript.life -= damage;
                        enemyScript.TakeDamageEffect();

                        if (canStun)
                        {
                            enemyScript.StunEnemy(stunDuration);
                        }
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
