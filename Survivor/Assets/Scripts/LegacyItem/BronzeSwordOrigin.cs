using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronzeSwordOrigin : MonoBehaviour
{
    
    public float damage;
    public float speed;

    void Start()
    {
        Rigidbody swordRigidbody = GetComponent<Rigidbody>();

        swordRigidbody.velocity = transform.forward*speed;
    }


    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other) {
        if(other.tag =="Enemy")
        {
            IDamageable target = other.GetComponent<IDamageable>();

            if(target != null)
                target.OnDamage(damage);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Boundary")
            Destroy(gameObject);
    }
}
