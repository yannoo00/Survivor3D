using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartGem : MonoBehaviour,IItemDrop
{

    public int health = 40;
    public GameObject text;
    string content;
    void Start()
    {
        content = "Health +"+health;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 60* Time.deltaTime, 0f);
    }

    public void Use(GameObject target)
    {

        LivingEntity life = target.GetComponent<LivingEntity>();

        if (life != null)
        {
            // 체력 회복 실행
            life.RestoreHealth(health);
        }

        GameObject hudText = Instantiate(text);
        hudText.transform.position = transform.position + Vector3.up;
        hudText.GetComponent<FloatingDamage>().content = content;

    
        Destroy(gameObject);
    }
}
