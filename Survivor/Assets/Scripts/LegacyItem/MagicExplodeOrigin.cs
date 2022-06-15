using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicExplodeOrigin : MonoBehaviour
{
    private AudioSource audioSource;
    public event Action off;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
       
    }

    private void OnEnable()
    {
        StartCoroutine(Explode());        
        
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        
        // Collider[] enemyColliders = 
        // Physics.OverlapSphere(transform.position,3f);

        // for(int i=0; i<enemyColliders.Length; i++)
        // {
        //     LivingEntity livingEntity = enemyColliders[i].GetComponent<LivingEntity>();

        //     if(livingEntity!=null&&livingEntity.tag =="Enemy")
        //     {
        //         livingEntity.OnDamage(120);
        //     }
        // }
    }

    void Update()
    {
        
    }

    private IEnumerator Explode()
    {



        yield return new WaitForSeconds(0.8f);
        if(off!=null)
            off();
    }
}
