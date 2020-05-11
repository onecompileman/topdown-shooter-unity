using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : EnemyController
{

    [SerializeField]
    public GameObject iceSpike;

    [SerializeField]
    public GameObject iceFall;

    [SerializeField]
    public float iceSpikeInterval;

    [SerializeField]
    public float iceSpikeLength;

    [SerializeField]
    public float iceSpikeCooldown;

    [SerializeField]
    public float iceFallCooldown;

    [SerializeField]
    public float changeModeTime;

    [SerializeField]
    public float iceSpikeDistance;

    [SerializeField]
    public AudioClip iceSpikeSound;

    private AudioSource audioSource;

    private int mode = 1;

    private bool canIceSpike = false;
    private bool canIceFall = false;

    private float originalSpeed;
    private float originalDistanceToShoot;

    void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();

        originalSpeed = speed;
        originalDistanceToShoot = distanceToShoot;

        StartCoroutine("ChangeMode");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (mode)
        {
            case 0:
                // canShoot = tuer;
                // canAttack = false;
                canShoot = false;
                distanceToShoot = 15f;
                if (canIceSpike)
                {
                    StartCoroutine("IceSpike");
                }
                break;
            case 1:
                canShoot = true;
                distanceToShoot = originalDistanceToShoot;
                // canAttack = false;
                if (canIceFall)
                {
                    StartCoroutine("IceFall");
                }
                break;
            case 2:
                canShoot = true;
                distanceToShoot = originalDistanceToShoot;
                break;
        }
    }

    private IEnumerator ChangeMode()
    {
        yield return new WaitForSeconds(3);
        canIceSpike = true;
        canIceFall = true;

        while (true)
        {
            yield return new WaitForSeconds(changeModeTime);
            mode = Random.Range(0, 3);
            // if (mode == 2 || mode == 1)
            // {
            //     canAttack = true;
            // }
            // else
            // {
            //     canAttack = false;
            // }

            // if (mode == 1)
            // {
            //     // canIceSpike = true;
            //     canIceFall = true;
            // }
        }
    }

    private IEnumerator IceSpike()
    {
        canIceSpike = false;

        var numberOfIceSpikes = Mathf.RoundToInt(iceSpikeLength / iceSpikeDistance);

        var initialForward = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);

        audioSource.volume = 0.3f;

        animator.Play("Attack");


        audioSource.PlayOneShot(iceSpikeSound);

        speed = 0;

        // yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < numberOfIceSpikes; i++)
        {
            var obj = Instantiate(iceSpike, (transform.position + (initialForward * (i + 1) * iceSpikeDistance)) - new Vector3(0, 2.5f, 0), Quaternion.identity);

            var objectScript = obj.GetComponent<IceSpike>();
            objectScript.damage = damage;

            yield return new WaitForSeconds(iceSpikeInterval);
        }

        speed = originalSpeed;
        yield return new WaitForSeconds(iceSpikeCooldown);
        canIceSpike = true;
    }

    private IEnumerator IceFall()
    {
        canIceFall = false;
        var iceFallPosition = new Vector3(follow.position.x, follow.position.y + 6, follow.position.z);
        var obj = Instantiate(iceFall, iceFallPosition, Quaternion.identity);

        var objectScript = obj.GetComponent<IceFall>();
        objectScript.damage = damage;
        yield return new WaitForSeconds(iceFallCooldown);
        canIceFall = true;
    }
}
