
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

    [SerializeField]
    public float manaCost = 1;

    [SerializeField]
    public PlayerController player;

    [SerializeField]
    public float angleFire;

    [SerializeField]
    public bool isRandomAngle = false;

    [SerializeField]
    public bool isExplodingBullet = false;

    [SerializeField]
    public bool isWaveBullet = false;

    [SerializeField]
    public GameObject explodingBullet;

    [SerializeField]
    private bool canFire = true;

    [SerializeField]
    public float dampAngle;

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
        if (!player.isStunned)
        {
            if (isMobile)
            {
                if ((Mathf.Abs(shootJoystick.Vertical) > 0.25 || Mathf.Abs(shootJoystick.Horizontal) > 0.25))
                {
                    isFiring = true;
                    camera.xOffset = shootJoystick.Horizontal * 7;
                    camera.yOffset = shootJoystick.Vertical * 5.5f;
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

            if (isFiring && canFire && player.mana >= manaCost)
            {
                player.mana -= manaCost;
                animator.Play("Attack");
                shootSound.Play();
                StartCoroutine("FireCooldown");
                muzzle.Play();

                if (isExplodingBullet)
                {
                    var bullet = Instantiate(explodingBullet, transform.position, Quaternion.identity);
                    var bulletScript = bullet.GetComponent<ExplodingBullet>();

                    bulletScript.velocity = transform.forward;
                    bulletScript.damage = bulletDamage;
                    bulletScript.speed = bulletSpeed;
                }
                else
                {

                    if (bulletsToFire == 1)
                    {
                        Vector3 velocity;
                        if (isRandomAngle)
                        {
                            var angle = Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;
                            angle = (angle + Random.Range(-dampAngle, dampAngle)) * Mathf.Deg2Rad;
                            velocity = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                        }
                        else
                        {
                            velocity = transform.forward;
                        }

                        Debug.Log(velocity);

                        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                        if (isWaveBullet)
                        {
                            var bulletScript = bullet.GetComponent<WaveBullet>();

                            bulletScript.velocity = velocity;
                            bulletScript.damage = bulletDamage;
                            bulletScript.speed = bulletSpeed;
                            bulletScript.maxDistance = bulletMaxDistance;
                        }
                        else
                        {
                            var bulletScript = bullet.GetComponent<Bullet>();

                            bulletScript.velocity = velocity;
                            bulletScript.damage = bulletDamage;
                            bulletScript.speed = bulletSpeed;
                            bulletScript.maxDistance = bulletMaxDistance;
                        }

                    }
                    else
                    {
                        if (isRandomAngle)
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
                        else
                        {
                            var startingAngle = transform.rotation.y - (angleFire / 2);
                            var incrementAngle = angleFire / bulletsToFire;

                            for (int i = 0; i < bulletsToFire; i++)
                            {

                                var radAngle = startingAngle * Mathf.Deg2Rad;
                                // Velocity to damp
                                var velocity = new Vector3(Mathf.Cos(startingAngle), 0, Mathf.Sin(startingAngle));

                                var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                                var bulletScript = bullet.GetComponent<Bullet>();

                                bulletScript.velocity = velocity;
                                bulletScript.damage = bulletDamage;
                                bulletScript.speed = bulletSpeed;
                                bulletScript.maxDistance = bulletMaxDistance;

                                startingAngle += incrementAngle;
                            }
                        }
                    }
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
