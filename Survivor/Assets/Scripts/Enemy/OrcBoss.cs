using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrcBoss : LivingEntity
{

    public LayerMask whatIsTarget; // 추적 대상 레이어
    private LivingEntity targetEntity; // 추적할 대상
    private NavMeshAgent pathFinder; // 경로계산 AI 에이전트

    //public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과
    //public AudioClip deathSound; // 사망시 재생할 소리
    public AudioClip hitSound; // 피격시 재생할 소리

    private Animator enemyAnimator; // 애니메이터 컴포넌트
    private AudioSource enemyAudioPlayer; // 오디오 소스 컴포넌트
    public Renderer enemyRenderer; // 렌더러 컴포넌트
    
    public Material flashWhite;
    public Material originColor;

    public float damage = 40f; // 공격력
    public float timeBetAttack = 1f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점
    public float runSpeed=3f;

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
        enemyAudioPlayer = GetComponent<AudioSource>();

        //enemyRenderer = GetComponentInChildren<Renderer>();
        
        originColor = enemyRenderer.material;
    }


    void Start()
    {
        StartCoroutine(UpdatePath());

        StartCoroutine(Run());
    }

    
    void Update()
    {
        enemyAnimator.SetBool("HasTarget", hasTarget);

        // if(hasTarget)
        // {
        //     if(Vector3.Distance(targetEntity.transform.position,transform.position)<5f)
        //     {
        //         enemyAnimator.SetBool("Close",true);
        //         pathFinder.speed += 3f;
        //     } 
        // }
    }
    private IEnumerator Run()
    {
        while(true)
        {
            enemyAnimator.SetBool("Close",true);            
            pathFinder.speed+=runSpeed;

            yield return new WaitForSeconds(3f);

            enemyAnimator.SetBool("Close",false);            
            pathFinder.speed-=runSpeed;
            
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

            enemyAudioPlayer.PlayOneShot(hitSound);
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

                int action = Random.Range(0,2);
                if(action!=0)
                    enemyAnimator.SetTrigger("Attack2");
                else
                    enemyAnimator.SetTrigger("Attack1");

                attackTarget.OnDamage(damage);
            }
        }
    }

}
