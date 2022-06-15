using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSpeed : MonoBehaviour, IItem
{
    // Start is called before the first frame update
    public int speed;
    public int cooldown;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(GameObject target)
    {
        target.GetComponent<PlayerMovement>().DashSpeed += speed;
        target.GetComponent<PlayerMovement>().DashCoolDown -=cooldown;
        return;
    }

    public void Reinforce(int tech)
    {

    }

}
