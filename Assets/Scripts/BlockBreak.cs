using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak : MonoBehaviour
{
    private ParticleSystem ps;

    private void Start()
    {
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void OnDestroy()
    {
        if (GetComponent<BoxCollider2D>()) 
           Destroy(GetComponent<BoxCollider2D>());
        ps.Play();
    }
}
