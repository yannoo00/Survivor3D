using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meth : MonoBehaviour,IItem
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void Reinforce(int tech)
    {

    }

    public void Use(GameObject target)
    {
        // int pick = Random.Range(0,4);
        // if(pick == 0)       
        //     target.GetComponentInChildren<Gun>().damage += 5;
        // else if(pick == 1)
        //     target.GetComponent<PlayerMovement>().moveSpeed += 1.2f;
        // else if(pick == 2)
        //     target.GetComponentInChildren<Gun>().magCapacity += 30;
        // else
        //     target.GetComponent<LivingEntity>().OnDamage(20);

        GameManager.instance.AddGem(2000);
    }
}
