using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinegunHolder : ReinforceState,IItem  //15
{
    GameObject musket;

    


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        musket = Instantiate(gameObject) as GameObject;
        musket.transform.SetParent(target.transform.GetChild(0),false);
        transform.GetChild(0).GetComponent<Machinegun>().gun = target.GetComponentInChildren<Gun>();
        transform.GetChild(0).GetComponent<Machinegun>().playerInput = target.GetComponent<PlayerInput>();
        transform.GetChild(1).GetComponent<Machinegun>().gun = target.GetComponentInChildren<Gun>();
        transform.GetChild(1).GetComponent<Machinegun>().playerInput = target.GetComponent<PlayerInput>();
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                musket.transform.GetChild(0).GetComponent<Machinegun>().damage += 3;
                musket.transform.GetChild(1).GetComponent<Machinegun>().damage += 3;
                step++;
                break;

            case 1:
                musket.transform.GetChild(0).GetComponent<Machinegun>().damage += 5;
                musket.transform.GetChild(1).GetComponent<Machinegun>().damage += 5;
                step++;
                break;

            case 2:
                musket.transform.GetChild(0).GetComponent<Machinegun>().timeBetFire -= 0.03f;
                musket.transform.GetChild(1).GetComponent<Machinegun>().timeBetFire -= 0.03f;
                step++;
                break;
        }
    }

    public void SharpShooter()
    {
        musket.transform.GetChild(0).GetComponent<Machinegun>().damage += 8;
        musket.transform.GetChild(1).GetComponent<Machinegun>().damage += 8;
        Debug.Log("m damage:"+musket.transform.GetChild(1).GetComponent<Machinegun>().damage);
    }


}
