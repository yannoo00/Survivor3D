using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 
using UnityEngine.UI;

public class Chirone : LivingEntity
{
    public GameObject insectBullet;
    private LivingEntity targetEntity;
    public LayerMask whatIsTarget;
    private NavMeshAgent pathFinder;
    public AudioClip hitSound;
    private Animator enemyAnimator;
    private AudioSource audioSource;
    public Renderer enemyRenderer;
    public Material flashWhite;
    public Material originColor;
    public Slider slider;

    Queue<GameObject> insectQ = new Queue<GameObject>();

    public float damage = 40;
    public float timeBetAttack = 1f;
    public float timeBetShot = 5f;
    private float lastAttackTime;
    private float lastShotTime;
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
    private void Awake() {
    // 초기화
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        //enemyRenderer = GetComponentInChildren<Renderer>();
        
        originColor = enemyRenderer.material;

        for(int i =0; i<25; i++)
        {
            GameObject insect = Instantiate(insectBullet);
            insectQ.Enqueue(insect);
            insect.SetActive(false);
        }
    }

    void Start()
    {
        StartCoroutine(UpdatePath());

        StartCoroutine(Run());
        StartCoroutine(Deal());
    }
    private IEnumerator Run()
    {
        while(true)
        {
            enemyAnimator.SetBool("Close",true);            
            pathFinder.speed+=3f;

            yield return new WaitForSeconds(3f);

            enemyAnimator.SetBool("Close",false);            
            pathFinder.speed-=3f;
            
            yield return new WaitForSeconds(3f);
        }
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
            //hitEffect.transform.position = hitPoint;
            //hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal); //공격이 날아온 방향을 바라보는 방향(?)
            //hitEffect.Play();

            audioSource.PlayOneShot(hitSound);
            StartCoroutine(HitEffect());
            slider.value = health;
        }

        base.OnDamage(damage);
    }
    private IEnumerator HitEffect()
    {
        enemyRenderer.material = flashWhite;
        yield return new WaitForSeconds(0.05f);
        enemyRenderer.material = originColor;
    }
    IEnumerator Deal()
    {
        while(true)
        {
            if(!dead)
            {
                Collider[] other = Physics.OverlapSphere(transform.position,10f,whatIsTarget);
                if(other.Length>=1)
                {
                    LivingEntity attackTarget= other[0].GetComponent<LivingEntity>();
                    if(attackTarget!=null&&attackTarget==targetEntity)
                    {
                        lastShotTime = Time.time;
                        enemyAnimator.SetTrigger("Attack2");
                        for(int i =0; i<4; i++)
                        {
                            GameObject insect = insectQ.Dequeue();
                            insect.SetActive(true);
                            insect.transform.position = transform.position + Vector3.up;
                            insect.transform.eulerAngles = transform.eulerAngles+new Vector3(0,i*90,0);
                            insect.GetComponent<Rigidbody>().velocity = insect.transform.forward*14;                        
                            insect.GetComponent<Bullet>().off+=()=>Add(insect);
                            insect.GetComponent<Bullet>().off+=()=>insect.SetActive(false);
                        }
                        yield return new WaitForSeconds(0.5f);
                        for(int i =0; i<4; i++)
                        {
                            GameObject insect = insectQ.Dequeue();
                            insect.SetActive(true);
                            insect.transform.position = transform.position + Vector3.up;
                            insect.transform.eulerAngles = transform.eulerAngles+new Vector3(0,45+i*90,0);
                            insect.GetComponent<Rigidbody>().velocity = insect.transform.forward*14;                        
                            insect.GetComponent<Bullet>().off+=()=>Add(insect);
                            insect.GetComponent<Bullet>().off+=()=>insect.SetActive(false);
                        }

                    }
                }
            }
            yield return new WaitForSeconds(2.5f);
        }
    }

    public override void Die() {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        Collider[] enemyColliders = GetComponents<Collider>();

        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
        //enemyAudioPlayer.PlayOneShot(deathSound);
    }
    private void OnTriggerStay(Collider other) {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행   

        if(!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;

                enemyAnimator.SetTrigger("Attack2");
                attackTarget.OnDamage(damage);
            }
        }
    }

    public void FindPlayer()
    {
        if(!dead && Time.time>=lastShotTime+timeBetShot)
        {
            Collider[] other = Physics.OverlapSphere(transform.position,10f,whatIsTarget);
            if(other.Length>=1)
            {
                LivingEntity attackTarget= other[0].GetComponent<LivingEntity>();
                if(attackTarget!=null&&attackTarget==targetEntity)
                {
                    lastShotTime = Time.time;
                    enemyAnimator.SetTrigger("Attack2");
                    for(int i =0; i<4; i++)
                    {
                        GameObject insect = insectQ.Dequeue();
                        insect.SetActive(true);
                        insect.transform.position = transform.position + Vector3.up;
                        insect.transform.eulerAngles = transform.eulerAngles+new Vector3(0,i*90,0);
                        insect.GetComponent<Rigidbody>().velocity = insect.transform.forward*15;                        
                        insect.GetComponent<Bullet>().off+=()=>Add(insect);
                        insect.GetComponent<Bullet>().off+=()=>insect.SetActive(false);
                    
                    }

                }
            }
        }
    }
    private void Add(GameObject bullet)
    {
        if(!insectQ.Contains(bullet))
            insectQ.Enqueue(bullet);
    }


    void Update()
    {
        enemyAnimator.SetBool("HasTarget",hasTarget);
    }
}
