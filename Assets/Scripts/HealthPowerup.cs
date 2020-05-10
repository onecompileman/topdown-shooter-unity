using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : MonoBehaviour
{

    [SerializeField]
    public float lifeToAdd = 20f;

    [SerializeField]
    public ParticleSystem healthEffect;

    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(0, 2f, 0);
    }


    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            var playerScript = obj.gameObject.GetComponent<PlayerController>();

            playerScript.TakeLifeEffect(lifeToAdd);

            Instantiate(healthEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
