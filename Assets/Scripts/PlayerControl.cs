using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public enum speedEnum { walk, jog, run };

    private float jogSpeed = .6f;
    private float runSpeed = 1f;
    private float walkSpeed = .3f;
    private float dashSpeed = 5f;
    private float dashCooldown = 0f;
    private float jumpCooldown = 0f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private HealthControl hc;
    private ParentAttackControl ac;
    private CharStats stat;
    private float tempSpeed;

    private DamagePillar dptest;

    public speedEnum Speed { get; set; }
    public Vector2 Movement { get; set; }

    private void Awake()
    {
        stat = GetComponent<CharStats>();
        stat.AttackCooldown = 0.8f;
        stat.DamageStat = 2f; // Change with weapon + stats
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        hc = GetComponent<HealthControl>();
        ac = GetComponent<ParentAttackControl>();

        dptest = GameObject.Find("Pillar_0").GetComponent<DamagePillar>();
        stat.HealthStat = 10000;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            stat.LevelUp();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            dptest.Break();
        }

        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Walk and Run
        if (Input.GetKey(KeyCode.LeftShift)) { Speed = speedEnum.run; }
        else if (Input.GetKey(KeyCode.LeftControl)) { Speed = speedEnum.walk; }
        else { Speed = speedEnum.jog; }
        
        // Attack
        if (Input.GetMouseButtonDown(0))
        {
            ac.Attack(stat.AttackCooldown, stat.StamDrain);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        Move();

        // Movement
        if (movement.magnitude > Mathf.Epsilon)
        {
            // Appearance
            anim.SetInteger("AnimState", 2);

            if (movement.x > 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
        else
        {
            // Idle
            anim.SetInteger("AnimState", 0);

            if (!ac.Attacking)
            {
                hc.StaminaRegen = stat.mStamRegenStat;
            }
            else
            {
                hc.StaminaRegen = stat.mStamRegenStat * 0f;
            }
        }

        //if (jumpCooldown > .26f)
        //{
        //    rb.MovePosition(rb.position + Vector2.up * stat.SpeedStat * Time.fixedDeltaTime);
        //}
        //else if (jumpCooldown > 0f)
        //{
        //    rb.MovePosition(rb.position + Vector2.down * stat.SpeedStat * Time.fixedDeltaTime);
        //}

        //if (jumpCooldown < 0)
        //{
        //    anim.SetBool("Grounded", true);
        //}

        dashCooldown -= Time.deltaTime;
        //jumpCooldown -= Time.deltaTime;
    }

    private void Move()
    {
        if (!ac.Attacking)
        {
            // Math
            if (hc.Stamina < 0.5) { Speed = speedEnum.walk; }
            switch (Speed)
            {
                case speedEnum.run:
                    tempSpeed = stat.SpeedStat * runSpeed;
                    hc.StaminaRegen = stat.StamRegenStat * -1f;
                    anim.SetFloat("SpeedMultiplier", 1.2f);
                    break;

                case speedEnum.walk:
                    tempSpeed = stat.SpeedStat * walkSpeed;
                    hc.StaminaRegen = stat.StamRegenStat * 0.6f;
                    anim.SetFloat("SpeedMultiplier", 0.6f);
                    break;

                default:
                    tempSpeed = stat.SpeedStat * jogSpeed;
                    hc.StaminaRegen = stat.StamRegenStat * 0.2f;
                    anim.SetFloat("SpeedMultiplier", 0.9f);
                    break;
            }

            if (dashCooldown > 1.9)
            {
                rb.velocity = movement * stat.SpeedStat * dashSpeed;
                //rb.MovePosition(rb.position + movement.normalized * stat.SpeedStat * dashSpeed * Time.fixedDeltaTime);
            }
            else
            {
                rb.velocity = movement * tempSpeed;
                //rb.MovePosition(rb.position + movement.normalized * tempSpeed * Time.fixedDeltaTime);
            }

            // Dash
            if (Input.GetMouseButton(1))
            {
                Dash();
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Dash()
    {
        if (hc.Stamina > 50 && dashCooldown < 0)
        {
            dashCooldown = 2f;
            hc.StaminaDrain(stat.DashStamDrain);
        }
    }

    private void Jump()
    {
        if (hc.Stamina > 20 && jumpCooldown < 0)
        {
            jumpCooldown = 0.5f;
            hc.StaminaDrain(20f); // Stats
            anim.SetBool("Grounded", false);
            anim.SetTrigger("Jump");
        }
    }
}
