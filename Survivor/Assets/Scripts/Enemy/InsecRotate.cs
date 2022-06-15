using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsecRotate : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.RotateAround(transform.parent.position,Vector3.down,100*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Player")
        {
            IDamageable target = other.GetComponent<IDamageable>();

            if(target != null)
                target.OnDamage(24);
        }

    }

}
