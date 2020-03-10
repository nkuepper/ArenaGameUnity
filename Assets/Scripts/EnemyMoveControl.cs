using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMoveControl : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private HealthControl hc;
    private ParentAttackControl ac;
    private CharStats stat;
    private AIPath pathing;
    private AIDestinationSetter pathDest;
    private float close = 2f;
    private float vision = 7f;
    private Transform target;
    private float attention = 0f;
    private Vector2 tempDest;

    private void Awake()
    {
        stat = GetComponent<CharStats>();
        stat.AttackCooldown = 1.1f;
    }

    private void Start()
    {
        target = GameObject.Find("Player").transform;

        pathing = GetComponent<AIPath>();
        pathDest = GetComponent<AIDestinationSetter>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        hc = GetComponent<HealthControl>();
        ac = GetComponent<ParentAttackControl>();

        pathDest.target = target;
    }

    private void Update()
    {
        // Idle
        anim.SetInteger("AnimState", 0);

        if (!ac.Attacking)
        {
            hc.StaminaRegen = stat.StamRegenStat * 0.7f;
        }
        else
        {
            hc.StaminaRegen = stat.StamRegenStat * 0f;
        }

        float distance = Vector2.Distance(target.position, transform.position);

        // Within vision
        if (distance <= vision)
        {
            if (!ac.Attacking)
            {
                if (distance > close)
                {
                    // Within vision but not close
                    anim.SetInteger("AnimState", 2);
                    anim.SetFloat("SpeedMultiplier", 0.9f);
                    
                    pathing.canSearch = true;
                    pathing.canMove = true;
                }
                else
                {
                    // Close
                    ac.Attack(stat.AttackCooldown, stat.StamDrain);
                }
            }
            else
            {
                // Attacking
                pathing.canMove = false;
                pathing.canSearch = false;
            }

            attention = 5f;
            FaceTarget();
        }
        else
        {
            pathing.canSearch = false;
            pathing.canMove = false;

            if (attention > 0f)
            {
                anim.SetInteger("AnimState", 1);
            }
        }

        attention -= Time.deltaTime;
    }

    private void FaceTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        if (direction.x > 0)
        {
            sr.flipX = true;
        }
        else if (direction.x < 0)
        {
            sr.flipX = false;
        }
    }
}
