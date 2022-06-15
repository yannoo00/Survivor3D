using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrossShootingEnemy : LivingEntity
{
    public LayerMask whatIsTarget;
    public GameObject bulletPrefab;
    public ParticleSystem die;
    public AudioClip dieAudio;
    Queue<GameObject> bulletQ = new Queue<GameObject>();
    LivingEntity targetEntity;
    NavMeshAgent pathFinder;
    AudioSource audioSource;
    Animator animator;
    public Renderer enemyRenderer;
    public Material flashWhite;
    Material originColor;

    public float timeBetAttack = 1;
    float lastAttackTime;
    public int speed = 4;

    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }
    public void Setup(float newHealth,float newSpeed)
    {
        pathFinder = GetComponent<NavMeshAgent>();
        startingHealth = newHealth;
        health = newHealth;
        pathFinder.speed = newSpeed;
    }

    void Start()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        originColor = enemyRenderer.material;

        Collider[] colliders  =
        Physics.OverlapSphere(transform.position, 50f, whatIsTarget);

        for(int i =0; i < colliders.Length; i++)
        {
            LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

            if(livingEntity != null && !livingEntity.dead)
            {
                targetEntity = livingEntity;

                break;
            }
        }

        StartCoroutine(UpdatePath());

        for(int i = 0; i < 48; i++) 
        {
            GameObject bullet = Instantiate(bulletPrefab);    
            bulletQ.Enqueue(bullet);
            bullet.SetActive(false);        
        }

        StartCoroutine(Deal());
    }

    private IEnumerator UpdatePath() {
        // 살아있는 동안 무한 루프
        while (!dead)
        {
            if (hasTarget) //추적 대상 존재
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(
                    targetEntity.transform.position
                );
            }
            
            else
            {
                pathFinder.isStopped = true;

                Collider[] colliders  =
                Physics.OverlapSphere(transform.position, 50f, whatIsTarget);

                for(int i =0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    if(livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;

                        break;
                    }
                }
            }
            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }
    public override void OnDamage(float damage) {
        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        if(!dead)
        {
            GameObject hudText = Instantiate(hudDamageText); // 생성할 텍스트 오브젝트
            hudText.transform.position = transform.position + Vector3.up; // 표시될 위치
            hudText.GetComponent<FloatingDamage>().damage = (int)damage; // 데미지 전달

            audioSource.Play();
            StartCoroutine(HitEffect());
        }

        base.OnDamage(damage);
    }
    private IEnumerator HitEffect()
    {
        enemyRenderer.material = flashWhite;
        yield return new WaitForSeconds(0.05f);
        enemyRenderer.material = originColor;
    }
    public override void Die() 
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();
        animator.SetTrigger("Die");
        
        pathFinder.isStopped = true;
        pathFinder.enabled = false;
        if(dieAudio!=null)
            audioSource.PlayOneShot(dieAudio);
    }

    IEnumerator Deal()
    {
        while(!dead)
        {
            for(int i =0; i<6; i++)
            {
                GameObject bullet = bulletQ.Dequeue();
                bullet.SetActive(true);
                bullet.transform.position =transform.position+Vector3.up*0.5f;
                bullet.transform.eulerAngles = transform.eulerAngles+new Vector3(0,i*60,0);
                animator.SetTrigger("Attack1");
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward*10;
                bullet.GetComponent<Bullet>().off+=()=>Add(bullet);
                bullet.GetComponent<Bullet>().off+=()=>bullet.SetActive(false);
            }

            yield return new WaitForSeconds(2);
        }
    }

    void Update()
    {
        animator.SetBool("HasTarget",hasTarget);
    }
    private void Add(GameObject bullet)
    {
        if(!bulletQ.Contains(bullet))
            bulletQ.Enqueue(bullet);
    }
}
