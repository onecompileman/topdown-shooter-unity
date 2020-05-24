using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    [SerializeField]
    public GameObject coin;

    [SerializeField]
    public GameObject gem;

    [SerializeField]
    public int noOfGems;

    [SerializeField]
    public int noOfCoins;

    [SerializeField]
    public ParticleSystem openEffects;

    [HideInInspector]
    public bool opened = false;

    private Animator animator;

    private AudioSource audioSource;

    private Rigidbody rb;

    private BoxCollider box;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision obj)
    {
        if (!opened && obj.gameObject.tag == "Player")
        {
            opened = true;
            audioSource.PlayDelayed(0.1f);
            animator.Play("Open");
        }
    }

    public void OnChestOpen()
    {
        Instantiate(openEffects, transform.position, Quaternion.identity);

        Destroy(rb);
        Destroy(box);

        var positionOffset = new Vector3(0, 1f, 0);
        var forceIntensity = 1000f;

        for (int i = 0; i < noOfGems; i++)
        {
            var gemObj = Instantiate(gem, transform.position + positionOffset, Quaternion.identity);

            var randomUpwardForce = new Vector3(Random.Range(-2, 3), 4.5f, Random.Range(-2, 3));


            gemObj.GetComponent<Rigidbody>().AddForce(randomUpwardForce * forceIntensity);
        }

        for (int i = 0; i < noOfCoins; i++)
        {
            var coinObj = Instantiate(coin, transform.position + positionOffset, Quaternion.identity);

            var randomUpwardForce = new Vector3(Random.Range(-2, 3), 4.5f, Random.Range(-2, 3));


            coinObj.GetComponent<Rigidbody>().AddForce(randomUpwardForce * forceIntensity);
        }
    }

}
