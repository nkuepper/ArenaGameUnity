using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePillar : MonoBehaviour
{
    private ParticleSystem ps;
    private Animator anim;
    private int condition = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    public void Break()
    {
        if (condition != 3)
        {
            ps.Play();
            condition += 1;
        }
        anim.SetInteger("PillarCondition", condition);

        if (condition == 3)
        {
            if (GetComponent<Collider2D>().enabled)
                GetComponent<Collider2D>().enabled = false;
        }
    }
}
