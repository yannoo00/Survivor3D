using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{

    
    public ParticleSystem particle;
    ParticleSystem particle1;
    ParticleSystem particle2;

    public LayerMask whatIsTarget;

 

    public int damage;
    
    private int pulling = 1;
    void Start()
    {
        
            particle1 = Instantiate(particle);

            particle2 = Instantiate(particle);

            
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {

        

        if(other.tag =="Enemy" || other.tag =="Ground")
        {
            
            
            if(other.tag=="Enemy")
            {
                if(pulling == 1)
                {
                    particle1.transform.position = transform.position;   
                    pulling = 2;    

                    particle1.Play();       
                    particle1.GetComponent<AudioSource>().Play();
                    
                }
                else if(pulling == 2)
                {
                    particle2.transform.position = transform.position;   
                    pulling = 1;       

                    particle2.Play();
                    particle2.GetComponent<AudioSource>().Play();  
                            
                }                
            }
            if(other.tag=="Ground")
            {
                if(pulling == 1)
                {
                    particle1.transform.position = transform.position + Vector3.up * 2f;   
                    pulling = 2;    

                    particle1.Play();
                    particle1.GetComponent<AudioSource>().Play();    
                         
                }
                else if(pulling == 2)
                {
                    particle2.transform.position = transform.position + Vector3.up * 2f;   
                    pulling = 1;       

                    particle2.Play();       
                    particle2.GetComponent<AudioSource>().Play();   
                    
                }  
            }



            Collider[] enemyColliders =
            Physics.OverlapSphere(transform.position,1f,whatIsTarget);

            for(int i=0; i < enemyColliders.Length; i++)
            {
                LivingEntity livingEntity = enemyColliders[i].GetComponent<LivingEntity>();
                if(livingEntity != null)
                {
                    livingEntity.OnDamage(damage);
                }
            }


            gameObject.SetActive(false);            
        }

    }
}
