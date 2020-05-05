using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : EnemyController
{

    [SerializeField]
    public float laserChargeTime;

    [SerializeField]
    public float laserFireTime;

    [SerializeField]
    public float laserCooldownTime;

    [SerializeField]
    public ParticleSystem laserCharge;

    [SerializeField]
    public float changeModeTime;


    [SerializeField]
    public ParticleSystem laserBeam;

    [SerializeField]
    public ParticleSystem energySphere;

    [SerializeField]
    public float laserAttackSpeed;

    [SerializeField]
    public float chaseSpeed;

    [SerializeField]
    public AudioClip laserChargeSound;


    [SerializeField]
    public AudioClip laserBeamSound;

    private float originalDistanceToShoot;

    private int mode = 1;

    public bool canLaserAttack = true;

    public float originalSpeed;

    private AudioSource audioSource;

    void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        originalSpeed = speed;
        originalDistanceToShoot = distanceToShoot;

        StartCoroutine("ChangeMode");
    }


    void LateUpdate()
    {
        switch (mode)
        {
            case 0:
                speed = originalSpeed;
                distanceToShoot = originalDistanceToShoot;
                if (canLaserAttack)
                {
                    canShoot = true;
                }
                break;
            case 1:
                speed = originalSpeed;
                distanceToShoot = originalDistanceToShoot;
                canShoot = false;
                if (canLaserAttack)
                {
                    StartCoroutine("LaserAttack");
                }
                break;
            case 2:
                canShoot = false;
                distanceToShoot = 1f;
                speed = chaseSpeed;
                break;
        }
    }

    private IEnumerator ChangeMode()
    {
        yield return new WaitForSeconds(1);
        canLaserAttack = true;

        while (true)
        {
            yield return new WaitForSeconds(changeModeTime);
            mode = Random.Range(0, 3);
        }
    }

    private IEnumerator LaserAttack()
    {
        canLaserAttack = false;
        laserCharge.Play();
        energySphere.Stop();
        energySphere.Play();
        audioSource.volume = 0.1f;
        audioSource.PlayOneShot(laserChargeSound);
        yield return new WaitForSeconds(laserChargeTime);
        laserCharge.Stop();
        energySphere.Stop();
        speed = laserAttackSpeed;
        laserBeam.Play();
        audioSource.PlayOneShot(laserBeamSound);

        yield return new WaitForSeconds(laserFireTime);
        laserBeam.Stop();
        speed = originalSpeed;
        yield return new WaitForSeconds(laserCooldownTime);
        canLaserAttack = true;
    }
}
