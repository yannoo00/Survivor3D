using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyFire : ReinforceState,IItem       //num 2
{
    [HideInInspector]
    public float period = 3f;
    
    public float damage = 30f;

    public float speed = 10f;
    public int repeater = 1;
    public GameObject bronzeSwordOrigin;
    //public GameObject livingEnergyPrefab;

    private AudioSource audioSource;
    public AudioClip shotSound;

    Vector3 enemyPosition;
    GameObject bronzeSword;
    GameObject bronzeSwordSpawner;
    GameObject player;
    //GameObject livingFire;
    // Start is called before the first frame update
    void Start()
    {   
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ReadyForShoot());
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
                //bronzeSword.GetComponent<BronzeSwordOrigin>().damage += 10;
                //위처럼 하면 아직 생성되기 전인 오브젝트에 접근하려해서 오류남
                //생성한 오브젝트에 접근해서 공격력 ++ 해줘야 하는것임

                bronzeSwordSpawner.GetComponent<EnergyFire>().damage += 15;
                step++;
                break;



            case 1:
                bronzeSwordSpawner.GetComponent<EnergyFire>().period -= 1;
                step++;
                break;



            case 2:
                bronzeSwordSpawner.GetComponent<EnergyFire>().repeater++;
                step++;
                break;

            case 3:
                bronzeSwordSpawner.GetComponent<EnergyFire>().repeater++;
                step++;
                break;

            case 4:
                bronzeSwordSpawner.GetComponent<EnergyFire>().damage += 10;
                bronzeSwordSpawner.GetComponent<EnergyFire>().period -= 1;
                step++;
                break;
        }
    }

    private IEnumerator ReadyForShoot()
    {
        while(true)
        {        
            for(int i =0; i<repeater; i++)
            {
                if(FindClosestEnemy()!=null)
                {
                    enemyPosition = FindClosestEnemy().transform.position;
                    
                    Vector3 targetPosition = new Vector3 (enemyPosition.x, enemyPosition.y,enemyPosition.z);



                    StartCoroutine(Generate(targetPosition));   
                }
  
                yield return new WaitForSeconds(0.3f);           
            }
            //bronzeSword.transform.localEulerAngles = new Vector3(90,0,0);
            //bronzeSword.GetComponent<Rigidbody>().velocity = transform.up*speed;
           

            //bronzeSwordOG.transform.LookAt(enemyPosition);

            //Rigidbody swordRigidbody = bronzeSwordOG.GetComponent<Rigidbody>();
            //swordRigidbody.velocity = Vector3.forward*speed;
            //bronzeSwordOG.transform.position = Vector3.Lerp(bronzeSwordOG.transform.position,enemyPosition,Time.deltaTime);
            yield return new WaitForSeconds(period);
        }
    }


    public IEnumerator Generate(Vector3 targetPosition)
    {

        bronzeSword = Instantiate(bronzeSwordOrigin, transform.position, transform.rotation);

        audioSource.PlayOneShot(shotSound);
        bronzeSword.GetComponent<BronzeSwordOrigin>().damage = damage;

        if(!player.GetComponent<LivingEntity>().dead)
        {
            if(player.GetComponentInChildren<Gun>().damage<=20)
                bronzeSword.GetComponent<BronzeSwordOrigin>().damage+=25;
                    
            bronzeSword.GetComponent<BronzeSwordOrigin>().speed = speed;
            bronzeSword.transform.LookAt(targetPosition);              
                     
        }
        yield return new WaitForSeconds(1f);
    }

    // Find the name of the closest enemy

    GameObject FindClosestEnemy() 
    {

        GameObject[] gos;

        gos = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = null;

        float distance = Mathf.Infinity;

        Vector3 position = transform.position;

        foreach (GameObject go in gos) 
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance) 
            {
                if(go.GetComponent<LivingEntity>()!=null)
                {
                    if(!go.GetComponent<LivingEntity>().dead)
                    {
                        closest = go;
                        distance = curDistance;                    
                    }                    
                }


            }
        }
        

        return closest;
    }
    

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        bronzeSwordSpawner = Instantiate(gameObject) as GameObject;
        bronzeSwordSpawner.transform.SetParent(target.transform,false);
        bronzeSwordSpawner.GetComponent<EnergyFire>().player=target;

        return;

    }
}
