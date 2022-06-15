using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private int damage = 40;
    private int speed = 8;
    void Start()
    {
        Rigidbody fireBallRigidbody = GetComponent<Rigidbody>();
        fireBallRigidbody.velocity = transform.forward*speed;
        Destroy(gameObject,3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag =="Player")
        {
            IDamageable target = other.GetComponent<IDamageable>();

            if(target != null)
                target.OnDamage(damage);

            Destroy(gameObject);
        }
    }

    // private void OnTriggerExit(Collider other) {
    //     if(other.tag == "Player" || other.tag=="Ground")
    //         Destroy(gameObject);
    // }
}
