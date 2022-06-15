using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeOrigin : MonoBehaviour
{
    public int damage = 48;
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
        if(other.tag =="Enemy")
        {
            other.GetComponent<LivingEntity>().OnDamage(damage);
            
        }
    }
}
