using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kimchi : MonoBehaviour, IItem
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
        target.GetComponent<PlayerMovement>().moveSpeed += 10f;
        
        return;
    }
}
