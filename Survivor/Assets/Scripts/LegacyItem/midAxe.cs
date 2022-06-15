using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class midAxe : ReinforceState,IItem
{
    public int repeat = 1;
    public float speed = 9f;
    public float period = 3f;
    public int damage = 48;
    public int rotatingSpeed = 800;

    public GameObject axePrefab;
    GameObject axeSpawner;
    AudioSource audioSource;
    Queue<GameObject> axeQ = new Queue<GameObject>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        for(int i =0; i< 10; i++)
        {
            GameObject axe = Instantiate(axePrefab);
            axeQ.Enqueue(axe);
            axe.SetActive(false);
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
        axeSpawner = Instantiate(gameObject) as GameObject;
        axeSpawner.transform.SetParent(target.transform.GetChild(0),false);
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                axeSpawner.GetComponent<midAxe>().damage+=20;
                axeSpawner.GetComponent<midAxe>().period -=0.5f;
                step++;
                break;
            
            case 1:
                axeSpawner.GetComponent<midAxe>().repeat++;
                step++;
                break;

            case 2:
                axeSpawner.GetComponent<midAxe>().repeat++;
                step++;
                break;
            
            case 3:
                axeSpawner.GetComponent<midAxe>().rotatingSpeed+=200;
                step++;
                break;
        }
    }

    private IEnumerator Deal()
    {
        while(true)
        {
            for(int i =0; i< repeat; i++)
            {
                GameObject axe = axeQ.Dequeue();
                axe.SetActive(true);

                audioSource.Play();

                axe.GetComponentInChildren<AxeOrigin>().damage = damage;
                axe.GetComponentInChildren<AxeOrigin>().rotatingSpeed = rotatingSpeed;
                axe.transform.position = transform.position;
                axe.GetComponent<Rigidbody>().velocity =transform.forward*speed;

                axe.GetComponent<AxeHolder>().off+=()=>Add(axe);
                axe.GetComponent<AxeHolder>().off+=()=>axe.SetActive(false);

                yield return new WaitForSeconds(0.175f);
            }
            yield return new WaitForSeconds(period);
        }
    }


    public void Add(GameObject axe)
    {
        if(!axeQ.Contains(axe))
            axeQ.Enqueue(axe);
    }
}
