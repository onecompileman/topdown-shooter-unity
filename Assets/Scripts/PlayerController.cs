using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Material playerSkin;

    private Color originalColor;

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
        playerSkin = meshRenderer.materials[2];
        originalColor = new Color { r = playerSkin.color.r, g = playerSkin.color.g, b = playerSkin.color.b };
        speedId = Animator.StringToHash("Speed");
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
            transform.rotation = Quaternion.Euler(new Vector3(0, (-angle + 90), 0));
        }
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

        var movespeed = isRolling ? rollingSpeed : speed;

        // Debug.Log(velocity.magnitude * movespeed);
        animator.SetFloat(speedId, velocity.magnitude == 0 ? 0 : movespeed);

        var vel = isRolling ? transform.forward : velocity;

        if (isRolling && !animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            animator.Play("Roll");
        }

        transform.position = Vector3.Lerp(transform.position, transform.position + (vel * movespeed), Time.deltaTime);
    }

    void FixedUpdate()
    {

    }

    public void TakeDamageEffect()
    {
        StartCoroutine("TakeDamage");
    }

    public void TakeLifeEffect(float life)
    {
        life = Mathf.Clamp(life, 0, 100);
        StartCoroutine("TakeLife");
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