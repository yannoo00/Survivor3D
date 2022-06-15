using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCrash : ReinforceState, IItem
{
    public float speed = 15f;
    public int damage = 20;
    public float period =2f;
    public int repeat= 1;
    public float bigger= 1;
    public int multiple = 3;
    public bool shieldDrain = false;
    public GameObject shieldPrefab;
    GameObject shieldSpawenr;
    GameObject player;
    AudioSource audioSource;
    Queue<GameObject> shieldQ = new Queue<GameObject>();


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        for(int i =0; i< 48; i++)    
        {
            GameObject shield = Instantiate(shieldPrefab);
            shieldQ.Enqueue(shield);
            shield.SetActive(false);
        }

        StartCoroutine(Deal());
    }

    void Update()
    {
        
    }

    public void Use(GameObject target)
    {
        step = 0;
        slotNum = 0;
        shieldSpawenr = Instantiate(gameObject) as GameObject;
        shieldSpawenr.transform.SetParent(target.transform.GetChild(0),false);
        shieldSpawenr.GetComponent<ShieldCrash>().player = target;
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                shieldSpawenr.GetComponent<ShieldCrash>().bigger += 0.5f;
                step++;
                break;

            case 1:
                shieldSpawenr.GetComponent<ShieldCrash>().damage += 20;
                shieldSpawenr.GetComponent<ShieldCrash>().period -= 0.5f;
                step++;
                break;

            case 2:
                shieldSpawenr.GetComponent<ShieldCrash>().repeat++;
                step++;
                break;

            case 3:
                shieldSpawenr.GetComponent<ShieldCrash>().shieldDrain=true;
                step++;
                break;

            case 4:
                shieldSpawenr.GetComponent<ShieldCrash>().multiple++;
                step++;
                break;                
        
        }
    }

    private IEnumerator Deal()
    {
        while(true)
        {
            for(int i =0; i < repeat; i++)
            {
                GameObject shield = shieldQ.Dequeue();
                shield.SetActive(true);

                audioSource.Play();

                shield.GetComponent<ShieldCrashOrigin>().damage
                = damage + (int)player.GetComponent<PlayerHealth>().Shield*multiple;

                shield.transform.localScale = new Vector3(bigger,bigger,bigger);

                shield.transform.eulerAngles= transform.parent.eulerAngles;
                shield.transform.position = transform.position+Vector3.up;
                shield.GetComponent<Rigidbody>().velocity = transform.forward*speed;
                shield.GetComponent<ShieldCrashOrigin>().player = player;
                if(shieldDrain)
                    shield.GetComponent<ShieldCrashOrigin>().shieldDrain=true;
                
                shield.GetComponent<ShieldCrashOrigin>().off +=()=>Add(shield);
                shield.GetComponent<ShieldCrashOrigin>().off +=()=>shield.SetActive(false);
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(period);            
        }
    }

    private void Add(GameObject shield)
    {
        if(!shieldQ.Contains(shield))
            shieldQ.Enqueue(shield);
    }
}
