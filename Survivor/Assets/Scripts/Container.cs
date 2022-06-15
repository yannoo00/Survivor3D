using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Container : MonoBehaviour
{
    public TextMeshProUGUI info;

    public GameObject[] skills;





    void Start()
    {
        float time;
        time = PlayerPrefs.GetFloat("currentTime",0);
        int trial;
        trial = PlayerPrefs.GetInt("trial",0);
        info.text = "Best Record: "+ time +"seconds" + "\nTrials: "+trial;

        int berserkerSoul=PlayerPrefs.GetInt("berserkerSoul",0);
        if(berserkerSoul==1)
        {
            skills[0].GetComponent<Image>().color = new Color(255,255,255,255);
            skills[0].GetComponent<Button>().enabled = true;
        }

        int swordMaster=PlayerPrefs.GetInt("swordMaster",0);
        if(swordMaster==1)
        {
            skills[1].GetComponent<Image>().color = new Color(255,255,255,255);
            skills[1].GetComponent<Button>().enabled = true;
        }

        int summoner=PlayerPrefs.GetInt("summoner",0);
        if(summoner==1)
        {
            skills[2].GetComponent<Image>().color = new Color(255,255,255,255);
            skills[2].GetComponent<Button>().enabled = true;
        }
        int sharpShooter=PlayerPrefs.GetInt("sharpShooter",0);
        if(sharpShooter==1)
        {
            skills[3].GetComponent<Image>().color = new Color(255,255,255,255);
            skills[3].GetComponent<Button>().enabled = true;
        }
        int highTemplar=PlayerPrefs.GetInt("highTemplar",0);
        if(highTemplar==1)
        {
            skills[4].GetComponent<Image>().color = new Color(255,255,255,255);
            skills[4].GetComponent<Button>().enabled = true;
        }
        int hellWalker=PlayerPrefs.GetInt("hellWalker",0);
        if(hellWalker==1)
        {
            skills[5].GetComponent<Image>().color = new Color(255,255,255,255);
            skills[5].GetComponent<Button>().enabled = true;
        }
        int ironMan=PlayerPrefs.GetInt("ironMan",0);
        if(ironMan==1)
        {
            skills[6].GetComponent<Image>().color = new Color(255,255,255,255);
            skills[6].GetComponent<Button>().enabled = true;
        }
        int alchemist=PlayerPrefs.GetInt("alchemist",0);
        if(alchemist==1)
        {
            skills[7].GetComponent<Image>().color = new Color(255,255,255,255);
            skills[7].GetComponent<Button>().enabled = true;
        }
        int barbarian=PlayerPrefs.GetInt("barbarian",0);
        if(barbarian==1)
        {
            skills[8].GetComponent<Image>().color = new Color(255,255,255,255);
            skills[8].GetComponent<Button>().enabled = true;
        }


    }
    void Update()
    {
        
    }
}
