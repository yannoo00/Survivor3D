using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneGuardian : ReinforceState,IItem    // num 5
{

    GameObject droneGuardian;
    public GameObject playerFor2;
    public GameObject playerForCor;
    public bool shieldReinforce =false;
  
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
       StartCoroutine(GenerateShield());
    }

    
    void Update()
    {
        transform.Rotate(0f, 80* Time.deltaTime, 0f);
    }

    public void Reinforce(int tech)
    {


         switch(tech)
         {
            case 0:

                playerFor2.GetComponentInChildren<Gun>().magCapacity += 10;
                playerFor2.GetComponentInChildren<Gun>().ammoRemain += 200;
                step++;
                break;

            case 1:

                playerFor2.GetComponentInChildren<Gun>().reloadTime -= 0.2f;
                step++;
                break;

            case 2:

                playerFor2.GetComponentInChildren<Gun>().reloadTime -= 0.3f;
                step++;
                break;

            case 3:
                playerFor2.GetComponentInChildren<Gun>().magCapacity += 15;
                playerFor2.GetComponentInChildren<Gun>().ammoRemain += 400;
                step++;
                break;

            case 4:

                playerFor2.GetComponentInChildren<Gun>().damage += 9;
                step++;
                break;
    
            case 5:

                playerFor2.GetComponentInChildren<Gun>().damage += 36;
                step++;
                break;

            case 6:

                droneGuardian.GetComponent<DroneGuardian>().shieldReinforce = true;
                step++;
                //StartCoroutine(droneGuardian.GetComponent<DroneGuardian>().GenerateShield());
                break;
         }


    }

    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        droneGuardian = Instantiate(gameObject) as GameObject;
        droneGuardian.transform.SetParent(target.transform.GetChild(0),false);

        target.GetComponentInChildren<Gun>().magCapacity += 10;
        target.GetComponentInChildren<Gun>().ammoRemain += 500;
        target.GetComponentInChildren<Gun>().reloadTime -= 0.3f;
        target.GetComponentInChildren<Gun>().damage += 2;
        
        playerFor2 = target;
        //드론가디언은 프리팹 자체에서 강화효과만 주면 돼서 이런 식으로 저장

        droneGuardian.GetComponent<DroneGuardian>().playerForCor = target;


        

        return;
    }

    private IEnumerator GenerateShield()
    {
        while(true)
        {
            if(shieldReinforce)
                {
                    playerForCor.GetComponent<PlayerHealth>().ChrageShield(15);
                    audioSource.Play();
                    yield return new WaitForSeconds(15f);
                }    

            yield return new WaitForSeconds(0.5f);
        }
    }

}
