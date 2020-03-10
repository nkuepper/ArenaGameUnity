using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour
{
    private Animator anim;
    private Image HealthBar;
    private Image StamBar;
    private CharStats stat;
    private bool dead = false;
    private GameObject particleManager;

    public float Health { get; set; }
    public float Stamina { get; set; }
    public float StaminaRegen { get; set; }

    private void Start()
    {
        particleManager = transform.GetChild(3).gameObject;
        stat = GetComponent<CharStats>();
        Stamina = stat.StamStat;
        Health = stat.HealthStat;
        anim = GetComponent<Animator>();
        HealthBar = transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>();
        StamBar = transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        Stamina = Mathf.Clamp(Stamina + StaminaRegen, 0, stat.StamStat);
        StamBar.fillAmount = Stamina / stat.StamStat;

        if (Health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        if (!dead)
        {
            Health = Mathf.Clamp(Health - amount, 0, stat.HealthStat);
            HealthBar.fillAmount = Health / stat.HealthStat;
            anim.SetTrigger("Hurt");
        }
    }

    public void Bleed()
    {
        //ParticleSystem blood1 = particleManager.transform.GetChild(0).GetComponent<ParticleSystem>();
        //blood1.Play();
    }

    public void StaminaDrain(float amount)
    {
        Stamina = Mathf.Clamp(Stamina - amount, 0, stat.StamStat);
        StamBar.fillAmount = Stamina / stat.StamStat;
    }

    public void Die()
    {
        anim.SetTrigger("Death");
        if (GetComponent<PlayerControl>())
            GetComponent<PlayerControl>().enabled = false;
        if (GetComponent<EnemyMoveControl>())
            GetComponent<EnemyMoveControl>().enabled = false;
        if (GetComponent<MinotaurControl>())
            Destroy(GetComponent<MinotaurControl>());
        if (GetComponent<AIPath>())
            GetComponent<AIPath>().enabled = false;
        GetComponent<ParentAttackControl>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        if (GetComponent<Rigidbody2D>())
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        dead = true;
        enabled = false;

        if (gameObject.name == "Player")
        {
            // Game Over
            particleManager.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
            particleManager.transform.GetChild(0).transform.localPosition += Vector3.down;
        }
        else
        {
            GameObject.Find("Player").GetComponent<CharStats>().GiveExp(stat.ExpWorth);

            // Destroy Children
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name != "ParticleManager")
                    { Destroy(transform.GetChild(i).gameObject); }
                else
                {
                    transform.GetChild(i).transform.localPosition += Vector3.down;
                }
            }
        }
    }
}
