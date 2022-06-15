using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InfernoProjectile : MonoBehaviour
{
    public event Action off;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag =="Enemy"|| other.tag=="Boundary")
        {
            LivingEntity enemyEntity = other.GetComponent<LivingEntity>();

            if(enemyEntity!=null&&!enemyEntity.dead)
            {
                enemyEntity.OnDamage(32);   
            }

            Die();
        }

        // else if(other.tag =="Boundary")
        //     Die();

        // if(off!=null)
        //     off();
    }    



    public virtual void Die()
    {
        if(off!=null)
            off();
    }
    
    private IEnumerator Off()
    {
        yield return new WaitForSeconds(1.75f);
        if(off!=null)
            off();
    }



    void OnEnable()
    {
        //StartCoroutine(Off());
    }



    void Start()
    {
       
    }




    void Update()
    {
        
    }
}
