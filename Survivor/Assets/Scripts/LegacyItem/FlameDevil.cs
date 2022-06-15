using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDevil : ReinforceState,IItem  //num 7
{
    GameObject flameDevil;
    GameObject bullet,bullet2,bullet3;

    GameObject dropBullet1, dropBullet2, dropBullet3;
    new public Rigidbody rigidbody;
    public GameObject fireBullet;

    Animator animator;
    

    //불릿은 하나만 가지고 지금처럼 어디 닿으면 비활성화, 
    //다시 활성화 후 내 위치로 이동시키는 방식

    //파티클을 2개 생성해두고 다음, 총알이 닿은 위치로 파티클을 옮기고 play.
    //play 후 다시 비활성화. 파티클1번 2번을 번갈아가며 사용


    public int damage = 40;
    public float period = 3f;
    public float speed = 7f;
    public bool dropChecker=false;
    public bool strongerChecker=false;

    private AudioSource audioSource;
    public AudioClip shotSound;
    GameObject player;
    
    void Start()
    {
        // animator = GetComponent<Animator>();

        // bullet = Instantiate(fireBullet) as GameObject;
        

        // rigidbody = bullet.GetComponent<Rigidbody>();
        // bullet.GetComponent<FireBullet>().damage = damage;

        // StartCoroutine(Shoot());
        
    }

    private void OnEnable() 
    {
        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();

        bullet = Instantiate(fireBullet) as GameObject;
        bullet.transform.position=transform.position;
        bullet.SetActive(false);
        bullet2 = Instantiate(fireBullet) as GameObject;
        bullet2.transform.position=transform.position;
        bullet2.SetActive(false);
        bullet3 = Instantiate(fireBullet) as GameObject;
        bullet3.transform.position=transform.position;
        bullet3.SetActive(false);

        rigidbody = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<FireBullet>().damage = damage;

        StartCoroutine(Shoot());
        

        if(dropChecker)
            StartCoroutine(DropBullet());
    }
    
    
    void Update()
    {
        
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {

            case 0:
                flameDevil.GetComponent<FlameDevil>().period -= 0.5f;
                flameDevil.GetComponent<FlameDevil>().damage += 20;
                step++;
                break;



            case 1:
                flameDevil.GetComponent<FlameDevil>().speed += 4f;
                flameDevil.GetComponent<FlameDevil>().period -= 0.5f;
                step++;
                break;


            case 2:
                flameDevil.GetComponent<FlameDevil>().damage += 40;
                flameDevil.GetComponent<FlameDevil>().period -= 0.5f;
                //bullet.GetComponent<FireBullet>().damage = damage;
                step++;
                break;               


            case 3:
                flameDevil.GetComponent<FlameDevil>().dropChecker = true;
                flameDevil.SetActive(false);
                flameDevil.SetActive(true);
                step++;
                break;  

            case 4:
                flameDevil.GetComponent<FlameDevil>().strongerChecker = true;
                step++;
                break;  
            case 5:
                GameObject flameDevil2 = Instantiate(flameDevil)as GameObject;
                flameDevil2.transform.localPosition = flameDevil.transform.localPosition+transform.right;
                flameDevil2.transform.SetParent(flameDevil.GetComponent<FlameDevil>().player.transform.GetChild(0),false);
                step++;
                break;
            case 6:
                GameObject flameDevil3 = Instantiate(flameDevil)as GameObject;
                flameDevil3.transform.localPosition = flameDevil.transform.localPosition-transform.right;
                flameDevil3.transform.SetParent(flameDevil.GetComponent<FlameDevil>().player.transform.GetChild(0),false);
                step++;
                break;
        }
    }






    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        flameDevil = Instantiate(gameObject) as GameObject;
        flameDevil.transform.SetParent(target.transform.GetChild(0),false);
        flameDevil.GetComponent<FlameDevil>().player=target;
    }

    public IEnumerator Shoot()
    {
        while(true)
        {
            animator.SetTrigger("Attack");
            audioSource.PlayOneShot(shotSound);
            bullet.SetActive(true);
            bullet.transform.position = transform.position;

            rigidbody.velocity = transform.forward * speed;

            bullet.GetComponent<FireBullet>().damage = damage;

            if(strongerChecker)
            {
                bullet2.SetActive(true);
                bullet2.transform.position = transform.position;
                bullet2.GetComponent<Rigidbody>().velocity = transform.forward *speed + transform.right;
                bullet2.GetComponent<FireBullet>().damage = damage;


                bullet3.SetActive(true);
                bullet3.transform.position = transform.position;
                bullet3.GetComponent<Rigidbody>().velocity = transform.forward *speed + transform.right*-1;
                bullet3.GetComponent<FireBullet>().damage = damage;

            
            }


            yield return new WaitForSeconds(period);            
        }

    }

    public IEnumerator DropBullet()
    {
        dropBullet1 = Instantiate(fireBullet);
        dropBullet2 = Instantiate(fireBullet);
        dropBullet3 = Instantiate(fireBullet); 
        dropBullet1.SetActive(false);  
        dropBullet2.SetActive(false);
        dropBullet3.SetActive(false);  

        while(true)
        {
            dropBullet1.SetActive(true);

            float randomX = Random.Range(transform.position.x-7f,transform.position.x+7f);
            float randomZ = Random.Range(transform.position.z-7f,transform.position.z+7f);           
            Vector3 randomPosition = new Vector3(randomX,10f,randomZ);
            dropBullet1.transform.position = randomPosition;
            dropBullet1.GetComponent<FireBullet>().damage = damage;

            dropBullet1.GetComponent<Rigidbody>().velocity = new Vector3(0,-35,0);

            yield return new WaitForSeconds(0.5f);


            dropBullet2.SetActive(true);

            randomX = Random.Range(transform.position.x-7f,transform.position.x+7f);
            randomZ = Random.Range(transform.position.z-7f,transform.position.z+7f);           
            randomPosition = new Vector3(randomX,10f,randomZ);
            dropBullet2.transform.position = randomPosition;
            dropBullet2.GetComponent<FireBullet>().damage = damage;

            dropBullet2.GetComponent<Rigidbody>().velocity = new Vector3(0,-35,0);

            yield return new WaitForSeconds(0.5f);



            dropBullet3.SetActive(true);

            randomX = Random.Range(transform.position.x-7f,transform.position.x+7f);
            randomZ = Random.Range(transform.position.z-7f,transform.position.z+7f);           
            randomPosition = new Vector3(randomX,10f,randomZ);
            dropBullet3.transform.position = randomPosition;
            dropBullet3.GetComponent<FireBullet>().damage = damage;

            dropBullet3.GetComponent<Rigidbody>().velocity = new Vector3(0,-35,0);


            yield return new WaitForSeconds(0.5f);            
        }

    }
}
