using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LivingFire : MonoBehaviour
{
    
    private NavMeshAgent pathFinder;
    public LayerMask whatIsTarget;
    private LivingEntity targetEntity;

    public float damage;
    private float timeBetAttack = 0.5f;
    private float lastAttackTime = 0;
    //public float speed;

    private bool hasTarget
    {
        get
        {
            if(targetEntity!=null&&!targetEntity.dead)
                return true;
            
            return false;
        }
    }



    void Start()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdatePath());
    }

    void OnEnable()
    {
        
    }

    
    void Update()
    {
        
    }

    private IEnumerator UpdatePath()
    {
        while(true)
        {
            if(hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;
                Collider[] colliders =
                Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                for(int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity =colliders[i].GetComponent<LivingEntity>();
                    if(livingEntity!=null && !livingEntity.dead)
                    {
                        targetEntity= livingEntity;
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag=="Enemy")
        {
            IDamageable target = other.GetComponent<IDamageable>();

            if(target != null)
                target.OnDamage(damage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Enemy")
        {
            if(Time.time >= lastAttackTime + timeBetAttack)
            {
                IDamageable target = other.GetComponent<IDamageable>();

                if(target != null)
                    {
                        target.OnDamage(damage);
                        lastAttackTime = Time.time;
                    }
                
            }
        }
    }
}
