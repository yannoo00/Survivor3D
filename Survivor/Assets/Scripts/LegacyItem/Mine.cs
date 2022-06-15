using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Mine : MonoBehaviour
{
    public event Action off;
    public LayerMask whatIsTarget;
    public ParticleSystem mineExplosion;
    //AudioSource audioSource;
    public int damage = 150;
    public int lastTime =15;
    void Start()
    {

    }

    private void OnEnable() 
    {
        StartCoroutine(Die());       
        //audioSource = GetComponent<AudioSource>(); 
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Enemy")
        {
            //other.GetComponent<LivingEntity>().OnDamage(damage);
            Bomb();
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(lastTime);
        Bomb();
    }

    private void Bomb()
    {
        ParticleSystem explosion = Instantiate(mineExplosion, gameObject.transform.position,Quaternion.identity);
        explosion.Play();

        //audioSource.Play()

        Collider[] colliders = Physics.OverlapSphere(transform.position,4.5f,whatIsTarget);
        if(colliders.Length>0)
        {
            for(int i=0; i<colliders.Length;i++)
            {
                LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                livingEntity.OnDamage(damage);                
            }
        }   

        off();

    }

}
