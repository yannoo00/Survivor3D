using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Battery : MonoBehaviour,IItemDrop
{
    public float shield = 30;
    public GameObject text;
    //TextMeshPro content;
    string content;
    
    void Start()
    {
        content = "Shield +" + shield;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 60* Time.deltaTime, 0f);
    }

    public void Use(GameObject target)
    {
        target.GetComponent<PlayerHealth>().ChrageShield(shield);
        
        GameObject hudText = Instantiate(text);
        hudText.transform.position = transform.position + Vector3.up;
        hudText.GetComponent<FloatingDamage>().content = content;


        Destroy(gameObject);
    }
}
