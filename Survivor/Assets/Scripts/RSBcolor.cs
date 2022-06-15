using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RSBcolor : MonoBehaviour
{
    public int buttonNumber;
    public bool first = true;
    public GameObject[] buttons;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ColorChange()
    {
        if(first)
        {
            gameObject.transform.parent.GetComponent<Image>().color=new Color(0,255,255,255);
            first=false;
        }
            
        else
        {
            gameObject.transform.parent.GetComponent<Image>().color=new Color(255,255,255,255);
            first=true;
        }
            
    }

    public void AllColorReset()
    {
        for(int i=0; i<20; i++)
        {
            buttons[i].GetComponent<Image>().color=new Color(255,255,255,255);
            buttons[i].GetComponentInChildren<RSBcolor>().first=true;
        }
    }
}
