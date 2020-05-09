using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpike : MonoBehaviour
{

    [SerializeField]
    public float lerpSpeed;

    [SerializeField]
    public float damage;

    [SerializeField]
    public float stayTime;

    [SerializeField]
    public ParticleSystem explodeEffect;

    [SerializeField]
    public MeshRenderer spike1;

    [SerializeField]
    public MeshRenderer spike2;

    private AudioSource iceBreakSound;

    private Vector3 targetVector;
    private bool isCollided = false;

    private bool isDestroyed = false;


    void Start()
    {
        targetVector = new Vector3(transform.position.x, transform.position.y + 2.7f, transform.position.z);
        iceBreakSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetVector, lerpSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetVector) <= 0.15f && !isDestroyed)
        {
            StartCoroutine("DestroyObject");
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player" && !isCollided)
        {
            isCollided = true;
            var playerScript = obj.gameObject.GetComponent<PlayerController>();

            playerScript.life -= damage;
            playerScript.TakeDamageEffect();
        }
    }

    private IEnumerator DestroyObject()
    {
        isDestroyed = true;
        yield return new WaitForSeconds(stayTime);
        Instantiate(explodeEffect, transform.position, Quaternion.identity);
        iceBreakSound.volume = 0.15f;
        iceBreakSound.Play();
        spike1.enabled = false;
        spike2.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

    }
}
