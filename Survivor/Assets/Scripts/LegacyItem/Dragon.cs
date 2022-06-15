using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dragon : MonoBehaviour
{
    //플레이어 일정 거리 두고 따라다님.
    //주위에 적 있으면 두 세마리한테 단일 공격
    //공격 타입이 매직클로, 폭발 두 가지로 나뉨.

    public GameObject magicCloPrefab; //공격 타입 1
    public GameObject magicExplodePrefab; //공격 타입 2
    public LayerMask whatIsTarget; //따라다닐 플레이어
    
    public LayerMask whatIsTarget2; //공격할 적
    
    NavMeshAgent pathFinder;
    LivingEntity targetEntity;
    Transform targetTrans;

    Animator dragonAnimator; 
    Queue<GameObject> cloQ = new Queue<GameObject>();
    Queue<GameObject> exploQ = new Queue<GameObject>();
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
    int basicDamage = 90;

    void Start()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        dragonAnimator = GetComponent<Animator>();

        StartCoroutine(UpdatePath());

        for(int i =0; i< 24; i++) //16개씩 넣고
        {
            GameObject magicClo = Instantiate(magicCloPrefab);
            cloQ.Enqueue(magicClo);
            magicClo.SetActive(false);

            GameObject magicExplode = Instantiate(magicExplodePrefab);
            exploQ.Enqueue(magicExplode);
            magicExplode.SetActive(false);
        }

        StartCoroutine(Deal());
    }

    void Update()
    {
        
    }

    private IEnumerator UpdatePath() //플레이어 찾아 쫓아댕기기
    {
        // 살아있는 동안 무한 루프
        while(true)
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
                Physics.OverlapSphere(transform.position, 30f, whatIsTarget);

                for(int i =0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    if(livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        //hasTarget = true;
                        break;
                    }
                }
            }
            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
        
    }

    private IEnumerator Deal()
    {
        while(true)
        {
            for(int i = 0; i<1;i++) 
            {
                Collider[] colliders = 
                Physics.OverlapSphere(transform.position, 10f, whatIsTarget2);
                int limit = 16;
                int cnt = 0;

                for(int j =0; j<colliders.Length; j++)
                {
                    cnt++;

                    if(cnt>=limit)
                        break;
                    

                    LivingEntity livingEntity =
                    colliders[j].GetComponent<LivingEntity>();

                    if(livingEntity!=null && !livingEntity.dead)
                    {
                        targetTrans =
                        colliders[j].GetComponent<Transform>();

                                        
                        
                
                        if(targetTrans!=null)
                        {
                            dragonAnimator.SetTrigger("Attack");
                            int pick = Random.Range(0,4);

                            if(pick == 0 || pick== 1 || pick ==2)
                            //if(pick == 4)
                            {
                                
                                livingEntity.OnDamage(90);
                                GameObject clo = cloQ.Dequeue();
                                clo.SetActive(true);
                                clo.transform.position = targetTrans.position+Vector3.up;
                                clo.GetComponent<CloOrigin>().off +=()=> cloQ.Enqueue(clo);
                                clo.GetComponent<CloOrigin>().off +=()=> clo.SetActive(false);
                            }
                            else
                            {
                                //dragonAnimator.SetTrigger("Attack2");
                                livingEntity.OnDamage(180);
                                GameObject explode = exploQ.Dequeue();
                                explode.SetActive(true);
                                explode.transform.position = targetTrans.position+Vector3.up;
                                explode.GetComponent<MagicExplodeOrigin>().off +=()=> exploQ.Enqueue(explode);
                                explode.GetComponent<MagicExplodeOrigin>().off +=()=> explode.SetActive(false);
                            }
                        }
                            //break;
                    }
                }

            }


            yield return new WaitForSeconds(1.75f);
        }
    }

}
