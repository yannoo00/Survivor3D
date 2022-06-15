using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketHolder : ReinforceState,IItem  //15
{
    GameObject musket;

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
        musket = Instantiate(gameObject) as GameObject;
        musket.transform.SetParent(target.transform.GetChild(0),false);
        transform.GetChild(0).GetComponent<Musket>().gun = target.GetComponentInChildren<Gun>();
        transform.GetChild(0).GetComponent<Musket>().playerInput = target.GetComponent<PlayerInput>();
        transform.GetChild(1).GetComponent<Musket>().gun = target.GetComponentInChildren<Gun>();
        transform.GetChild(1).GetComponent<Musket>().playerInput = target.GetComponent<PlayerInput>();
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                musket.transform.GetChild(0).GetComponent<Musket>().damage += 5;
                musket.transform.GetChild(1).GetComponent<Musket>().damage += 5;
                step++;
                break;

            case 1:
                musket.transform.GetChild(0).GetComponent<Musket>().damage += 7;
                musket.transform.GetChild(1).GetComponent<Musket>().damage += 7;
                step++;
                break;

            case 2:
                musket.transform.GetChild(0).GetComponent<Musket>().damage += 10;
                musket.transform.GetChild(1).GetComponent<Musket>().damage += 10;
                step++;
                break;
        }
    }
    public void ShaprShooter()
    {
        musket.transform.GetChild(0).GetComponent<Musket>().damage += 8;
        musket.transform.GetChild(1).GetComponent<Musket>().damage += 8;
    }
}
