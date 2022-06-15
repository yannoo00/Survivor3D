
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShieldCrashOrigin : MonoBehaviour
{
    public event Action off;
    public GameObject player;
    public int damage;
    public float speed;
    public bool shieldDrain=false;
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
                other.GetComponent<LivingEntity>().OnDamage(damage);
            if(shieldDrain)
                player.GetComponent<PlayerHealth>().ChrageShield(5);
            off();
        }    
        else if (other. tag =="Boundary")
            off();
    }
}

