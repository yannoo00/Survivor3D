using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustExplosion : MonoBehaviour
{
    private int damage = 400;
    public LayerMask whatIsTarget;
    // Start is called before the first frame update
    void Start()
    {
        Collider[] enemyColliders = 
        Physics.OverlapSphere(transform.position,6f,whatIsTarget);

        for(int i=0; i<enemyColliders.Length; i++)
        {
            LivingEntity livingEntity = enemyColliders[i].GetComponent<LivingEntity>();
            

            if(livingEntity!=null)
            {
                livingEntity.OnDamage(damage);
            }
        }

        Destroy(gameObject,1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
