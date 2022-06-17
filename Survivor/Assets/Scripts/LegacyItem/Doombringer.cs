using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doombringer : MonoBehaviour
{
    public int damage = 84;
    public int rotatingSpeed = 800;
    



    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0f, 0f, -rotatingSpeed* Time.deltaTime); //회전
        transform.position = transform.parent.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Enemy"&&other.GetComponent<LivingEntity>()!=null)
        {
            other.GetComponent<LivingEntity>().OnDamage(damage);
            transform.parent.parent.parent.GetComponent<PlayerHealth>().RestoreHealth(1);
        }
    }
}
