using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBullet : MonoBehaviour
{

    [SerializeField]
    public ParticleSystem explodeEffects;

    [SerializeField]
    public float explodeDamage;

    [SerializeField]
    public float damage;

    [SerializeField]
    public Vector3 velocity;

    [SerializeField]
    public float explodeRadius;

    [SerializeField]
    public float explodeFrames;

    [SerializeField]
    public float speed;

    [SerializeField]
    public Material explodingMaterial;

    public bool isExploding = false;

    private int touchGroundCtr = 0;

    private bool hasCollided;

    private Material bulletMaterial;

    private Color originalColor;
    private Rigidbody rb;

    void Start()
    {
        bulletMaterial = GetComponent<MeshRenderer>().materials[0];
        originalColor = new Color(bulletMaterial.color.r, bulletMaterial.color.g, bulletMaterial.color.b);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(velocity * speed * 400);

    }

    void Update()
    {
    }

    void OnCollisionEnter(Collision col)
    {

        // Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Ground")
        {
            touchGroundCtr++;
            if (!isExploding && touchGroundCtr == 2)
            {
                StartCoroutine("Explode");
            }
        }

        if (!hasCollided && col.gameObject.tag == "Player" && gameObject.tag == "EnemyBullet")
        {
            hasCollided = true;
            var playerScript = col.gameObject.GetComponent<PlayerController>();
            playerScript.life -= explodeDamage;
            playerScript.TakeDamageEffect();
        }
        else if (!hasCollided && col.gameObject.tag == "Enemy" && gameObject.tag == "PlayerBullet")
        {
            hasCollided = true;
            var enemyScript = col.gameObject.GetComponent<EnemyController>();
            enemyScript.life -= explodeDamage;
            enemyScript.TakeDamageEffect();
        }
    }

    private IEnumerator Explode()
    {
        isExploding = true;

        for (int i = 0; i < explodeFrames; i++)
        {
            bulletMaterial.color = explodingMaterial.color;
            yield return new WaitForSeconds(0.25f);
            bulletMaterial.color = originalColor;
            yield return new WaitForSeconds(0.25f);
        }

        Instantiate(explodeEffects, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (var collider in colliders)
        {
            if (collider.gameObject.tag == "Player" && gameObject.tag == "EnemyBullet")
            {
                var enemyScript = collider.gameObject.GetComponent<PlayerController>();

                enemyScript.life -= explodeDamage;
                enemyScript.TakeDamageEffect();

                collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(3000000, transform.position, explodeRadius);

            }
            else if (collider.gameObject.tag == "Enemy" && gameObject.tag == "PlayerBullet")
            {
                var playerScript = collider.gameObject.GetComponent<EnemyController>();

                playerScript.life -= explodeDamage;
                playerScript.TakeDamageEffect();

                collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(3000000, transform.position, explodeRadius);

            }
        }

        Destroy(gameObject);
    }
}
