using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoonLightOrigin : MonoBehaviour
{
    public event Action off;
    void Start()
    {
        
    }

    void OnEnable()
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        StartCoroutine(Die());
        particle.Play();
    }

    void Update()
    {
        
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.4f);
        off();
    }

}
