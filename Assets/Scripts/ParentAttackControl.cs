using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentAttackControl : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    private HealthControl hc;
    private Transform AttackSpot1;
    private Transform AttackSpot2;
    private float attackRange;
    private float attackCooldown;
    public LayerMask enemyLayer;
    private GameObject opponent;
    private CharStats stat;

    public bool Attacking { get; set; }

    void Start()
    {
        AttackSpot1 = transform.GetChild(0);
        AttackSpot2 = transform.GetChild(1);
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        hc = GetComponent<HealthControl>();
        stat = GetComponent<CharStats>();
        attackRange = stat.AttackRange;
        attackCooldown = stat.AttackCooldown;
    }
    
    private void FixedUpdate()
    {
        if (attackCooldown > 0f)
        { attackCooldown -= Time.deltaTime; }
        else { Attacking = false; }
    }

    public void Attack(float cooldown, float stamDrain)
    {
        if (!Attacking && hc.Stamina > stamDrain)
        {
            Attacking = true;
            attackCooldown = cooldown;
            anim.SetTrigger("Attack");
            hc.StaminaDrain(stamDrain);
        }
    }

    public void TriggerAttack()
    {
        Transform hitSpot;
        if (sr.flipX)
        {
            hitSpot = AttackSpot1;
        }
        else
        {
            hitSpot = AttackSpot2;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitSpot.position, attackRange, enemyLayer);

        int numHit = 0;
        numHit = Mathf.Clamp(numHit + hitEnemies.Length, 0, 3);

        for (int i = 0; i < numHit; i++)
        {
            hitEnemies[i].GetComponent<HealthControl>().TakeDamage(stat.DamageStat);
        }
    }

    public void gotHit()
    {
        attackCooldown = 0;
    }

    //public void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireSphere(AttackSpot1.position, attackRange);
    //    Gizmos.DrawWireSphere(AttackSpot2.position, attackRange);
    //}
}
