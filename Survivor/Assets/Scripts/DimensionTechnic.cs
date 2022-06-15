using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionTechnic : MonoBehaviour, IItem
{
    public int capacity;
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
        target.GetComponentInChildren<Gun>().magCapacity += capacity;

    }
}
