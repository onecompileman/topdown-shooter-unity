using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExplodingEnemy : EnemyController
{
    [SerializeField]
    public bool explode;

    [SerializeField]
    public int explodeFrames;

    [SerializeField]
    public Material explodeMaterial;

    [SerializeField]
    public ParticleSystem explodeEffects;

    [SerializeField]
    public float radiusExplosion;

    [SerializeField]
    public bool destroyOnExplode = true;

    [SerializeField]
    public AudioClip explodeSound;

    private Color explodeSkinColor;


    void Start()
    {
        base.Start();
        var skinMainColor = meshRenderer.materials[2].color;
        explodeSkinColor = new Color { r = skinMainColor.r, g = skinMainColor.g, b = skinMainColor.b };
    }

    // Update is called once per frame
    void LateUpdate()
    {

    }

    public override void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            speed = 0;
            StartCoroutine("Explode");
        }
    }

    private IEnumerator Explode()
    {

        isExploding = true;

        for (int i = 0; i < explodeFrames; i++)
        {
            enemySkin.color = explodeMaterial.color;
            yield return new WaitForSeconds(0.15f);
            enemySkin.color = explodeSkinColor;
            yield return new WaitForSeconds(0.15f);
        }

        // enemySkin.color = explodeMaterial.color;

        Instantiate(explodeEffects, transform.position, Quaternion.identity);

        var acceptedTags = new string[] { "Enemy", "Player" };

        Collider[] collideObjects = Physics.OverlapSphere(transform.position, radiusExplosion)
            .Where(collider => acceptedTags.Contains(collider.gameObject.tag))
            .ToArray();

        foreach (Collider collide in collideObjects)
        {
            var gameObject = collide.gameObject;

            Rigidbody rb = gameObject.GetComponent<Rigidbody>();

            rb.AddExplosionForce(25f, transform.position, radiusExplosion);

            if (gameObject.tag == "Player")
            {
                var playerScript = gameObject.GetComponent<PlayerController>();
                playerScript.life -= damage;
                playerScript.TakeDamageEffect();
            }
        }

        if (destroyOnExplode)
        {
            AudioSource.PlayClipAtPoint(explodeSound, transform.position);
            Destroy(gameObject);
        }
        isExploding = false;
        dashEffects.Stop();
        isDashing = false;
        canDash = true;

    }
}
