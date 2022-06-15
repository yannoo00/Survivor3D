using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musket : MonoBehaviour
{
    public LayerMask layerMask;
    public Transform fireTransform;

    private Vector3 realFireTransform;
    public ParticleSystem muzzleFlashEffect;
    private LineRenderer bulletLineRenderer;

    public int damage = 20;
    private float fireDistance = 20f;
    private float timeBetFire = 0.12f;
    private float lastFireTime;
////////////////////////////////////////////////////

    GameObject musket;
    public Gun gun;
    public PlayerInput playerInput;


    // public void Use(GameObject target)
    // {
    //     musket = Instantiate(gameObject) as GameObject;
    //     musket.transform.SetParent(target.transform);
    //     musket.GetComponent<Musket>().gun = target.GetComponentInChildren<Gun>();
    //     musket.GetComponent<Musket>().playerInput = target.GetComponentInChildren<PlayerInput>();
    // }

    void Start()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.enabled= false;
    }

    void Update()
    {
        if(playerInput!=null)
        {
            if(playerInput.fire)
                Fire();
            realFireTransform = fireTransform.position;            
        }
        else
        {
            playerInput = transform.parent.transform.parent.transform.parent.GetComponent<PlayerInput>();
            gun = transform.parent.transform.parent.GetComponentInChildren<Gun>();
        }
    }

    void Fire()
    {
        if(gun.state == Gun.State.Ready&&Time.time>=lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }

    }

    void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if(Physics.Raycast(realFireTransform,-fireTransform.right,out hit, fireDistance,layerMask))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if(target!=null && hit.collider.tag =="Enemy")
            {
                target.OnDamage(damage);
            }
            hitPosition = hit.point;
        }
        else
            hitPosition = realFireTransform+fireTransform.right*-fireDistance;
        
        
        StartCoroutine(ShotEffect(hitPosition));
    }
    

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();

        bulletLineRenderer.SetPosition(0,realFireTransform);
        bulletLineRenderer.SetPosition(1,hitPosition);
        bulletLineRenderer.enabled= true;

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled= false;
    }



}
