using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : EnemyController
{

    [SerializeField]
    public float flameThrowerDamage;

    [SerializeField]
    public float flameThrowerDuration;

    [SerializeField]
    public float changeModeTime;

    [SerializeField]
    public float flameThrowerCooldown;

    [SerializeField]
    public ParticleSystem flameThrower;

    [SerializeField]
    public AudioClip flameThrowerSound;

    [SerializeField]
    public float flameThrowerSpeed;

    [SerializeField]
    public float flameThrowerDistanceToShoot;

    private float originalDistanceToShoot;

    private AudioSource audioSource;
    private float originalSpeed;

    private int mode = 1;

    private bool canFlameThrower = false;

    void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        originalSpeed = speed;
        originalDistanceToShoot = distanceToShoot;
        var particleLaserCollision = flameThrower.GetComponent<ParticleLaserCollision>();
        particleLaserCollision.damage = flameThrowerDamage;

        StartCoroutine("ChangeMode");
    }

    void LateUpdate()
    {
        switch (mode)
        {
            case 1:
                canShoot = false;
                if (canFlameThrower)
                {
                    StartCoroutine("FlameAttack");
                }
                break;
            case 2:
                if (canFlameThrower)
                {
                    canShoot = true;
                    canRotateFire = true;
                    bulletsToFire = 4;
                    bulletAngle = 360;
                    angleRotateSpeed = 2;
                    attackCooldown = 0.25f;
                }
                break;
            case 3:
                if (canFlameThrower)
                {
                    canShoot = true;
                    canRotateFire = true;
                    angleRotateSpeed = 2;
                    attackCooldown = 0.3f;
                    bulletsToFire = 6;
                    canRotateFire = false;
                    attackCooldown = 0.4f;
                }
                break;


        }
    }

    private IEnumerator FlameAttack()
    {
        canFlameThrower = false;
        flameThrower.Play();
        audioSource.clip = flameThrowerSound;
        audioSource.Play();
        speed = flameThrowerSpeed;
        distanceToShoot = flameThrowerDistanceToShoot;
        yield return new WaitForSeconds(flameThrowerDuration);
        flameThrower.Stop();
        audioSource.Stop();
        speed = originalSpeed;
        distanceToShoot = originalDistanceToShoot;

        yield return new WaitForSeconds(flameThrowerCooldown);

        canFlameThrower = true;
    }

    private IEnumerator ChangeMode()
    {
        yield return new WaitForSeconds(1f);
        canFlameThrower = true;

        while (true)
        {
            yield return new WaitForSeconds(changeModeTime);
            mode = Random.Range(1, 4);
        }
    }
}
