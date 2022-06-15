using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour,IItemDrop
{
    public float cooldown = 0.25f;
    public GameObject text;
    //TextMeshPro content;
    string content;
    
    void Start()
    {
        content = "Dash cooldown -" + cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 60* Time.deltaTime, 0f);
    }

    public void Use(GameObject target)
    {
        target.GetComponent<PlayerMovement>().DashCoolDown-=cooldown;
        
        GameObject hudText = Instantiate(text);
        hudText.transform.position = transform.position + Vector3.up;
        hudText.GetComponent<FloatingDamage>().content = content;


        Destroy(gameObject);
    }
}
