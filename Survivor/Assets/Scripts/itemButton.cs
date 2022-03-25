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



}
