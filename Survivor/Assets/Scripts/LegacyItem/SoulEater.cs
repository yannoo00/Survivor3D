using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEater : ReinforceState, IItem //num6
{
    GameObject soulEater;
    public GameObject Player;
    public LayerMask whatIsTarget;
    private AudioSource audioSource;
    public Sprite drainAura;

    public int damage = 24;
    public float period = 1f;
    public float boundary = 4f;

    public bool drainCheck = false;
    void Start()
    {
        StartCoroutine(Deal());
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0: 
                soulEater.GetComponent<SoulEater>().damage += 5;
                step++;
                break;

            case 1 :
                soulEater.GetComponent<SoulEater>().period -= 0.2f;
                step++;
                break;

            case 2 :
                soulEater.GetComponent<SoulEater>().damage += 10;
                step++;
                break;

            case 3 :
                soulEater.GetComponent<SoulEater>().period -= 0.3f;
                step++;
                break;

            case 4 :
                soulEater.transform.localScale = new Vector3(3,3,1);
                soulEater.GetComponent<SoulEater>().boundary += 2.5f;
                step++;
                break;

            case 5 :
                soulEater.GetComponent<SoulEater>().damage += 10;
                step++;
                break;
                
            case 6:
                soulEater.GetComponent<SoulEater>().drainCheck = true;
                //soulEater.GetComponent<SpriteRenderer>().sprite = drainAura;
                step++;
                break;

        }
    }

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        soulEater = Instantiate(gameObject) as GameObject;
        soulEater.transform.SetParent(target.transform,false);
        soulEater.GetComponent<SoulEater>().Player = target;
    }

    private IEnumerator Deal()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        
        while(true)
        {
            Collider[] enemyColliders =
            Physics.OverlapSphere(transform.position, boundary, whatIsTarget);

            for(int i =0; i<enemyColliders.Length; i++)
            {
                LivingEntity livingEntity = enemyColliders[i].GetComponent<LivingEntity>();

                if(livingEntity != null)
                {
                    livingEntity.OnDamage(damage);
                    if(drainCheck)
                        {
                            GameObject target = Player;
                            LivingEntity targetLife = target.GetComponent<LivingEntity>();
                            targetLife.RestoreHealth(1);
                        }
                        
                }
                   
            }


            yield return new WaitForSeconds(period);            
        }
    }   


}
