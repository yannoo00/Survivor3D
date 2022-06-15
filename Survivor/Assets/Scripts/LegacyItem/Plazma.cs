using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plazma : ReinforceState,IItem       //num3
{

    public LayerMask whatIsTarget;
    Transform targetTrans;
    LivingEntity targetEntity;

    public GameObject plazmaOrigin;
    //GameObject plazma;
    GameObject plazmaSpawner;
    GameObject player;

    //[HideInInspector]
    public float period = 7f;
    //[HideInInspector]
    public float damage = 200f;
    public int repeater = 1;
    public float boundary = 1;
    public bool bigger =false;

    private AudioSource audioSource;
    public AudioClip shotSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(getEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        //audioSource = GetComponent<AudioSource>();
        //StartCoroutine(getEnemy());
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                plazmaSpawner.GetComponent<Plazma>().damage += 50;
                plazmaSpawner.GetComponent<Plazma>().period -= 0.5f;
                step++;
                break;

            case 1:
                plazmaSpawner.GetComponent<Plazma>().period -= 1;
                plazmaSpawner.GetComponent<Plazma>().damage += 20;
                step++;
                break;

            case 2:
                plazmaSpawner.GetComponent<Plazma>().boundary +=1;
                plazmaSpawner.GetComponent<Plazma>().bigger =true;
                step++;
                break;

            case 3:
                plazmaSpawner.GetComponent<Plazma>().repeater++;
                step++;
                break;

            case 4:
                plazmaSpawner.GetComponent<Plazma>().period -= 2;
                step++;
                break;

            case 5:
                plazmaSpawner.GetComponent<Plazma>().damage += 30;
                plazmaSpawner.GetComponent<Plazma>().period -= 2;
                step++;
                break;

            case 6:
                plazmaSpawner.GetComponent<Plazma>().damage += 100;
                step++;
                break;

            case 7:
                plazmaSpawner.GetComponent<Plazma>().repeater++;
                step++;
                break;
        }
    }

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        plazmaSpawner = Instantiate(gameObject) as GameObject;
        plazmaSpawner.transform.SetParent(target.transform,false);
        plazmaSpawner.GetComponent<Plazma>().player=target;
        return;
    }

    private IEnumerator getEnemy()
    {
        while(true)
        {
            
            for(int i =0; i < repeater; i++)
            {
                Collider[] colliders =
                Physics.OverlapSphere(transform.position, 10f, whatIsTarget);

                for(int j =0; j < colliders.Length;j++)
                {

                    LivingEntity livingEntity =
                    colliders[j].GetComponent<LivingEntity>();

                    if(livingEntity != null&&!livingEntity.dead)
                        {
                            targetTrans = 
                            colliders[j].GetComponent<Transform>();
                            
                            targetEntity = livingEntity;
                            break;
                        }
                }

                if(targetTrans!=null)
                {
                    GameObject plazma = Instantiate(plazmaOrigin, targetTrans.position,transform.rotation);
                    plazma.GetComponent<PlazmaOrigin>().damage = damage;
                    plazma.GetComponent<PlazmaOrigin>().boundary = boundary;
                    if(bigger)
                        plazma.transform.localScale += new Vector3(1,1,1);
                    audioSource.PlayOneShot(shotSound);
                }       

                yield return new WaitForSeconds(0.25f);        
            }
            float cooldown = period;
            if(player.GetComponentInChildren<Gun>()!=null)
            {
                if(player.GetComponentInChildren<Gun>().damage<=20)
                    cooldown-=1;                
            }
            yield return new WaitForSeconds(cooldown);                
            

        }
    }

}
