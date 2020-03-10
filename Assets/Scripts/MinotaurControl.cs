using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using Pathfinding;

public class MinotaurControl : MonoBehaviour
{
    #region Declarations
    #region Declarations_Variables
    private CharStats stat;
    private Transform victim;
    private SpriteRenderer sr;
    private HealthControl hc;
    private float close = 5f;
    private bool attacking = false;
    private float abilityCooldown = 3f;
    private float newAbilityCooldown = 20f;
    private float pillarBreakCooldown = 0f;
    private float stunCooldown = 0f;
    private Vector2 pos;
    private float health;
    private float stam;
    private Transform hitSpot;
    private GameObject particleManager;
    private AstarPath pathfinding;
    private AIPath pathing;
    private Camera myCamera;
    private bool stunned = false;
    #endregion

    #region Declarations_AttackHitDetection
    public LayerMask enemyLayer;
    public float attackRange = 0.5f;
    public float lowAttackRange = 0.5f;
    public float highAttackRange = 0.5f;
    public Transform AttackSpot1;
    public Transform AttackSpot2;
    public Transform LowSpot;
    public Transform HighSpot;
    #endregion
    #endregion

    private void Awake()
    {
        myCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        myCamera.GetComponent<CameraFollow>().enabled = false;
        myCamera.GetComponent<CameraShaker>().enabled = true;
        particleManager = transform.GetChild(3).gameObject;
        stat = GetComponent<CharStats>();
        stat.HealthStat = 200f;
        stat.DamageStat = 3f;
        stat.StamStat = 900f;
        stat.StamRegenStat = 5f;
        stat.SpeedStat = 8f;
        stat.ExpWorth = 1000f;
    }

    private void Start()
    {
        victim = GameObject.Find("Player").transform;

        sr = GetComponent<SpriteRenderer>();
        hc = GetComponent<HealthControl>();
        pathing = GetComponent<AIPath>();
        pathfinding = GameObject.Find("Pathfinding").GetComponent<AstarPath>();
        
    }

    void Update()
    {
        float distance = Vector2.Distance(victim.position, transform.position);
        
        if (distance > close && !attacking)
        {
            // Move closer TODO: Animate movement
            // Handled by pathfinding
               // transform.position = Vector2.MoveTowards(transform.position, victim.position, stat.SpeedStat / 3 * Time.deltaTime);
        }
        else
        {
            // Attack
            if (victim.position.y + 2.5f < transform.position.y)
            {
                // Sweeping attack
                hitSpot = LowSpot;
            }
            else if (victim.position.y + 0.5f > transform.position.y)
            {
                // Upwards attack
                hitSpot = HighSpot;
            }
            else
            {
                // Bash
                if (sr.flipX)
                {
                    hitSpot = AttackSpot1;
                }
                else
                {
                    hitSpot = AttackSpot2;
                }
            }
        }

        FaceTarget();
    }

    private void FixedUpdate()
    {
        // Idle
        //anim.SetInteger("AnimState", 0);

        if (!attacking)
        {
            hc.StaminaRegen = stat.StamRegenStat * 0.7f;
        }
        else
        {
            hc.StaminaRegen = stat.StamRegenStat * 0f;
        }

        // Charging Motion
        if (abilityCooldown > newAbilityCooldown && !stunned)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos, stat.SpeedStat * 2 * Time.deltaTime);
            hitSpot = LowSpot;
            BullAttack();
            CameraShaker.Instance.ShakeOnce(1f, 3f, 0.1f, 3f);
        }

        if (abilityCooldown <= 0f)
        {
            // Ability

            // Target player position and charge
            // Play start-up animation, then trigger charge
            //anim.settrigger("Charge");
            Charge();
        }

        if (stunCooldown <= 0f)
        {
            pos = victim.position;
            pathing.canMove = true;
            stunned = false;
        }

        if (!stunned)
            abilityCooldown -= Time.deltaTime;

        pillarBreakCooldown -= Time.deltaTime;
        stunCooldown -= Time.deltaTime;
    }

    private void Charge()
    {
        pos = victim.position;
        abilityCooldown = Random.Range(2f, 5f);
        newAbilityCooldown = abilityCooldown - 0.5f;
    }

    public void BullAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitSpot.position, attackRange, enemyLayer);

        int numHit = 0;
        numHit = Mathf.Clamp(numHit + hitEnemies.Length, 0, 3);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<HealthControl>())
            {
                enemy.GetComponent<HealthControl>().TakeDamage(stat.DamageStat);
            }
            else if (enemy.GetComponent<BlockBreak>())
            {
                Destroy(enemy.GetComponent<SpriteRenderer>());
                Destroy(enemy.GetComponent<BlockBreak>());
                Destroy(enemy);
                pathfinding.Scan(pathfinding.graphs[0]);
            }
            else if(enemy.GetComponent<DamagePillar>())
            {
                if (enemy.GetComponent<Collider2D>().enabled && pillarBreakCooldown <= 0f)
                {
                    pillarBreakCooldown = 7f;
                    enemy.GetComponent<DamagePillar>().Break();
                    Stun();
                }
            }
        }
    }

    private void Stun()
    {
        // Stun minotaur
        stunCooldown = 5f;
        stunned = true;
        pathing.canMove = false;

        abilityCooldown = 6f;
        newAbilityCooldown = abilityCooldown - 0.5f;
    }

    private void FaceTarget()
    {
        Vector2 direction = (victim.position - transform.position).normalized;
        if (direction.x > 0)
        {
            sr.flipX = true;
        }
        else if (direction.x < 0)
        {
            sr.flipX = false;
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(AttackSpot1.position, attackRange);
        Gizmos.DrawWireSphere(AttackSpot2.position, attackRange);
        Gizmos.DrawWireSphere(LowSpot.position, lowAttackRange);
        Gizmos.DrawWireSphere(HighSpot.position, highAttackRange);
    }

    private void OnDestroy()
    {
        if (myCamera != null)
        {
            myCamera.GetComponent<CameraShaker>().enabled = false;
            myCamera.GetComponent<CameraFollow>().enabled = true;
            myCamera.transform.localRotation = new Quaternion(0,0,0,0);
        }
    }
}
