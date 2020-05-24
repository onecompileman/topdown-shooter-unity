using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFall : MonoBehaviour
{

    [SerializeField]
    public float lerpSpeed;

    [SerializeField]
    public float targetScale;

    [SerializeField]
    public float damage;

    [SerializeField]
    public ParticleSystem hitEffect;

    private Vector3 targetVectorScale;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        targetVectorScale = new Vector3(targetScale, targetScale, targetScale);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetVectorScale, lerpSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.localScale, targetVectorScale) <= 0.2f)
        {
            rb.useGravity = true;
        }
    }

    void OnCollisionEnter(Collision obj)
    {

        if (obj.gameObject.tag == "Player")
        {
            var playerScript = obj.gameObject.GetComponent<PlayerController>();

            playerScript.life -= damage;
            playerScript.TakeDamageEffect();
            playerScript.Freeze(2f);


        }

        Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }
}
