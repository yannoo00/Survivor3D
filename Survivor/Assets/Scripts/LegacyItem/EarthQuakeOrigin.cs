using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EarthQuakeOrigin : MonoBehaviour
{
    public event Action off;
    public int damage;

    //public BoxCollider[] colliders;
    public BoxCollider collide;

    

    void Start()
    {
        
    }
    void OnEnable()
    {
        StartCoroutine(Finish());
        StartCoroutine(Collide());
    }

    
    void Update()
    {
        
    }

    public virtual void Die()
    {
        if(off!=null)
            off();
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(3);
        Die();
    }

    private IEnumerator Collide()
    {
        collide.enabled=true;
        yield return new WaitForSeconds(0.5f);        
        collide.enabled=false;
    }

    private void OnTriggerEnter(Collider other) {

        if(other.tag == "Enemy")
        {
            if(other.GetComponent<LivingEntity>()!=null)
                other.GetComponent<LivingEntity>().OnDamage(damage);
            
        }
    }


}
