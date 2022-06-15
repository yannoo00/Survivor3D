using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour,IItem
{
    public int ammo=700;
    void Start()
    {

    }

    public void Use(GameObject target)
    {
        target.GetComponentInChildren<Gun>().ammoRemain+=ammo;
    }
    public void Reinforce(int tech)
    {

    }

    void Update()
    {
        
    }
}
