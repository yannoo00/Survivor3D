using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kimchi : MonoBehaviour, IItem
{
    public float speed;

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
        target.GetComponent<PlayerMovement>().moveSpeed += speed;
        
        return;
    }
}
