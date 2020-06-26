using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField]
    public Material explodeMaterial;

    [SerializeField]
    public ParticleSystem explodeEffects;

    [SerializeField]
    public float explodeFrames;

    [SerializeField]
    public float explodeRadius;

    [SerializeField]
    public AudioClip explosionSound;

    [SerializeField]
    public float life;

    [SerializeField]
    public float mineDamage = 100f;

    private Material mineMaterial;

    private Color originalColor;
    void Start()
    {
        mineMaterial = GetComponentInChildren<MeshRenderer>().materials[0];
        originalColor = new Color(mineMaterial.color.r, mineMaterial.color.g, mineMaterial.color.b);
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            Instantiate(explodeEffects, transform.position, Quaternion.identity);
            ExplodeToNearby();

            Destroy(gameObject);
        }
    }

    public void TakeDamage()
    {
        StartCoroutine("TakeDamageEffect");
    }

    private void OnCollisionEnter(Collision obj)
    {

        if (obj.gameObject.tag == "Player")
        {
            StartCoroutine("Explode");
        }
    }

    private void ExplodeToNearby()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (var collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                var playerScript = collider.gameObject.GetComponent<PlayerController>();
                var rb = collider.gameObject.GetComponent<Rigidbody>();

                playerScript.life -= (mineDamage / 2);
                playerScript.TakeDamageEffect();

                rb.AddExplosionForce(6000000, transform.position, explodeRadius);
            }
            else if (collider.gameObject.tag == "Enemy")
            {
                var playerScript = collider.gameObject.GetComponent<EnemyController>();
                var rb = collider.gameObject.GetComponent<Rigidbody>();

                playerScript.life -= mineDamage;
                playerScript.TakeDamageEffect();

                rb.AddExplosionForce(600000, transform.position, explodeRadius);
            }
        }
    }

    private IEnumerator TakeDamageEffect()
    {
        mineMaterial.color = explodeMaterial.color;
        yield return new WaitForSeconds(0.2f);
        mineMaterial.color = originalColor;
    }

    private IEnumerator Explode()
    {
        for (int i = 0; i < explodeFrames; i++)
        {
            mineMaterial.color = explodeMaterial.color;
            yield return new WaitForSeconds(0.2f);
            mineMaterial.color = originalColor;
            yield return new WaitForSeconds(0.2f);
        }

        Instantiate(explodeEffects, transform.position, Quaternion.identity);
        ExplodeToNearby();
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1f);
        Destroy(gameObject);
    }
}