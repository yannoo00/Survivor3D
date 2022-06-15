using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrigin : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource=GetComponent<AudioSource>();
    }
 
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag=="EnemyBullet")
        {
            audioSource.Play();           
            transform.parent.parent.GetComponent<PlayerHealth>().ChrageShield(5);
        }


    }


}
