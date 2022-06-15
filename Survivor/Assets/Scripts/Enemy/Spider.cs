using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spider : LivingEntity
{


    private NavMeshAgent pathFinder;

    private Animator enemyAnimator;

    public Renderer enemyRenderer;
    public Material flashWhite;
    public Material originColor;

    public ParticleSystem dustExplosion;
    public LayerMask whatIsTarget;
    //public AudioClip boomSound;
    public AudioClip hitSound;
    ParticleSystem particle;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //particle = Instantiate(dustExplosion);

        //dustExplosion.Pause();
        originColor = enemyRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpiderSetup(float newHealth, float newSpeed) {
    
    startingHealth = newHealth;
    
    health = newHealth;

    //damage = newDamage;

    pathFinder.speed = newSpeed;

    //enemyRenderer.material.color = skinColor;
    }

    private IEnumerator UpdatePath(){

        while(!dead)
        {
            //pathFinder.SetDestination(GetRandomPositionInNavMesh(transform.position,10f));
            enemyAnimator.SetBool("HasTarget",true);

            Collider[] colliders  =
            Physics.OverlapSphere(transform.position, 50f, whatIsTarget);
            
            if(colliders.Length>0)
            {
                pathFinder.SetDestination(colliders[0].transform.position);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    private Vector3 GetRandomPositionInNavMesh(Vector3 center, float distance)
    {
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos,out hit, distance, NavMesh.AllAreas);

        return hit.position;
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
            audioSource.PlayOneShot(hitSound);

            //enemyAudioPlayer.PlayOneShot(hitSound);
            StartCoroutine(HitEffect());
        }

        base.OnDamage(damage);
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

        ParticleSystem dust = Instantiate(dustExplosion,gameObject.transform.position,Quaternion.identity);
        dust.Play();
        
        //audioSource.PlayOneShot(boomSound);

        Collider[] colliders = 
        Physics.OverlapSphere(transform.position,4f,whatIsTarget);
        if(colliders.Length>0)
        {
            LivingEntity livingEntity = colliders[0].GetComponent<LivingEntity>();

            if(livingEntity!=null)
                livingEntity.OnDamage(60);
        }        
        //dustExplosion.transform.position = gameObject.transform.position;
        //dustExplosion.Play();
        
        //enemyAnimator.SetTrigger("Die");
        //enemyAudioPlayer.PlayOneShot(deathSound);
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private IEnumerator HitEffect()
    {
        enemyRenderer.material = flashWhite;
        yield return new WaitForSeconds(0.05f);
        enemyRenderer.material = originColor;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            Die();
    }

}
