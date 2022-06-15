using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DaggerOrigin : MonoBehaviour
{
    new Rigidbody rigidbody;

    public event Action off;

    public int damage;
    public float speed;

    void Start()
    {
       
    }

    void Update()
    {
        
    }

    void OnEnable()
    { 
        //rigidbody = GetComponent<Rigidbody>();
        //rigidbody.velocity= new Vector3(0,0,0);

        //rigidbody.velocity=transform.forward*speed;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<LivingEntity>().OnDamage(damage);
            off();
        }
        else if(other.tag=="Boundary")
            off();
    }
}
