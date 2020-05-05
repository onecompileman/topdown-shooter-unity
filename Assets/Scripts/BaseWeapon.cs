
using System;
using System.Collections;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField]
    public GameObject bulletPrefab;

    [SerializeField]
    public float fireRate;

    [SerializeField]
    public float bulletDamage = 20;
    [SerializeField]
    public float bulletSpeed = 30;

    [SerializeField]
    public float bulletMaxDistance = 20;

    [SerializeField]
    public ParticleSystem muzzle;

    [SerializeField]
    public int bulletsToFire = 1;

    [SerializeField]
    public AudioSource shootSound;

    [SerializeField]
    public Joystick shootJoystick;

    [SerializeField]
    public bool isMobile = true;

    [SerializeField]
    public CameraFollow camera;

    private bool canFire = true;

    private bool isFiring = false;
    [SerializeField]
    public Animator animator;

    private int attackId;

    public void Start()
    {
        attackId = Animator.StringToHash("Attack");
        canFire = true;
    }

    void Update()
    {
        if (isMobile)
        {
            if ((Math.Abs(shootJoystick.Vertical) > 0.25 || Math.Abs(shootJoystick.Horizontal) > 0.25))
            {
                isFiring = true;
                camera.xOffset = shootJoystick.Horizontal * 7;
                camera.yOffset = shootJoystick.Vertical * 7;
            }
            else
            {
                isFiring = false;
                camera.xOffset = 0;
                camera.yOffset = 0;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && canFire)
            {
                isFiring = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isFiring = false;
            }
        }

        if (isFiring && canFire)
        {

            animator.Play("Attack");
            shootSound.Play();
            StartCoroutine("FireCooldown");
            muzzle.Play();

            if (bulletsToFire == 1)
            {

                var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                var bulletScript = bullet.GetComponent<Bullet>();

                bulletScript.velocity = transform.forward;
                bulletScript.damage = bulletDamage;
                bulletScript.speed = bulletSpeed;
                bulletScript.maxDistance = bulletMaxDistance;
            }
            else
            {
                for (int i = 0; i < bulletsToFire; i++)
                {
                    // Velocity to damp
                    var dampVelocity = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), 0, UnityEngine.Random.Range(-0.15f, 0.15f));

                    var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    var bulletScript = bullet.GetComponent<Bullet>();

                    bulletScript.velocity = transform.forward + dampVelocity;
                    bulletScript.damage = bulletDamage;
                    bulletScript.speed = bulletSpeed;
                    bulletScript.maxDistance = bulletMaxDistance;
                }
            }
        }
    }

    private IEnumerator FireCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        canFire = true;
        animator.SetBool(attackId, false);
    }
}
