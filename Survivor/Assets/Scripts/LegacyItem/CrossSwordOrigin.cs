using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSwordOrigin : MonoBehaviour
{
    public float damage;
    public float speed;
    public int through;
    private int throughCount;
    void Start()
    {
        Rigidbody swordRigidbody = GetComponent<Rigidbody>();

        swordRigidbody.velocity = transform.up*speed;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag =="Enemy")
        {
            IDamageable target = other.GetComponent<IDamageable>();

            if(target != null)
                target.OnDamage(damage);

            if(throughCount>=through)
                Destroy(gameObject);
            else if(throughCount<through)
                throughCount++;
        }

        else if(other.tag == "Boundary")
            Destroy(gameObject);
    }

    // private void OnTriggerExit(Collider other) {
    //     if(other.tag == "Enemy" || other.tag == "Ground")
    // }
}
