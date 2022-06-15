using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour
{
    public int damage = 24;
    public float duration=8;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        Destroy(gameObject,duration);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(0f, 0f, -800* Time.deltaTime); //회전
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Enemy")
        {
            if(other.GetComponent<LivingEntity>()!=null)
                other.GetComponent<LivingEntity>().OnDamage(damage);

        }
    }
}
