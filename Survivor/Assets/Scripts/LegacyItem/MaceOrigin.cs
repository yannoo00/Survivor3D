using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceOrigin : MonoBehaviour
{
    public int damage;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Enemy" && other.GetComponent<LivingEntity>()!=null)
            other.GetComponent<LivingEntity>().OnDamage(damage);    
    }
}
