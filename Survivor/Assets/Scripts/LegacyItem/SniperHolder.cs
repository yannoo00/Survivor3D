using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperHolder : ReinforceState,IItem  //17
{
    GameObject sniper;

    //public int damage;


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
        sniper = Instantiate(gameObject) as GameObject;
        sniper.transform.SetParent(target.transform.GetChild(0),false);
        transform.GetChild(0).GetComponent<Sniper>().gun = target.GetComponentInChildren<Gun>();
        transform.GetChild(0).GetComponent<Sniper>().playerInput = target.GetComponent<PlayerInput>();
        transform.GetChild(1).GetComponent<Sniper>().gun = target.GetComponentInChildren<Gun>();
        transform.GetChild(1).GetComponent<Sniper>().playerInput = target.GetComponent<PlayerInput>();
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                sniper.transform.GetChild(0).GetComponent<Sniper>().timeBetFire -= 0.1f;
                sniper.transform.GetChild(1).GetComponent<Sniper>().timeBetFire -= 0.1f;
                step++;
                break;

            case 1:
                sniper.transform.GetChild(0).GetComponent<Sniper>().damage += 12;
                sniper.transform.GetChild(1).GetComponent<Sniper>().damage += 12;
                step++;
                break;

            case 2:
                sniper.transform.GetChild(0).GetComponent<Sniper>().timeBetFire -= 0.1f;
                sniper.transform.GetChild(1).GetComponent<Sniper>().timeBetFire -= 0.1f;
                step++;
                break;
        }
    }
    public void SharpShooter()
    {
        sniper.transform.GetChild(0).GetComponent<Sniper>().damage += 8;
        sniper.transform.GetChild(1).GetComponent<Sniper>().damage += 8;
    }
}
