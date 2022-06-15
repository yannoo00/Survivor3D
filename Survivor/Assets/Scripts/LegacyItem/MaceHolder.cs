using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceHolder : ReinforceState,IItem //num22
{
    GameObject maceHolder;
    public int speed=100;

    void Start()
    {
        
    }
    void Update()
    {
        transform.Rotate(0,speed*Time.deltaTime,0);
    }

    public void Use(GameObject target)
    {
        step=0;
        slotNum=0;
        maceHolder = Instantiate(gameObject) as GameObject;
        maceHolder.transform.SetParent(target.transform,false);
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                maceHolder.transform.GetChild(2).gameObject.SetActive(true);
                maceHolder.transform.GetChild(3).gameObject.SetActive(true);
                step++;
                break;
            case 1:
                maceHolder.transform.GetChild(4).gameObject.SetActive(true);
                maceHolder.transform.GetChild(5).gameObject.SetActive(true);
                step++;
                break;
            case 2:
                maceHolder.GetComponent<MaceHolder>().speed+=50;
                step++;
                break;
        }
    }
}
