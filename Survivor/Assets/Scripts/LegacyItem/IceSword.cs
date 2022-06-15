using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSword : ReinforceState, IItem
{
    //0번
    //플레이어 child로 들어가면 플레이어가 회전할 때 같이 회전함
    private GameObject player;

    GameObject playerFor2;
    private GameObject iceSword;
    GameObject iceSword2;
    [HideInInspector]
    public float speed = 200f;
    [HideInInspector]
    public float damage = 50f;
    // {get; private set;}
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject;
        //speed = 200f;
        //damage = 50f;

    }

    //퍼블릭 아이스스워드 게임오브젝트를 3개를 만든다.
    //그 아이스스워드들은 충돌시 데미지를 주는 기능만 잇다.(생성 뭐 이런거 없음)
    //걔네들을 부모를 플레이어로 해서 생성하고 돌게하면 된다.
    //변수로 데미지, 스피드를 가지고 거기에 더해서 

    //얘는 4.5 , - 0.4 , -0.5;
    //매번 웨이브 새로 시작할 때마다 위치 초기화 필요.
    // 

    public void Reinforce(int tech)
    {
        switch(tech)
        {
            case 0:
                //Debug.Log("current damage : " + iceSword.GetComponent<IceSword>().damage);
                iceSword.GetComponent<IceSword>().damage += 30;
                //Debug.Log("damage up : " +  iceSword.GetComponent<IceSword>().damage);

                if(iceSword2 != null)
                    iceSword2.GetComponent<IceSword>().damage += 30;
                step++;
                break;

            case 1:
                //Debug.Log("current speed : " + speed);
                iceSword.GetComponent<IceSword>().speed +=100;
                if(iceSword2 != null)
                    iceSword2.GetComponent<IceSword>().speed += 100;
                //Debug.Log("Speed Up : "+speed);
                step++;
                break;

            case 2:

                iceSword.transform.localPosition = new Vector3(4.5f, -0.4f, -0.5f);
                iceSword.transform.localEulerAngles = new Vector3(90,-20,0);

                iceSword2 = Instantiate(gameObject) as GameObject;
                

                iceSword2.transform.SetParent(playerFor2.transform,false);
                
                iceSword2.transform.localPosition = new Vector3(-5f,-0.4f,-0.3f);
                //iceSword2.transform.position = new Vector3(iceSword.transform.position.x,iceSword.transform.position.y,iceSword.transform.position.z);

                iceSword2.transform.localEulerAngles = new Vector3(90,143,0);
                //iceSword2.transform.Rotate(Vector3.forward,180);

                iceSword2.GetComponent<IceSword>().damage = iceSword.GetComponent<IceSword>().damage;
                iceSword2.GetComponent<IceSword>().speed = iceSword.GetComponent<IceSword>().speed;
                iceSword2.transform.localScale = iceSword.transform.localScale;
                step++;
                break;



            case 3:
                iceSword.transform.localScale += new Vector3(1,1,1);

                if(iceSword2 != null)
                    iceSword2.transform.localScale += new Vector3(1,1,1);
                step++;
                break;


            case 4:
                iceSword.GetComponent<IceSword>().speed += 150;
                if(iceSword2 != null)
                    iceSword2.GetComponent<IceSword>().speed += 150;
                step++;
                break;


            case 5:
                iceSword.GetComponent<IceSword>().speed += 50;
                if(iceSword2 != null)
                    iceSword2.GetComponent<IceSword>().speed += 50;

                iceSword.GetComponent<IceSword>().damage += 20;
                
                if(iceSword2 != null)
                    iceSword2.GetComponent<IceSword>().damage += 20;
                step++;
                break;
            
        }

    }


    public void Use(GameObject target)
    {
        step = 0; //초기화 해주기
        slotNum =0;
        iceSword = Instantiate(gameObject) as GameObject;
        iceSword.transform.SetParent(target.transform,false);
        iceSword.GetComponent<IceSword>().damage = 50;
        iceSword.GetComponent<IceSword>().speed = 200;

        playerFor2 = target;
        return;
    }

    // Update is called once per frame
    void Update()
    {
         transform.RotateAround(player.transform.position, Vector3.down, speed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Enemy")
        {
            IDamageable target = other.GetComponent<IDamageable>();

            if(target != null)
                target.OnDamage(damage);
        }

    }
}
