using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuake : ReinforceState,IItem           //11
{

    private AudioSource audioSource;
    private GameObject earthQuakeSpawner;
    public GameObject earthQuakeOrigin;
    public Queue<GameObject> earthQuakeQ = new Queue<GameObject>();
    //GameObject earthQuake;
    GameObject player;
    Vector3 enemyPosition;
    public float period = 8;
    public int damage = 120;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        for(int i =0; i<4; i++)
        {
            GameObject earthQuake = Instantiate(earthQuakeOrigin) as GameObject; 
            earthQuake.transform.SetParent(player.transform,false); //플레이어 자식으로 만듦
            earthQuakeQ.Enqueue(earthQuake); //큐에 파티클 4개 넣고
            earthQuake.SetActive(false);

        }

        StartCoroutine(Generate());
    }

    
    void Update()
    {
        
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                earthQuakeSpawner.GetComponent<EarthQuake>().period--;
                step++;
                break;

            case 1:
                earthQuakeSpawner.GetComponent<EarthQuake>().period -= 2;
                step++;
                break;

            case 2:
                earthQuakeSpawner.GetComponent<EarthQuake>().period-= 1.5f;
                step++;
                break;
        }
    }

    //로테이션 Y축만 적 방향으로 바꿔주면 됨

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        earthQuakeSpawner = Instantiate(gameObject) as GameObject;
        earthQuakeSpawner.transform.SetParent(target.transform,false);
        earthQuakeSpawner.GetComponent<EarthQuake>().player = target;
    }


    private IEnumerator Generate()
    {
        while(true)
        {
            if(FindClosestEnemy()!=null)
                enemyPosition = FindClosestEnemy().transform.position;

            GameObject earthQuake = earthQuakeQ.Dequeue();  //큐에서 뽑아서
            earthQuake.SetActive(true);     //활성화 하고

            //earthQuake.transform.localEulerAngles =new Vector3(0,enemyPosition.y,0);
            //y축만 가장 가까운 적의 방향으로 조절 -> x,z축만 가져와서 바라보게 하기.
            Vector3 targetPosition = new Vector3(enemyPosition.x,earthQuake.transform.position.y,enemyPosition.z);
            earthQuake.transform.LookAt(targetPosition);
            earthQuake.GetComponent<EarthQuakeOrigin>().damage = damage;
            if(!player.GetComponent<LivingEntity>().dead)
            {
                if(player.GetComponentInChildren<Gun>().damage<=20)
                    earthQuake.GetComponent<EarthQuakeOrigin>().damage+=70;
                earthQuake.GetComponent<EarthQuakeOrigin>().off +=()=>earthQuakeQ.Enqueue(earthQuake);
                //earthQuake의 off를 통해 큐로 돌아오도록 설정
                earthQuake.GetComponent<EarthQuakeOrigin>().off +=()=>earthQuake.SetActive(false);

                audioSource.Play();                
            }
            yield return new WaitForSeconds(period);            
        }

    }


    GameObject FindClosestEnemy() {

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
    
}
