using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormTrainer : MonoBehaviour,IItem
{
    public int speed = 150;

    void Start()
    {
        
    }


    void Update()
    {
        transform.Rotate(0f,speed*Time.deltaTime, 0f);
    }

    public void Use(GameObject target)
    {

    }

    public void Reinforce(int tech)
    {

    }


}
