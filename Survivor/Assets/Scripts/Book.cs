using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour, IItem
{
    // Start is called before the first frame update
    public int damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(GameObject target)
    {
        target.GetComponentInChildren<Gun>().damage += damage;

        return;
    }

    public void Reinforce(int tech)
    {

    }
}
