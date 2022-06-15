using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CrabBoss : LivingEntity
{
    public LayerMask whatIsTarget;
    public Slider slider;
    public GameObject bulletPrefab1;
    public GameObject bulletPrefab2;
    public Queue<GameObject> bullet1Q = new Queue<GameObject>();
    public Queue<GameObject> bullet2Q = new Queue<GameObject>();
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;
    public AudioClip hitSound;
    public AudioClip attackSound;
    private Animator animator;
    private AudioSource audioSource;

    
    private int damage = 50;


    void Start()
    {
        Collider[] colliders = 
        Physics.OverlapSphere(transform.position, 50f, whatIsTarget);

        for(int i =0; i < colliders.Length; i++)
        {
            LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

            if(livingEntity!= null && !livingEntity.dead)
            {
                targetEntity = livingEntity;
                break;
            }
        }


        for(int i =0; i <500; i ++)
        {
            GameObject bullet1 = Instantiate(bulletPrefab1);
            bullet1Q.Enqueue(bullet1);
            bullet1.SetActive(false);
        }


        for(int i =0; i <500; i ++)
        {
            GameObject bullet2 = Instantiate(bulletPrefab2);
            bullet2Q.Enqueue(bullet2);
            bullet2.SetActive(false);
        }


        pathFinder = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(Tracking());
        StartCoroutine(Stop());
        StartCoroutine(Deal());

    }


    void Update()
    {
        
    }


    public override void OnDamage(float damage) {
        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        if(!dead)
        {
            GameObject hudText = Instantiate(hudDamageText); // 생성할 텍스트 오브젝트
            hudText.transform.position = transform.position + Vector3.up; // 표시될 위치
            hudText.GetComponent<FloatingDamage>().damage = (int)damage; // 데미지 전달
            //hitEffect.transform.position = hitPoint;
            //hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal); //공격이 날아온 방향을 바라보는 방향(?)
            //hitEffect.Play();
            slider.value = health;
            audioSource.PlayOneShot(hitSound);
            //StartCoroutine(HitEffect());
        }

        base.OnDamage(damage);
    }

    private IEnumerator Tracking()
    {
        while(!dead)
        {
            pathFinder.SetDestination(targetEntity.transform.position);

            yield return new WaitForSeconds(0.25f);            
        }

    }

    private IEnumerator Deal()
    {
        while(true)
        {
            if(pathFinder.isStopped)
            {
                int choose;
                choose = Random.Range(1,6);

                animator.SetBool("Moving",false);
                
                audioSource.PlayOneShot(attackSound);
                
                if(choose >3)
                {
                    animator.SetTrigger("Attack_1");
                    
                    
                    GameObject bullet1 = bullet2Q.Dequeue();
                    bullet1.SetActive(true);
                    bullet1.transform.position = transform.position+Vector3.up;
                    bullet1.transform.LookAt(targetEntity.transform.position);

                    bullet1.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                    bullet1.GetComponent<Rigidbody>().velocity = bullet1.transform.forward*8;
                    bullet1.GetComponent<Bullet>().off +=()=>Add2(bullet1);
                    bullet1.GetComponent<Bullet>().off +=()=>bullet1.SetActive(false);


                    GameObject bullet1_2 = bullet2Q.Dequeue();
                    bullet1_2.SetActive(true);
                    bullet1_2.transform.position = transform.position+Vector3.up;
                    bullet1_2.transform.LookAt(targetEntity.transform.position);

                    bullet1_2.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                    bullet1_2.GetComponent<Rigidbody>().velocity = bullet1_2.transform.forward*8+bullet1_2.transform.right*2;
                    bullet1_2.GetComponent<Bullet>().off +=()=>Add2(bullet1_2);
                    bullet1_2.GetComponent<Bullet>().off +=()=>bullet1_2.SetActive(false);



                    GameObject bullet1_3 = bullet2Q.Dequeue();
                    bullet1_3.SetActive(true);
                    bullet1_3.transform.position = transform.position+Vector3.up;
                    bullet1_3.transform.LookAt(targetEntity.transform.position);

                    bullet1_3.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                    bullet1_3.GetComponent<Rigidbody>().velocity = bullet1_3.transform.forward*8+bullet1_3.transform.right*-2;
                    bullet1_3.GetComponent<Bullet>().off +=()=>Add2(bullet1_3);
                    bullet1_3.GetComponent<Bullet>().off +=()=>bullet1_3.SetActive(false);

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////

                    yield return new WaitForSeconds(0.75f);

                    for(int i=0;i<2;i++)
                    {
                    for(int j =0; j<12; j ++)
                    {
                        GameObject bullet1_4 = bullet1Q.Dequeue();
                        bullet1_4.SetActive(true);
                        bullet1_4.transform.localEulerAngles = new Vector3(0,j*30,0);
                        bullet1_4.transform.position = transform.position+Vector3.up;
                        //bullet1_4.transform.LookAt(targetEntity.transform.position);
                        bullet1_4.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                        bullet1_4.GetComponent<Rigidbody>().velocity = bullet1_4.transform.forward*8;
                        bullet1_4.GetComponent<Bullet>().off +=()=>Add1(bullet1_4);
                        bullet1_4.GetComponent<Bullet>().off +=()=>bullet1_4.SetActive(false);
                    }
                    yield return new WaitForSeconds(0.25f);
                    }

                    yield return new WaitForSeconds(0.25f);

                    for(int i=0;i<2;i++)
                    {
                    for(int j =0; j<12; j ++)
                    {
                        GameObject bullet1_4 = bullet1Q.Dequeue();
                        bullet1_4.SetActive(true);
                        bullet1_4.transform.localEulerAngles = new Vector3(0,15+j*30,0);
                        bullet1_4.transform.position = transform.position+Vector3.up;
                        //bullet1_4.transform.LookAt(targetEntity.transform.position);
                        bullet1_4.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                        bullet1_4.GetComponent<Rigidbody>().velocity = bullet1_4.transform.forward*8;
                        bullet1_4.GetComponent<Bullet>().off +=()=>Add1(bullet1_4);
                        bullet1_4.GetComponent<Bullet>().off +=()=>bullet1_4.SetActive(false);
                    }
                    yield return new WaitForSeconds(0.25f);
                    }

                    yield return new WaitForSeconds(0.25f);

                    for(int i=0;i<2;i++)
                    {
                    for(int j =0; j<12; j ++)
                    {
                        GameObject bullet1_4 = bullet1Q.Dequeue();
                        bullet1_4.SetActive(true);
                        bullet1_4.transform.localEulerAngles = new Vector3(0,j*30,0);
                        bullet1_4.transform.position = transform.position+Vector3.up;
                        //bullet1_4.transform.LookAt(targetEntity.transform.position);
                        bullet1_4.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                        bullet1_4.GetComponent<Rigidbody>().velocity = bullet1_4.transform.forward*8;
                        bullet1_4.GetComponent<Bullet>().off +=()=>Add1(bullet1_4);
                        bullet1_4.GetComponent<Bullet>().off +=()=>bullet1_4.SetActive(false);
                    }
                    yield return new WaitForSeconds(0.25f);
                    }                      
                }
                else if(3==choose)
                {
                    for(int i = 0; i < 4; i ++)
                    {
                    for(int j =0; j<18; j ++)
                    {
                        GameObject bullet1_4 = bullet2Q.Dequeue();
                        bullet1_4.SetActive(true);
                        int angle;
                        angle = Random.Range(0,360);
                        bullet1_4.transform.localEulerAngles = new Vector3(0,angle,0);
                        bullet1_4.transform.position = transform.position+Vector3.up;
                        //bullet1_4.transform.LookAt(targetEntity.transform.position);
                        bullet1_4.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                        bullet1_4.GetComponent<Rigidbody>().velocity = bullet1_4.transform.forward*8;
                        bullet1_4.GetComponent<Bullet>().off +=()=>Add1(bullet1_4);
                        bullet1_4.GetComponent<Bullet>().off +=()=>bullet1_4.SetActive(false);
                    }
                    yield return new WaitForSeconds(0.4f);
                    }
                }

                else if(2>=choose)
                {
                    for(int i = 0; i < 18; i++)
                    {
                    animator.SetTrigger("Attack_2");
                    
                    GameObject bullet1 = bullet2Q.Dequeue();
                    bullet1.SetActive(true);
                    bullet1.transform.position = transform.position+Vector3.up;
                    bullet1.transform.LookAt(targetEntity.transform.position);

                    bullet1.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                    bullet1.GetComponent<Rigidbody>().velocity = bullet1.transform.forward*9;
                    bullet1.GetComponent<Bullet>().off +=()=>Add2(bullet1);
                    bullet1.GetComponent<Bullet>().off +=()=>bullet1.SetActive(false);
                    yield return new WaitForSeconds(0.3f);                        
                    }

                }

  

            }

            else
                animator.SetBool("Moving",true);

                yield return new WaitForSeconds(1.2f);  
        }
    }

    private IEnumerator Stop()
    {
        while(true)
        {
            if(!dead)
            {
                Collider[] colliders=
                Physics.OverlapSphere(transform.position, 15f, whatIsTarget);

                if(colliders.Length>= 1)
                    pathFinder.isStopped = true;

                else
                    pathFinder.isStopped =false;                
            }
            yield return new WaitForSeconds(0.25f);            
        }

    }

    public void Add1(GameObject bullet)
    {
        if(!bullet1Q.Contains(bullet))
            bullet1Q.Enqueue(bullet);
    }

    public void Add2(GameObject bullet)
    {
        if(!bullet2Q.Contains(bullet))
            bullet2Q.Enqueue(bullet);
    }

    public override void Die() 
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        Collider[] enemyColliders = GetComponents<Collider>();


        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        animator.SetTrigger("Die");
        //enemyAudioPlayer.PlayOneShot(deathSound);
    }



}
