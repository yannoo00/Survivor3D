using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anubis : MonoBehaviour,IItemDrop
{
   
    public LayerMask whatIsTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 60* Time.deltaTime, 0f);
    }

    public void Use(GameObject target)
    {
        Collider[] enemyColliders =
        Physics.OverlapSphere(transform.position,12f,whatIsTarget);

        for(int i=0; i < enemyColliders.Length; i++)
        {
            LivingEntity livingEntity = enemyColliders[i].GetComponent<LivingEntity>();
            if(livingEntity != null)
            {
                livingEntity.OnDamage(444);
            }
        }        


        Destroy(gameObject);
    }

}
