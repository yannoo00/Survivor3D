using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBreath : ReinforceState,IItem  //num 9
{
    public int damage = 30;
    public float period = 7;
    public float duration = 4;
    public float timeBetAttack = 0.3f;
    public float lastAttackTime;
    GameObject flameBreath;
    GameObject flameBreath2;
    GameObject playerFor2;
    ParticleSystem flame;
    new Collider collider;
    private AudioSource audioSource;
    public AudioClip shotSound;

    void Start()
    {
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;

        flame = GetComponent<ParticleSystem>();
        
        //flame.Pause();    

        //StartCoroutine(Breath());
    }

    private void OnEnable() 
    {
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;

        audioSource = GetComponent<AudioSource>();

        flame = GetComponent<ParticleSystem>();
        flame.Pause();  
        StartCoroutine(Breath());    
    }

    
    void Update()
    {
        
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                flameBreath.GetComponent<FlameBreath>().damage += 5;
                flameBreath.GetComponent<FlameBreath>().duration += 1;

                if(flameBreath2!=null)
                {
                    flameBreath2.GetComponent<FlameBreath>().damage +=5;
                    flameBreath2.GetComponent<FlameBreath>().duration += 0.5f;
                }
                step++;
                break;


            case 1:
                flameBreath.GetComponent<FlameBreath>().period -= 1;
                if(flameBreath2!=null)
                    flameBreath2.GetComponent<FlameBreath>().period -=1;
                step++;
                break;
                

            case 2:
                flameBreath.GetComponent<FlameBreath>().duration += 1;
                if(flameBreath2!=null)
                    flameBreath2.GetComponent<FlameBreath>().duration +=1;
                step++;
                break;


            case 3:
                flameBreath.GetComponent<FlameBreath>().damage += 20;
                if(flameBreath2!=null)
                    flameBreath2.GetComponent<FlameBreath>().damage +=20;
                step++;
                break;


            case 4: //?????? ??????
                flameBreath2 = Instantiate(gameObject) as GameObject;
                flameBreath2.transform.SetParent(playerFor2.transform.GetChild(0),false);
                flameBreath2.transform.localEulerAngles = new Vector3(0,893,-1.5f);

                flameBreath.SetActive(false);
                flameBreath.SetActive(true);

                flameBreath2.GetComponent<FlameBreath>().damage = flameBreath.GetComponent<FlameBreath>().damage;
                flameBreath2.GetComponent<FlameBreath>().period = flameBreath.GetComponent<FlameBreath>().period;
                flameBreath2.GetComponent<FlameBreath>().duration = flameBreath.GetComponent<FlameBreath>().duration;
                step++;
                break;  
                

            case 5:
                flameBreath.GetComponent<FlameBreath>().duration += 2;
                if(flameBreath2!=null)
                    flameBreath2.GetComponent<FlameBreath>().duration +=2;
                step++;
                break;


            case 6:
                flameBreath.GetComponent<FlameBreath>().period -= 2;
                flameBreath.GetComponent<FlameBreath>().damage += 5;
                if(flameBreath2!=null)
                {
                    flameBreath2.GetComponent<FlameBreath>().period -=2;
                    flameBreath2.GetComponent<FlameBreath>().damage += 5;
                }
                    
                step++;
                break;

        }
    }

    public void Use(GameObject target)
    {
        step = 0; //????????? ?????????
        slotNum =0;
        flameBreath = Instantiate(gameObject) as GameObject;
        flameBreath.transform.SetParent(target.transform.GetChild(0), false);
        playerFor2 = target;
        return;
    }


    private IEnumerator Breath()
    {
        while(true)
        {
            
            flame.Play();
            audioSource.Play();

            float time = 0;
            
            while(true)
            {
                collider.enabled= true;
                
                yield return new WaitForSeconds(timeBetAttack);//?????? ??? ?????? ??????(0.3???)
                collider.enabled= false; //???????????? ?????? ????????? ????????? ??????
                time+=timeBetAttack;

                if(time>=duration)
                    break;
            }
            
            //yield return new WaitForSeconds(duration);

            audioSource.Stop();
            flame.Stop();

            yield return new WaitForSeconds(period);            
        }

    }

    private void OnTriggerEnter(Collider other) {
        // ????????? ????????? ????????? ?????? ??????????????? ?????? ??????????????? ?????? ??????   


            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if(attackTarget!=null)
            {
                if (attackTarget.tag =="Enemy")
                {
                    lastAttackTime = Time.time;

                    attackTarget.OnDamage(damage);
                }
            }
    }



}
