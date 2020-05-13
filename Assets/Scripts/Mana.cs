using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{

    [SerializeField]
    public float manaToAdd;

    [SerializeField]
    public ParticleSystem manaEffects;

    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            var playerScript = obj.gameObject.GetComponent<PlayerController>();
            playerScript.TakeMana(manaToAdd);

            Instantiate(manaEffects, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
