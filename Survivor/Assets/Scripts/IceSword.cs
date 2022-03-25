using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSword : MonoBehaviour, IItem
{
    //플레이어 child로 들어가면 플레이어가 회전할 때 같이 회전함
    public GameObject player;


    public float speed = 1f;
    public float damage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Use(GameObject target)
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
         transform.RotateAround(player.transform.position, Vector3.down, speed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Enemy")
        {
            IDamageable target = other.GetComponent<IDamageable>();

            if(target != null)
                target.OnDamage(damage);
        }

    }
}
