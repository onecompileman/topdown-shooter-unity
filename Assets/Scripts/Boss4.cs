using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4 : EnemyController
{

    [SerializeField]
    public GameObject blackSpike;

    [SerializeField]
    public GameObject lightningBullet;


    [SerializeField]
    public float changeModeTime;

    [SerializeField]
    public float iceSpikeDistance;

    [SerializeField]
    public AudioClip iceSpikeSound;

    [SerializeField]
    public float iceSpikeCooldown;

    [SerializeField]
    public float iceSpikeInterval;

    [SerializeField]
    public int noOfIceSpikes;

    [SerializeField]
    public float iceSpikeAngle;

    [SerializeField]
    public float iceSpikeLength;

    [SerializeField]
    public float iceSpikeDistanceToShoot;

    private float originalDistanceToShoot;
    private float originalSpeed;

    private GameObject originalBullet;

    private float originalBulletSpeed;
    private AudioSource audioSource;

    private int mode = 1;

    private bool canIceSpike = false;

    void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        originalDistanceToShoot = distanceToShoot;
        originalBullet = bullet;
        originalSpeed = speed;
        originalBulletSpeed = bulletSpeed;
        StartCoroutine("ChangeMode");
    }

    void LateUpdate()
    {
        switch (mode)
        {
            case 0:
                if (!isDashing && canDash)
                {
                    canShoot = false;
                    canShootExploding = false;
                    canDash = false;
                    speed = originalSpeed;
                    dashForward = false;
                    distanceToShoot = iceSpikeDistanceToShoot;
                    if (canIceSpike)
                    {
                        var startingAngle = Mathf.Atan2(transform.forward.y, transform.forward.x) * Mathf.Rad2Deg - (iceSpikeAngle / 2);

                        var increment = iceSpikeAngle / (noOfIceSpikes - 1);

                        for (int i = 0; i < noOfIceSpikes; i++)
                        {
                            var radAngle = startingAngle * Mathf.Deg2Rad;
                            var initialForward = new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle));
                            initialForward.Normalize();

                            StartCoroutine("IceSpike", initialForward);

                            startingAngle += increment;
                        }

                        audioSource.clip = iceSpikeSound;
                        audioSource.volume = 0.3f;
                        audioSource.Play();

                    }
                }
                break;
            case 1:
                speed = 0;
                if (!isDashing)
                {
                    // canDash = true;
                    dashForward = true;
                }
                canShootExploding = false;
                bulletSpeed = originalBulletSpeed;
                canShoot = false;
                distanceToShoot = originalDistanceToShoot;
                break;
            case 2:
                if (!isDashing && canDash)
                {

                    canShoot = true;
                    dashForward = false;
                    speed = originalSpeed;

                    bulletSpeed = originalBulletSpeed;
                    canShootExploding = false;
                    canDash = false;
                    bullet = originalBullet;
                    distanceToShoot = originalDistanceToShoot;
                    attackCooldown = 0.35f;
                    canRotateFire = true;
                }
                break;
            case 3:
                if (!isDashing && canDash)
                {

                    canShoot = true;
                    dashForward = false;
                    speed = originalSpeed;
                    canDash = false;
                    canShootExploding = true;
                    distanceToShoot = originalDistanceToShoot;
                    attackCooldown = 1.4f;
                    canRotateFire = false;
                }
                break;
            case 4:
                if (!isDashing && canDash)
                {

                    canShootExploding = false;
                    speed = originalSpeed;
                    dashForward = false;
                    canDash = false;
                    canShoot = true;
                    attackCooldown = 1f;
                    bulletSpeed = 8f;
                    canRotateFire = false;
                    bullet = lightningBullet;
                }
                break;
        }
    }

    private IEnumerator ChangeMode()
    {
        yield return new WaitForSeconds(1f);
        canIceSpike = true;

        while (true)
        {
            yield return new WaitForSeconds(changeModeTime);
            mode = Random.Range(0, 5);

            canDash = true;
            LateUpdate();
            //     Debug.Log(mode);
        }
    }

    private IEnumerator IceSpike(Vector3 initialForward)
    {
        canIceSpike = false;

        var numberOfIceSpikes = Mathf.RoundToInt(iceSpikeLength / iceSpikeDistance);

        // var initialForward = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);


        audioSource.volume = 0.3f;

        animator.Play("Attack");

        speed = 0;

        // Debug.Log()

        // yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < numberOfIceSpikes; i++)
        {
            var obj = Instantiate(blackSpike, (transform.position + (initialForward * (i + 1) * iceSpikeDistance)) - new Vector3(0, 2.5f, 0), Quaternion.identity);

            var objectScript = obj.GetComponent<IceSpike>();
            objectScript.damage = damage;

            yield return new WaitForSeconds(iceSpikeInterval);
        }

        speed = originalSpeed;
        yield return new WaitForSeconds(iceSpikeCooldown);
        canIceSpike = true;
    }
}
