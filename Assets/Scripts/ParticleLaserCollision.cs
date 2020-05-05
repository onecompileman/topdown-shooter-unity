using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLaserCollision : MonoBehaviour
{

    [SerializeField]
    public float damage = 1;
    private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnParticleCollision(GameObject obj)
    {
        if (gameObject.tag == "EnemyLaser" && obj.tag == "Player")
        {
            var playerScript = obj.GetComponent<PlayerController>();

            playerScript.life -= damage;
            playerScript.TakeDamageEffect();
        }
    }
}
