using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Inferno : MonoBehaviour
{
    public GameObject infernoProjectilePrefab;
    public LayerMask whatIsTarget;

    public Queue<GameObject> infernoQ = new Queue<GameObject>();
    Transform targetTrans;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        for(int i =0; i< 25; i++)
        {
            GameObject inferno = Instantiate(infernoProjectilePrefab);
            infernoQ.Enqueue(inferno);
            inferno.SetActive(false);
        }
        StartCoroutine(Deal());
    }


    void Update()
    {
        transform.Rotate(0f, 30* Time.deltaTime, 0f);
    }
    
    private IEnumerator Deal()
    {
        while(true)
        {
            Collider[] colliders = 
            Physics.OverlapSphere(transform.position,15f,whatIsTarget);

            if(colliders.Length>0)
            {
                LivingEntity livingEntity =
                colliders[colliders.Length-1].GetComponent<LivingEntity>();

                if(livingEntity!=null&&!livingEntity.dead)
                {
                    targetTrans =
                    colliders[colliders.Length-1].GetComponent<Transform>();


                    //for(int i =0; i < 4; i ++)
                    //{
                        GameObject inferno = infernoQ.Dequeue();
                        inferno.SetActive(true);
                        inferno.transform.position = gameObject.transform.position;
                        inferno.transform.LookAt(targetTrans.position);
                        
                        inferno.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                        inferno.GetComponent<Rigidbody>().velocity = inferno.transform.forward*15;
                        audioSource.Play();
                        inferno.GetComponent<InfernoProjectile>().off+=()=> Add(inferno);
                        inferno.GetComponent<InfernoProjectile>().off+=()=>inferno.SetActive(false);
                
                        //yield return new WaitForSeconds(0.125f);                            
                    //}
                    
                
            
                //break;
                }                
            }


            yield return new WaitForSeconds(0.125f);
        }
    }
    public void Add(GameObject inferno)
    {
        if(!infernoQ.Contains(inferno))
            infernoQ.Enqueue(inferno);
    }


    GameObject FindClosestEnemy() {

        GameObject[] gos;

        gos = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = null;

        float distance = Mathf.Infinity;

        Vector3 position = transform.position;

        foreach (GameObject go in gos) 
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance) 
            {
                if(!go.GetComponent<LivingEntity>().dead)
                {
                    closest = go;
                    distance = curDistance;                    
                }

            }
        }


        return closest;
    }
}
