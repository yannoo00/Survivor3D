using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicShield : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0,60*Time.deltaTime,0f);
        
        transform.position = transform.parent.position;
    }
}
