using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CrossSword : ReinforceState,IItem //num 4
{
    //[HideInInspector]
    public float period = 4f;
    
    public float damage = 40f;
    [HideInInspector]
    public float speed = 8f;
    public int repeat = 1;
    public int through = 1;
    public bool stronger = false;


    public GameObject crossSwordOrigin;
    GameObject crossSpawenr;

    private AudioSource audioSource;
    public AudioClip shotSound;

    private 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ReadyForShoot());
    }

    
    void Update()
    {
        
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                crossSpawenr.GetComponent<CrossSword>().damage+=10;
                crossSpawenr.GetComponent<CrossSword>().period-=1f;
                step++;
                break;

            case 1:
                crossSpawenr.GetComponent<CrossSword>().speed+=4;
                crossSpawenr.GetComponent<CrossSword>().damage+=10;
                step++;
                break;

            case 2:
                crossSpawenr.GetComponent<CrossSword>().repeat++;
                step++;
                break;

            case 3:
                crossSpawenr.GetComponent<CrossSword>().stronger = true;
                step++;
                break;

            case 4:
                crossSpawenr.GetComponent<CrossSword>().damage += 15;
                crossSpawenr.GetComponent<CrossSword>().period-=1.5f;
                step++;
                break;

            case 5:
                crossSpawenr.GetComponent<CrossSword>().through +=3;
                step++;
                break;
            
        }
    }

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        crossSpawenr = Instantiate(gameObject) as GameObject;
        crossSpawenr.transform.SetParent(target.transform,false);

        return;        
    }

    private IEnumerator ReadyForShoot()
    {
       while(true)
       {
        
        StartCoroutine(Generate());


        yield return new WaitForSeconds(period);             
        
       }
    }

    public IEnumerator Generate()
    {
        for(int i =0; i < repeat; i++)
        {            
            GameObject crossSword1 = Instantiate(crossSwordOrigin,transform.position,Quaternion.Euler(90,90,0));
            GameObject crossSword2 = Instantiate(crossSwordOrigin,transform.position,Quaternion.Euler(90,180,0));
            GameObject crossSword3 = Instantiate(crossSwordOrigin,transform.position,Quaternion.Euler(90,270,0));
            GameObject crossSword4 = Instantiate(crossSwordOrigin,transform.position,Quaternion.Euler(90,360,0));


            crossSword1.GetComponent<CrossSwordOrigin>().damage = damage;
            crossSword1.GetComponent<CrossSwordOrigin>().speed = speed;
            crossSword1.GetComponent<CrossSwordOrigin>().through= through;
            crossSword2.GetComponent<CrossSwordOrigin>().damage = damage;
            crossSword2.GetComponent<CrossSwordOrigin>().speed = speed;
            crossSword2.GetComponent<CrossSwordOrigin>().through= through;
            crossSword3.GetComponent<CrossSwordOrigin>().damage = damage;
            crossSword3.GetComponent<CrossSwordOrigin>().speed = speed;
            crossSword3.GetComponent<CrossSwordOrigin>().through= through;
            crossSword4.GetComponent<CrossSwordOrigin>().damage = damage;
            crossSword4.GetComponent<CrossSwordOrigin>().speed = speed;
            crossSword4.GetComponent<CrossSwordOrigin>().through= through;

            if(stronger == true)
            {
                GameObject crossSword5 = Instantiate(crossSwordOrigin,transform.position,Quaternion.Euler(90,45,0));
                GameObject crossSword6 = Instantiate(crossSwordOrigin,transform.position,Quaternion.Euler(90,135,0));
                GameObject crossSword7 = Instantiate(crossSwordOrigin,transform.position,Quaternion.Euler(90,225,0));
                GameObject crossSword8 = Instantiate(crossSwordOrigin,transform.position,Quaternion.Euler(90,315,0));
            
                crossSword5.GetComponent<CrossSwordOrigin>().damage = damage;
                crossSword5.GetComponent<CrossSwordOrigin>().speed = speed;
                crossSword5.GetComponent<CrossSwordOrigin>().through= through;
                crossSword6.GetComponent<CrossSwordOrigin>().damage = damage;
                crossSword6.GetComponent<CrossSwordOrigin>().speed = speed;
                crossSword6.GetComponent<CrossSwordOrigin>().through= through;
                crossSword7.GetComponent<CrossSwordOrigin>().damage = damage;
                crossSword7.GetComponent<CrossSwordOrigin>().speed = speed;
                crossSword7.GetComponent<CrossSwordOrigin>().through= through;
                crossSword8.GetComponent<CrossSwordOrigin>().damage = damage;
                crossSword8.GetComponent<CrossSwordOrigin>().speed = speed;
                crossSword8.GetComponent<CrossSwordOrigin>().through= through;
            }

            audioSource.PlayOneShot(shotSound);

            yield return new WaitForSeconds(0.3f);
        }
    }

}
