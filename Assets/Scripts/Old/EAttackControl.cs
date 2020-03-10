using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EAttackControl : AttackControl
{
    //public override void Attack()
    //{
    //    if (!attacking && hc.Stamina > 40)
    //    {
    //        attacking = true;
    //        attackCooldown = 1.1f;
    //        anim.SetTrigger("Attack");
    //        hc.StaminaDrain(40f);
    //    }
    //}

    //public override void TriggerAttack() { }
    //protected override void Update() { }


    //public void ETriggerAttack()
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

    //    foreach (Collider2D enemy in hitEnemies)
    //    {
    //        enemy.GetComponent<HealthControl>().TakeDamage(1f);
    //    }
    //}
}
