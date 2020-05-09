using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    public float life;

    [SerializeField]
    public Transform healthIndicator;

    [SerializeField]
    public float speed;

    [SerializeField]
    public float damage;

    [SerializeField]
    public Transform follow;

    [SerializeField]
    public float distanceToShoot;

    [SerializeField]
    public bool canShoot;

    [SerializeField]
    public float attackCooldown;

    [SerializeField]
    public SkinnedMeshRenderer meshRenderer;

    [SerializeField]
    public ParticleSystem deathEffects;

    [SerializeField]
    public Transform bulletPoint;

    [SerializeField]
    public ParticleSystem muzzle;

    [SerializeField]
    public GameObject bullet;

    [SerializeField]
    public float bulletSpeed = 25f;

    [SerializeField]
    public float bulletMaxDistance = 40f;

    [SerializeField]
    public int bulletsToFire = 1;

    [SerializeField]
    public float bulletAngle = 45f;

    [SerializeField]
    public float dashLookTime = 4f;

    [SerializeField]
    public float dashSpeed = 9f;

    [SerializeField]
    public float dashTime;

    [SerializeField]
    public bool canDropHealth;

    [SerializeField]
    public float lifeToAdd;

    [SerializeField]
    public GameObject health;

    [SerializeField]
    public float dashCooldownTime;

    [SerializeField]
    public int mainSkinIndex = 0;

    public float lookSpeed = 1f;
    public bool canAttack = true;
    public float originalLife;

    [SerializeField]
    public bool canDash = false;
    [SerializeField]
    public bool dashForward;

    [SerializeField]
    public ParticleSystem dashEffects;

    public bool isDashing = false;

    public Animator animator;

    public Material enemySkin;

    public bool isExploding = false;

    private Vector3 velocity;

    private int speedId;

    public Color originalColor;
    public void Start()
    {
        animator = GetComponent<Animator>();
        enemySkin = meshRenderer.materials[mainSkinIndex];
        originalColor = new Color { r = enemySkin.color.r, g = enemySkin.color.g, b = enemySkin.color.b };
        originalLife = life;
        velocity = new Vector3(0, 0, 0);
        speedId = Animator.StringToHash("Speed");
    }
    void Update()
    {
        if (life <= 0)
        {
            OnDeath();
        }

        if (!isDashing)
        {
            Follow();
        }

        var lifePercentage = life / originalLife;

        healthIndicator.localScale = new Vector3(lifePercentage, 1.05f, 1.05f);
        healthIndicator.localPosition = new Vector3(((lifePercentage) - 1) / 2, 0, 0);

        var enemySpeed = canShoot && Vector3.Distance(follow.position, transform.position) <= distanceToShoot ? 0 : speed;
        enemySpeed = isDashing && !canDash ? dashSpeed : enemySpeed;
        enemySpeed = dashForward && !isDashing ? 0 : enemySpeed;

        enemySpeed = isExploding ? 0 : enemySpeed;

        animator.SetFloat(speedId, velocity.magnitude * enemySpeed);

        transform.Translate(velocity * enemySpeed * Time.deltaTime);


        if (canShoot && canAttack)
        {
            if (Vector3.Distance(follow.position, transform.position) <= distanceToShoot)
            {
                animator.Play("Attack");
                StartCoroutine("Attack");

                Shoot();
            }
        }

        if (canDash)
        {
            StartCoroutine("Dash");
        }
    }

    public virtual void OnDeath()
    {
        Instantiate(deathEffects, transform.position, Quaternion.identity);

        if (canDropHealth)
        {
            var healthObject = Instantiate(health, transform.position, Quaternion.identity);

            healthObject.GetComponent<HealthPowerup>().lifeToAdd = lifeToAdd;
        }

        Destroy(gameObject);
    }

    public void TakeDamageEffect()
    {
        StartCoroutine("TakeDamage");
    }

    private IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashLookTime);
        isDashing = true;
        canDash = false;
        dashEffects.Play();
        yield return new WaitForSeconds(dashTime);
        dashEffects.Stop();
        isDashing = false;
        yield return new WaitForSeconds(dashCooldownTime);
        canDash = true;
    }

    private void Shoot()
    {
        if (bulletsToFire == 1)
        {
            var enemyBullet = Instantiate(bullet, bulletPoint.position, Quaternion.identity);

            var bulletScript = enemyBullet.GetComponent<Bullet>();

            bulletScript.velocity = bulletPoint.transform.forward;
            bulletScript.damage = damage;
            bulletScript.speed = bulletSpeed;
            bulletScript.maxDistance = bulletMaxDistance;
        }
        else
        {
            float angle = Mathf.Atan2(bulletPoint.transform.forward.z, bulletPoint.transform.forward.x) * Mathf.Rad2Deg;
            float angleStart = angle - (bulletAngle / Random.Range(2f, 3f));
            float angleIncrement = bulletAngle / bulletsToFire;
            for (int i = 0; i < bulletsToFire; i++)
            {
                var enemyBullet = Instantiate(bullet, bulletPoint.position, Quaternion.identity);

                var bulletScript = enemyBullet.GetComponent<Bullet>();

                float a = angleStart * Mathf.Deg2Rad;
                var bulletVelocity = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));

                bulletScript.velocity = bulletVelocity;
                bulletScript.damage = damage;
                bulletScript.speed = bulletSpeed;
                bulletScript.maxDistance = bulletMaxDistance;

                angleStart += angleIncrement;
            }
        }
        muzzle.Play();
    }

    private IEnumerator TakeDamage()
    {
        enemySkin.color = new Color { r = 150, g = 150, b = 150 };
        yield return new WaitForSeconds(0.1f);
        enemySkin.color = originalColor;
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void Follow()
    {
        var targetPosX = follow.position.x - transform.position.x;
        var targetPosZ = follow.position.z - transform.position.z;

        float angle = Mathf.Atan2(targetPosZ, targetPosX) * Mathf.Rad2Deg;

        var toLookAt = Quaternion.Euler(new Vector3(0, (-angle + 90), 0));

        transform.rotation = Quaternion.Slerp(transform.rotation, toLookAt, Time.deltaTime * lookSpeed);

        var dir = (Vector2)(Quaternion.Euler(0, 0, (-transform.rotation.y + 90)) * Vector2.right);

        velocity = new Vector3(dir.x, 0, dir.y);

        velocity.Normalize();
    }


    public virtual void OnCollisionEnter(Collision obj)
    {
        if (canAttack && obj.gameObject.tag == "Player")
        {
            animator.Play("Attack");
            StartCoroutine("Attack");

            var playerScript = obj.gameObject.GetComponent<PlayerController>();
            playerScript.TakeDamageEffect();
            playerScript.life -= damage;
        }

        // Debug.Log(isDashing && obj.gameObject.tag == "Environment");

        if (isDashing && obj.gameObject.tag == "Blocker")
        {

            // isDashing = false;

            // Debug.Log(isDashing);
            velocity = Vector3.zero;

            dashEffects.Stop();
        }
    }
}
