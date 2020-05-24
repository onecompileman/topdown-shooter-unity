using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float rollingSpeed = 15f;

    [SerializeField]
    public float life = 100f;

    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;

    [SerializeField]
    public Joystick leftJoystick;

    [SerializeField]
    public Joystick shootJoystick;

    [SerializeField]
    public bool isMobile = true;

    [SerializeField]
    public GameObject stun;

    [SerializeField]
    public GameObject frozen;

    [SerializeField]
    public float mana;

    [SerializeField]
    public float manaRegenTime;

    [SerializeField]
    public bool isLobbyMode;

    public bool isStunned = false;

    public bool isFrozen = false;

    private Material playerSkin;

    private Color originalColor;

    public float originalLife;

    public float originalMana;

    public Quaternion rotationToFollow;

    public bool hasFullyRotated = true;

    public int gems = 0;

    public int coins = 0;
    private float rollCooldown = 0.4035f;

    private bool isRolling = false;


    private Vector3 velocity = Vector3.zero;

    private Rigidbody rb;
    private Animator animator;

    private int speedId;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        originalLife = life;
        originalMana = mana;
        playerSkin = meshRenderer.materials[2];
        originalColor = new Color { r = playerSkin.color.r, g = playerSkin.color.g, b = playerSkin.color.b };
        speedId = Animator.StringToHash("Speed");

        StartCoroutine("RegenMana");
    }

    void Update()
    {
        var xVel = isMobile ? leftJoystick.Horizontal : Input.GetAxis("Horizontal");
        var yVel = isMobile ? leftJoystick.Vertical : Input.GetAxis("Vertical");

        if (!isRolling && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine("Roll");
        }

        velocity = new Vector3(xVel, 0, yVel);
        velocity.Normalize();
        float angle = 0;
        if (velocity.magnitude > 0)
        {
            angle = Mathf.Atan2(yVel, xVel) * Mathf.Rad2Deg;
            rotationToFollow = Quaternion.Euler(new Vector3(0, (-angle + 90), 0));
            transform.rotation = Quaternion.Euler(new Vector3(0, (-angle + 90), 0));
        }
        if (shootJoystick != null)
        {
            if (!isRolling && Math.Abs(shootJoystick.Vertical) > 0.2 || Math.Abs(shootJoystick.Horizontal) > 0.2)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 5.23f;

                Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
                mousePos.x = mousePos.x - objectPos.x;
                mousePos.y = mousePos.y - objectPos.y;

                angle = Mathf.Atan2(isMobile ? shootJoystick.Vertical : mousePos.y, isMobile ? shootJoystick.Horizontal : mousePos.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, (-angle + 90), 0));

            }
        }

        var movespeed = isRolling ? rollingSpeed : speed;

        movespeed = isStunned || isFrozen ? 0 : movespeed;

        // Debug.Log(velocity.magnitude * movespeed);
        animator.SetFloat(speedId, velocity.magnitude == 0 ? 0 : movespeed);

        var vel = isRolling ? transform.forward : velocity;

        if (isRolling && !animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            animator.Play("Roll");
        }

        // transform.rotation = Quaternion.Slerp(transform.rotation, rotationToFollow, Time.deltaTime * 5f);

        // hasFullyRotated = (Vector3.Distance(rotationToFollow.eulerAngles, transform.rotation.eulerAngles) < 0.1f);

        transform.position = Vector3.Lerp(transform.position, transform.position + (vel * movespeed), Time.deltaTime);
    }

    void FixedUpdate()
    {

    }

    public void TakeDamageEffect()
    {
        StartCoroutine("TakeDamage");
    }

    public void TakeMana(float m)
    {
        mana = Mathf.Clamp(mana + m, 0, originalMana);
        StartCoroutine("TakeManaEffects");
    }

    public void TakeLifeEffect(float l)
    {
        life = Mathf.Clamp(life + l, 0, originalLife);
        StartCoroutine("TakeLife");
    }

    public void Freeze(float cooldown)
    {
        StartCoroutine("FreezePlayer", cooldown);
    }

    public void StunPlayer(float cooldown)
    {
        StartCoroutine("StunPlayerEffect", cooldown);
    }

    IEnumerator TakeManaEffects()
    {
        playerSkin.color = new Color(0.08f, 1, 1);
        yield return new WaitForSeconds(0.2f);
        playerSkin.color = originalColor;
    }

    IEnumerator RegenMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(manaRegenTime);
            mana = Mathf.Clamp(mana + 1, 0, originalMana);
        }
    }

    IEnumerator FreezePlayer(float cooldown)
    {
        isFrozen = true;
        frozen.SetActive(true);
        frozen.transform.localScale = new Vector3(400, 400, 400);

        var scale = 400;
        float secondsToWait = 0.6f / 125;

        for (int i = 0; i < 125; i++)
        {
            scale += 2;
            frozen.transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForSeconds(secondsToWait);
        }
        yield return new WaitForSeconds(cooldown);
        isFrozen = false;
        frozen.SetActive(false);
    }

    private IEnumerator StunPlayerEffect(float cooldown)
    {
        isStunned = true;
        stun.SetActive(true);
        yield return new WaitForSeconds(cooldown);
        stun.SetActive(false);
        isStunned = false;
    }

    private IEnumerator TakeDamage()
    {
        playerSkin.color = new Color(150, 150, 150);
        yield return new WaitForSeconds(0.2f);
        playerSkin.color = originalColor;
    }

    private IEnumerator TakeLife()
    {
        playerSkin.color = new Color(0.3f, 1, 0.3f);
        yield return new WaitForSeconds(0.2f);
        playerSkin.color = originalColor;
    }


    private IEnumerator Roll()
    {
        isRolling = true;
        yield return new WaitForSeconds(rollCooldown);
        isRolling = false;
    }
}