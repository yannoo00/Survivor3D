using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfDevilHunter : ReinforceState,IItem //num 8
{
    GameObject spirit;
    GameObject playerFor2;

    
    public int damage = 20;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                playerFor2.GetComponentInChildren<Gun>().thirdDamage += 20;
                step++;
                break;

            case 1:
                playerFor2.GetComponentInChildren<Gun>().thirdDamage += 10;
                step++;
                break;
                
            case 2:
                playerFor2.GetComponentInChildren<Gun>().thirdDamage += 100;
                step++;
                break;
        }
    }

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        spirit = Instantiate(gameObject) as GameObject;
        spirit.transform.SetParent(target.transform,false);

        target.GetComponentInChildren<Gun>().thirdDamage = damage;
        target.GetComponentInChildren<Gun>().spirit =true;
        playerFor2 = target;
    }
    
}
