using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    //protected SpriteRenderer sr;
    //protected Animator anim;
    //protected HealthControl hc;
    //protected Transform AttackSpot1;
    //protected Transform AttackSpot2;
    //protected float attackRange = 0.5f;
    //protected float attackCooldown = 0f;
    //public LayerMask enemyLayer;
    //GameObject opponent;
    //private float damage = 10f;

    //public bool attacking = false;

    //void Start()
    //{
    //    AttackSpot1 = transform.GetChild(1);
    //    AttackSpot2 = transform.GetChild(2);
    //    anim = GetComponent<Animator>();
    //    sr = GetComponent<SpriteRenderer>();
    //    hc = GetComponent<HealthControl>();
    //}
    
    //protected virtual void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //         Attack();
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    if (attackCooldown > 0f)
    //    { attackCooldown -= Time.deltaTime; }
    //    else { attacking = false; }
    //}

    //public virtual void Attack()
    //{
    //    if (!attacking && hc.Stamina > 60)
    //    {
    //        attacking = true;
    //        attackCooldown = 0.8f;
    //        anim.SetTrigger("Attack");
    //        hc.StaminaDrain(60f);
    //    }
    //}

    //public virtual void TriggerAttack()
    //{
    //    Transform tempspot;
    //    if (sr.flipX)
    //    {
    //        tempspot = AttackSpot1;
    //    }
    //    else
    //    {
    //        tempspot = AttackSpot2;
    //    }

    //    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(tempspot.position, attackRange, enemyLayer);

    //    int numHit = 0;
    //    numHit = Mathf.Clamp(numHit + hitEnemies.Length, 0, 3);

    //    for (int i = 0; i < numHit; i++)
    //    {
    //        hitEnemies[i].GetComponent<HealthControl>().TakeDamage(damage);
    //    }
    //}

    //public void gotHit()
    //{
    //    attackCooldown = 0;
    //}

    //public void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireSphere(AttackSpot1.position, attackRange);
    //    Gizmos.DrawWireSphere(AttackSpot2.position, attackRange);
    //}
}
