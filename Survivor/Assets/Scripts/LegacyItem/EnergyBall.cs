using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnergyBall : MonoBehaviour
{
    public event Action off;
    public int energyDamage= 36;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag =="Enemy")
        {
            if(other.GetComponent<LivingEntity>()!=null)
                other.GetComponent<LivingEntity>().OnDamage(energyDamage);
        }
        else if(other.tag =="Boundary")
        {
            off();
        }
    }
}
