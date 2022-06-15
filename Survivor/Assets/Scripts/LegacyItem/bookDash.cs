using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookDash : MonoBehaviour,IItem
{
    // Start is called before the first frame update
    public int damage;
    public int dashCool;
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
        target.GetComponent<PlayerMovement>().DashCoolDown -= dashCool;
        target.GetComponent<PlayerHealth>().maxHealth +=10;
        target.GetComponent<PlayerHealth>().maxShield +=10;
        target.GetComponent<PlayerHealth>().maxHealthUpdate(10);
        target.GetComponent<PlayerHealth>().maxShieldUpdate(10);
        return;
    }

    public void Reinforce(int tech)
    {

    }
}
