using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour,IItem
{
    // Start is called before the first frame update

    public int armor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Reinforce(int tech)
    {

    }


    public void Use(GameObject target)
    {
        target.GetComponent<PlayerHealth>().ChrageShield(armor);
        target.GetComponent<PlayerHealth>().RestoreHealth(armor);
    }
}
