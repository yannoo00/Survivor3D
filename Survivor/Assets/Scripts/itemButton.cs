using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemButton : MonoBehaviour
{
   public GameObject ItemAllocater;
    public int num;

    public void Input()
    {
        ItemAllocater.GetComponent<ItemAllocater>().OnClickButton(num);
    }  

    // public void Input_r()
    // {
    //     ItemAllocater.GetComponent<ItemAllocater>().OnClickButton_r(num);
    // }

     public void reinforce()
     {
         ItemAllocater.GetComponent<ItemAllocater>().OnClickButton2(num);
     }



}
