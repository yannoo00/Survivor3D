using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardPadding : MonoBehaviour,IItem
{
    public int shieldAmount=15;
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
        target.GetComponent<PlayerHealth>().maxShield+=shieldAmount;
        target.GetComponent<PlayerHealth>().maxShieldUpdate(shieldAmount);
    }
}
