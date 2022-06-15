using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem01 : MonoBehaviour,IItemDrop
{
    
    public int Gem = 100;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(GameObject target)
    {
        GameManager.instance.AddGem(Gem);
        Destroy(gameObject);
    }
}
