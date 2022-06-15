using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public AudioSource audioSource;
    //public AudioClip itemPickUp;
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        IItemDrop item = other.GetComponent<IItemDrop>();
        if(item !=null)
        {
            item.Use(transform.parent.gameObject);
            audioSource.PlayOneShot(transform.parent.GetComponent<PlayerHealth>().itemPickupClip);
        }

        
    }


}
