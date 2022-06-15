using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour,IItemDrop
{
    public int Gem = 500;
    public GameObject text;
    string content;
    void Start()
    {
        content = "Gem +"+Gem;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 60* Time.deltaTime, 0f);
    }


    public void Use(GameObject target)
    {
        GameManager.instance.AddGem(Gem);

        GameObject hudText = Instantiate(text);
        hudText.transform.position = transform.position + Vector3.up;
        hudText.GetComponent<FloatingDamage>().content = content;
    
        Destroy(gameObject);
    }
}
