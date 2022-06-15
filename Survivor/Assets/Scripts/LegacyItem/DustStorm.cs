using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustStorm : MonoBehaviour
{

    private float lastAttackTime; //마지막 공격 시점
    private float timeBetAttack = 0.2f; //공격 간격
    private int damage = 5;

    private void OnTriggerStay(Collider other)
    {
        if(Time.time >= lastAttackTime+timeBetAttack && other.tag =="Enemy")
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if(attackTarget != null)
            {
                lastAttackTime = Time.time;

                attackTarget.OnDamage(damage);
            }
        }
    }

    
    void Start()
    {
        Destroy(gameObject,8f);
    }

    
    void Update()
    {
        
    }


}
