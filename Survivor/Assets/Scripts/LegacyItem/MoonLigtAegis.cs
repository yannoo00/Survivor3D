using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLigtAegis : ReinforceState, IItem
{
    GameObject player;
    GameObject moonLightAegis;
    public LayerMask whatIsTarget;
    public float boudary=5f;
    public int damage = 5;
    public float period=1f;
    public float multiple= 0.1f;
    public GameObject moonLigtPrefab;
    Queue<GameObject> moonLightQ = new Queue<GameObject>();
    void Start()
    {
        for(int i =0; i< 40;i++)
        {
            GameObject moonLight = Instantiate(moonLigtPrefab);
            moonLightQ.Enqueue(moonLight);
            moonLight.SetActive(false);
        }

        StartCoroutine(Deal());
    }

    void Update()
    {
        
    }

    public void Use(GameObject target)
    {
        slotNum =0;
        step=0;
        moonLightAegis = Instantiate(gameObject) as GameObject;
        moonLightAegis.transform.SetParent(target.transform,false);

        target.GetComponent<PlayerHealth>().maxShield+=25;
        target.GetComponent<PlayerHealth>().shieldSlider.maxValue+=25;
        
        moonLightAegis.GetComponent<MoonLigtAegis>().player = target;
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                moonLightAegis.GetComponent<MoonLigtAegis>().damage+=5;
                moonLightAegis.GetComponent<MoonLigtAegis>().boudary+=1;
                step++;
                break;
            

            case 1:
                moonLightAegis.GetComponent<MoonLigtAegis>().period-=0.3f;
                moonLightAegis.GetComponent<MoonLigtAegis>().boudary+=1;
                step++;
                break;
            

            case 2:
                moonLightAegis.GetComponent<MoonLigtAegis>().multiple+=0.1f;

                step++;
                break;
        }
    }

    private IEnumerator Deal()
    {
        while(true)
        {
            Collider[] enemyColliders =
            Physics.OverlapSphere(transform.position, boudary+player.GetComponent<PlayerHealth>().Shield*0.1f, whatIsTarget);
            int cnt=0;

            for(int i= 0; i<enemyColliders.Length; i++)
            {
                LivingEntity livingEntity = enemyColliders[i].GetComponent<LivingEntity>();
                cnt++;
                if(cnt>20)
                {
                    cnt= 0;
                    break;
                }
                
                if(livingEntity!=null)
                {
                    GameObject moonLight = moonLightQ.Dequeue();
                    moonLight.SetActive(true);
                    moonLight.transform.position = livingEntity.transform.position;
                    moonLight.GetComponent<MoonLightOrigin>().off+=()=>Add(moonLight);
                    moonLight.GetComponent<MoonLightOrigin>().off+=()=>moonLight.SetActive(false);

                    livingEntity.OnDamage(damage+player.GetComponent<PlayerHealth>().Shield*multiple);
                }
            }

            yield return new WaitForSeconds(period);
        }
    }


    public void Add(GameObject moonLight)
    {
        if(!moonLightQ.Contains(moonLight))
            moonLightQ.Enqueue(moonLight);
    }
}
