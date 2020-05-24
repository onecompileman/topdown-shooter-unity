using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{

    [SerializeField]
    public ParticleSystem pickupEffect;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            obj.gameObject.GetComponent<PlayerController>().gems++;
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

            Destroy(gameObject);
        }
    }
}
