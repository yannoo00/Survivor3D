using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellSword : ReinforceState, IItem
{
    // num 1

    
    public float damage = 300f;
    
    public float period = 10f;
    new private Rigidbody rigidbody;


    private GameObject player;

    GameObject hellSword;
    GameObject hellSword2;
    GameObject hellSword3;
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ParticleSystem particle3;
    public ParticleSystem particle4;
    public ParticleSystem particle5;
    public ParticleSystem particle6;

    ParticleSystem ground;
    ParticleSystem groundDark;
    ParticleSystem sphere;
    ParticleSystem fire;
    ParticleSystem spark;
    ParticleSystem impact;

    private AudioSource audioSource;
    public AudioClip shotSound;

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        hellSword = Instantiate(gameObject) as GameObject;
        hellSword.transform.SetParent(target.transform,false);
        player = target;
        
        return;
    }

    void Start()
    {
        //player = transform.parent.gameObject;
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(ReadyForDrop());

        ground = Instantiate(particle1);
        ground.Pause();
        groundDark = Instantiate(particle2);
        groundDark.Pause();
        sphere = Instantiate(particle3);
        sphere.Pause();
        impact = Instantiate(particle4);
        impact.Pause();
        fire  = Instantiate(particle5);
        fire.Pause();
        spark = Instantiate(particle6);
        spark.Pause();
    }

    
    void Update()
    {
        
    }
    
    public void Reinforce(int tech)
    {
         switch(tech)
         {
            case 0:

                hellSword.GetComponent<HellSword>().damage += 100;

                if(hellSword2!=null)
                    hellSword2.GetComponent<HellSword>().damage +=100;
                if(hellSword3!=null)
                    hellSword3.GetComponent<HellSword>().damage +=100;
                step++;
                break;

            case 1:

                hellSword.GetComponent<HellSword>().damage += 500;
                if(hellSword2!=null)
                    hellSword2.GetComponent<HellSword>().damage +=500;
                if(hellSword3!=null)
                    hellSword3.GetComponent<HellSword>().damage +=500;
                step++;
                break;


            case 2:
                hellSword.GetComponent<HellSword>().period -= 3f;
                if(hellSword2!=null)
                    hellSword2.GetComponent<HellSword>().period -=3;
                if(hellSword3!=null)
                    hellSword3.GetComponent<HellSword>().period -=3;
                step++;
                break;

            case 3:
                hellSword.GetComponent<HellSword>().period -= 2f;
                if(hellSword2!=null)
                    hellSword2.GetComponent<HellSword>().period -=2;
                if(hellSword3!=null)
                    hellSword3.GetComponent<HellSword>().period -=2;
                step++;
                break;

            case 4:
                hellSword2 = Instantiate(gameObject) as GameObject;
                hellSword2.transform.SetParent(player.transform,false);
                hellSword2.GetComponent<HellSword>().damage = damage;
                hellSword2.GetComponent<HellSword>().period = period;
                hellSword2.GetComponent<HellSword>().player = player;
                step++;
                break;

            case 5:
                hellSword3 = Instantiate(gameObject) as GameObject;
                hellSword3.transform.SetParent(player.transform,false);
                hellSword3.GetComponent<HellSword>().damage = damage;
                hellSword3.GetComponent<HellSword>().period = period;
                hellSword3.GetComponent<HellSword>().player = player;
                step++;
                break;
         }
    }


    private IEnumerator ReadyForDrop()
    {
        while(true)
        {
            float randomX = Random.Range(-5,5);
            float randomZ = Random.Range(-5,5);

            Vector3 randomPosition = new Vector3(randomX,25f,randomZ);

            transform.localPosition = randomPosition;
            

            rigidbody.velocity = new Vector3(0, -30, 0);

            yield return new WaitForSeconds(period);
        }
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Enemy")
        {
            IDamageable target = other.GetComponent<IDamageable>();

            if(target != null)
                target.OnDamage(damage);
        }
        if(other.tag == "Ground")
        {   
            audioSource.PlayOneShot(shotSound);
            //x, z축은 헬소드의 위치로 하고 y축은 4.5(땅바닥)로 고정.
            ground.transform.position = new Vector3(transform.position.x,1.5f,transform.position.z);
            ground.Play();
            
            groundDark.transform.position = new Vector3(transform.position.x,1.5f,transform.position.z);
            groundDark.Play();

            sphere.transform.position = new Vector3(transform.position.x,1.5f,transform.position.z);
            sphere.Play();

            fire.transform.position = new Vector3(transform.position.x,1.5f,transform.position.z);
            fire.Play();

            impact.transform.position = new Vector3(transform.position.x,1.5f,transform.position.z);
            impact.Play();

            spark.transform.position = new Vector3(transform.position.x,1.5f,transform.position.z);
            spark.Play();

        }

    }


    

}

