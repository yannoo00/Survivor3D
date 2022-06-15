using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenchSet : MonoBehaviour,IItemDrop
{
    private int power = 2;
    public GameObject text;
    string content;

    void Start()
    {   
        content = "Gun Damage +" + power;

    }




    void Update()
    {
        transform.Rotate(0f, 60* Time.deltaTime, 0f);

    }
    public void Use(GameObject target)
    {
        target.GetComponentInChildren<Gun>().damage += power;

        GameObject hudText = Instantiate(text);
        hudText.transform.position = transform.position + Vector3.up;
        hudText.GetComponent<FloatingDamage>().content = content;

        Destroy(gameObject);
    }
}
