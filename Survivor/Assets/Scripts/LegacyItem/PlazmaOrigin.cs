using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazmaOrigin : MonoBehaviour
{
    
    public float damage;
    public float boundary;
    
    void Start()
    {
        Collider[] enemyColliders = 
        Physics.OverlapSphere(transform.position,boundary);

        for(int i=0; i<enemyColliders.Length; i++)
        {
            LivingEntity livingEntity = enemyColliders[i].GetComponent<LivingEntity>();

            if(livingEntity!=null&&livingEntity.tag =="Enemy")
            {
                livingEntity.OnDamage(damage);
            }
        }

        Destroy(gameObject,1f);
    }

    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        //StartCoroutine(bomb(other));
    }



    private IEnumerator bomb(Collider other)
    {   
        if(other.tag == "Enemy")
        {
            
            IDamageable target = other.GetComponent<IDamageable>();

            if(target != null)
                target.OnDamage(damage);
        }        

        yield return new WaitForSeconds(1f);
    }
}
