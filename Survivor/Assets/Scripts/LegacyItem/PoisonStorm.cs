using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoisonStorm : ReinforceState,IItem     //12
{
    private AudioSource audioSource;
    private GameObject player;
    private ParticleSystem particle;
    private Collider boxCollider;

    GameObject poisonStorm;

    public float period = 6;
    public int damage = 12;
    public int duration = 4;
    public int slow = 1;
    private float delay = 0.3f;

    public void Use(GameObject target)
    {
        
        step = 0; //초기화 해주기
        slotNum =0;
        poisonStorm = Instantiate(gameObject) as GameObject;
        poisonStorm.GetComponent<PoisonStorm>().player = target;
    } 
    

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                poisonStorm.GetComponent<PoisonStorm>().damage += 5;
                poisonStorm.GetComponent<PoisonStorm>().period -= 1;
                step++;
                break;


            case 1:
                poisonStorm.GetComponent<PoisonStorm>().damage += 5;
                poisonStorm.GetComponent<PoisonStorm>().duration += 2;
                step++;
                break;


            case 2:
                poisonStorm.transform.localScale += new Vector3(0.5f,0.5f,0.5f);
                step++;
                break;
        }
    }

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Pause();

        audioSource = GetComponent<AudioSource>();

        boxCollider = GetComponent<BoxCollider>();

        StartCoroutine(Deal());
    }

 
    void Update()
    {
        
    }

    private IEnumerator Deal()
    {
        while(true)
        {
            int randomX = Random.Range(-5,6);
            int randomZ = Random.Range(-5,6);
            Vector3 randomPosition = new Vector3(randomX,0,randomZ);

            transform.position = player.transform.position+randomPosition;
            particle.Play();
            audioSource.Play();

            float time = 0;

            while(true)
            {
                boxCollider.enabled = true;
                yield return new WaitForSeconds(delay);
                boxCollider.enabled = false;
                time += delay;

                if(time>=duration)
                    break;
            }

            particle.Stop();
            audioSource.Stop();

            yield return new WaitForSeconds(period);
        }

    }    
    
    
    private void OnTriggerEnter(Collider other )
    {
        
        LivingEntity attactkTarget = other.GetComponent<LivingEntity>();

        if(attactkTarget!=null)
        {
            if(attactkTarget.tag =="Enemy")
            {
                if(player.GetComponentInChildren<Gun>()!=null)
                {
                    if(player.GetComponentInChildren<Gun>().damage<=20)
                        attactkTarget.OnDamage(damage+24);
                    else
                        attactkTarget.OnDamage(damage);                    
                }
            }
        }
    }


    // private IEnumerator Slow(Collider other)
    // {
    //     other.gameObject.GetComponent<NavMeshAgent>().speed -=1;
    //     yield return new WaitForSeconds(1);
    //     other.gameObject.GetComponent<NavMeshAgent>().speed +=1;
    // }
}
