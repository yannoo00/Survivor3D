using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour, IItem
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(GameObject target)
    {
        target.GetComponent<PlayerHealth>().startingHealth = 500;

        return;
    }
}
