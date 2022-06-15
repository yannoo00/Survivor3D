using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AxeHolder : MonoBehaviour
{
    public event Action off;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Boundary")
            off();
    }

}
