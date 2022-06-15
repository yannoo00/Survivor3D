using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : ReinforceState,IItem
{
    GameObject mineSpawner;
    GameObject player;
    AudioSource audioSource;
    public GameObject minePrefab;
    //GameObject player;
    Queue<GameObject> mineQ = new Queue<GameObject>();
    public int damage = 125;
    public int lastTime = 15;
    public float period = 5;
    public int repeat = 1;

    void Start()
    {
        for(int i = 0; i < 15; i++)
        {
            GameObject mine = Instantiate(minePrefab);
            mineQ.Enqueue(mine);
            mine.SetActive(false);
        }

        StartCoroutine(Discharge());
    }

    void Update()
    {
        
    }

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        mineSpawner = Instantiate(gameObject) as GameObject;
        mineSpawner.transform.SetParent(target.transform,false);
        mineSpawner.GetComponent<MineSpawner>().player = target;
    }
    
    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                mineSpawner.GetComponent<MineSpawner>().period -=1;
                mineSpawner.GetComponent<MineSpawner>().lastTime += 3;
                step++;
                break;
            
            case 1:
                mineSpawner.GetComponent<MineSpawner>().damage += 100;
                mineSpawner.GetComponent<MineSpawner>().period -=1.5f;
                step++;
                break;
            
            case 2:
                mineSpawner.GetComponent<MineSpawner>().repeat++;
                step++;
                break;
        }
    }

    private IEnumerator Discharge()
    {
        while(true)
        {
            for(int i =0; i< repeat; i++)
            {
                audioSource= GetComponent<AudioSource>();
                audioSource.Play();

                GameObject mine = mineQ.Dequeue();
                mine.SetActive(true);
                mine.transform.position = transform.position;

                mine.GetComponent<Mine>().damage = damage
                +(int)player.GetComponent<PlayerHealth>().Shield;

                mine.GetComponent<Mine>().lastTime = lastTime;
                mine.GetComponent<Mine>().off+=()=>Add(mine);
                mine.GetComponent<Mine>().off+=()=>mine.SetActive(false);

                yield return new WaitForSeconds(0.175f);                   
            }

            yield return new WaitForSeconds(period); 
        }
    }

    private void Add(GameObject mine)
    {
        if(!mineQ.Contains(mine))
            mineQ.Enqueue(mine);
    }



}
