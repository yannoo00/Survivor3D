using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerSpawner : ReinforceState,IItem    //13
{

    public float speed = 10f;
    public int damage = 90;
    public float period = 2f;
    public int repeat = 1;

    public bool stronger = false;



    public GameObject daggerPrefab;

    GameObject daggerSpawner;
    GameObject player; 
    AudioSource audioSource;
    Queue<GameObject> daggerQ = new Queue<GameObject>();
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        for(int i = 0; i< 48; i++)
        {
            GameObject dagger = Instantiate(daggerPrefab);
            daggerQ.Enqueue(dagger);
            dagger.SetActive(false);
        }
        StartCoroutine(Deal());
    }

    
    void Update()
    {
        
    }

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        daggerSpawner = Instantiate(gameObject) as GameObject;
        daggerSpawner.transform.SetParent(target.transform.GetChild(0),false);
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                daggerSpawner.GetComponent<DaggerSpawner>().damage += 30;
                daggerSpawner.GetComponent<DaggerSpawner>().period -= 0.5f;
                step++;
                break;

            case 1:
                daggerSpawner.GetComponent<DaggerSpawner>().damage += 30;
                daggerSpawner.GetComponent<DaggerSpawner>().speed += 3f;
                step++;
                break;

            case 2:
                daggerSpawner.GetComponent<DaggerSpawner>().repeat++;
                step++;
                break;

            case 3:
                daggerSpawner.GetComponent<DaggerSpawner>().repeat++;
                step++;
                break;

            case 4:
                daggerSpawner.GetComponent<DaggerSpawner>().repeat++;
                step++;
                break;

            case 5:
                daggerSpawner.GetComponent<DaggerSpawner>().stronger =true;
                step++;
                break;
        }
    }

    private IEnumerator Deal()
    {
        while(true)
        {
            for(int i=0; i<repeat; i++)
            {
                GameObject dagger = daggerQ.Dequeue();
                dagger.SetActive(true);
                
                audioSource.Play();

                dagger.GetComponent<DaggerOrigin>().damage = damage;
                //dagger.GetComponent<DaggerOrigin>().speed = speed;
                dagger.transform.localEulerAngles = new Vector3(dagger.transform.localEulerAngles.x,transform.parent.localEulerAngles.y,transform.parent.localEulerAngles.z);
                dagger.transform.position = transform.position;
                dagger.GetComponent<Rigidbody>().velocity = transform.forward *speed;
                dagger.GetComponent<DaggerOrigin>().off+=()=>Add(dagger);
                dagger.GetComponent<DaggerOrigin>().off+=()=>dagger.SetActive(false);

                if(stronger)
                {
                    GameObject dagger2 = daggerQ.Dequeue();
                    dagger2.SetActive(true);
                    
                    //audioSource.Play();

                    dagger2.GetComponent<DaggerOrigin>().damage = damage;
                    //dagger.GetComponent<DaggerOrigin>().speed = speed;
                    dagger2.transform.localEulerAngles = new Vector3(dagger2.transform.localEulerAngles.x,transform.parent.localEulerAngles.y,transform.parent.localEulerAngles.z+25);
                    dagger2.transform.position = transform.position;
                    dagger2.GetComponent<Rigidbody>().velocity = transform.forward *speed +transform.right*2;
                    dagger2.GetComponent<DaggerOrigin>().off+=()=>Add(dagger2);
                    dagger2.GetComponent<DaggerOrigin>().off+=()=>dagger2.SetActive(false);
                
                    GameObject dagger3 = daggerQ.Dequeue();
                    dagger3.SetActive(true);
                    
                    //audioSource.Play();

                    dagger3.GetComponent<DaggerOrigin>().damage = damage;
                    //dagger.GetComponent<DaggerOrigin>().speed = speed;
                    dagger3.transform.localEulerAngles = new Vector3(dagger3.transform.localEulerAngles.x,transform.parent.localEulerAngles.y,transform.parent.localEulerAngles.z-25);
                    dagger3.transform.position = transform.position;
                    dagger3.GetComponent<Rigidbody>().velocity = transform.forward *speed+transform.right*-2;
                    dagger3.GetComponent<DaggerOrigin>().off+=()=>Add(dagger3);
                    dagger3.GetComponent<DaggerOrigin>().off+=()=>dagger3.SetActive(false);

                }
                yield return new WaitForSeconds(0.125f);
            }


            yield return new WaitForSeconds(period);            
        }
    }


    public void Add(GameObject dagger)
    {
        if(!daggerQ.Contains(dagger))
            daggerQ.Enqueue(dagger);
    }

}
