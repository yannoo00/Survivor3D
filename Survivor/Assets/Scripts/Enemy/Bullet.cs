using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    public event Action off;
    new Rigidbody rigidbody;

    //public int speed;
    public int damage;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //rigidbody.velocity = transform.forward * speed;
    }
    void OnEnable()
    {
        //rigidbody.velocity=new Vector3(0,0,0);
        //rigidbody.velocity = transform.forward * speed;

    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player" || other.tag =="Boundary")
        {
            LivingEntity livingEntity = other.GetComponent<LivingEntity>();
            
            if(livingEntity!=null)
                livingEntity.OnDamage(damage);

            Die();            
        }
    }

    public virtual void Die()
    {
        if(off != null)
            off();
    }


}
