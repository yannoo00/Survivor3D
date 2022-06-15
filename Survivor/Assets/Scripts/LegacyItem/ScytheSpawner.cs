using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheSpawner : ReinforceState,IItem            //num10
{
    
    GameObject scytheSpawner;
    GameObject scythe;
    public GameObject scytheOrigin;

    public float period = 12f;
    public float duration = 6f;

    public int damage = 24;

    public int repeater = 1;
    public bool bigger = false;

    void Start()
    {
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
                scytheSpawner.GetComponent<ScytheSpawner>().damage +=15;
                scytheSpawner.GetComponent<ScytheSpawner>().duration +=1;
                step++;
                break;

            case 1:
                scytheSpawner.GetComponent<ScytheSpawner>().period--;
                scytheSpawner.GetComponent<ScytheSpawner>().damage +=8;
                step++;
                break;

            case 2:
                scytheSpawner.GetComponent<ScytheSpawner>().duration++;
                scytheSpawner.GetComponent<ScytheSpawner>().period--;
                step++;
                break;

            case 3:
                scytheSpawner.GetComponent<ScytheSpawner>().damage+=10;
                step++;
                break;           

            case 4:
                scytheSpawner.GetComponent<ScytheSpawner>().repeater++;
                step++;
                break;

            case 5:
                scytheSpawner.GetComponent<ScytheSpawner>().period -= 3;
                step++;
                break;

            case 6:
                scytheSpawner.GetComponent<ScytheSpawner>().period -= 1;
                scytheSpawner.GetComponent<ScytheSpawner>().damage +=10;
                scytheSpawner.GetComponent<ScytheSpawner>().duration++;
                step++;
                break;      

            case 7:
                scytheSpawner.GetComponent<ScytheSpawner>().repeater++;
                step++;
                break;

            case 8:
                scytheSpawner.GetComponent<ScytheSpawner>().period -=2;
                scytheSpawner.GetComponent<ScytheSpawner>().damage +=10;
                step++;
                break;

            case 9:
                scytheSpawner.GetComponent<ScytheSpawner>().bigger = true;
                step++;
                break;
        }

    }   

    public void Use(GameObject target)
    {

        step = 0; //초기화 해주기
        slotNum =0;
        scytheSpawner = Instantiate(gameObject) as GameObject;
        scytheSpawner.transform.SetParent(target.transform,false);
    }

    private IEnumerator Generate()
    {

        while(true)
        {
            for(int i =0; i < repeater ; i++)
            {

                scythe = Instantiate(scytheOrigin);

                float randomX = Random.Range(-7.5f,7.5f);
                float randomZ = Random.Range(-7.5f,7.5f);
                Vector3 randomPosition = new Vector3(transform.position.x+randomX,0.5f,transform.position.z + randomZ);
                scythe.transform.localPosition = randomPosition;

                //scythe.transform.localPosition = transform.position;

                scythe.GetComponent<Scythe>().damage = damage;
                scythe.GetComponent<Scythe>().duration = duration;

                if(bigger)
                    scythe.transform.localScale += new Vector3(1,1,1);

                yield return new WaitForSeconds(0.3f);

            }     
            
            yield return new WaitForSeconds(period);       
        }
        
    }

}
