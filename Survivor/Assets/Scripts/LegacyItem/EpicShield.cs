using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpicShield : ReinforceState,IItem
{
    GameObject epicShield;
    public int speed = 60;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0f,speed*Time.deltaTime, 0f);
    }

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;        
        epicShield = Instantiate(gameObject) as GameObject;
        epicShield.transform.SetParent(target.transform,false);
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                epicShield.transform.localScale += new Vector3(0.5f,0.5f,0);
                step++;
                break;

            case 1:
                epicShield.transform.localScale += new Vector3(0.5f,0.5f,0);
                step++;
                break;

            case 2:
                epicShield.GetComponent<EpicShield>().speed += 40;
                step++;
                break;
        }
    }
}
