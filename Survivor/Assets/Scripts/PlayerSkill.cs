using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    
    public bool[] skills = new bool[9];
    public GameObject[] skillAlarms;
    public GameObject slashPrefab;
    public GameObject dragonPrefab;
    public GameObject infernoPrefab;
    public GameObject EnergyPrefab;
    public GameObject doombringerPrefab;
    private Queue<GameObject> slashQ = new Queue<GameObject>();
    private Queue<GameObject> energyQ = new Queue<GameObject>();
    public Image[] skillImages;
    public AudioClip energyBallSound;
    public LayerMask whatIsTarget;
    AudioSource audioSource;
    Transform targetTrans;
    //LivingEntity targetEntity;

    ////////////SKILL////////////////////////////////////////////
    private int weaponMaster = 0; 
    private int swordMaster = 1;
    private int summoner = 2;
    private int gunMan = 3;
    private int magician = 4;
    private int hellWalker= 5;
    private int ironMan = 6;
    private int alchemist = 7;
    private int barbarian = 8;
    /////////////////////////////////////////////////////////////
    private int slashRepeat = 4;
    private int slashDamage = 200;
    private bool rDefense1 = false;
    private bool rDefense2 = false;
    private bool rDefense3 = false;
    private bool rDefense4 = false;
    private bool rDefense5 = false;
    private bool rDefense6 = false;
    private bool rDefense7 = false;
    private bool rDefense8 = false;

    private float lastFireTime;    
    private float timeBetFire = 0.2f;
    public int energyDamage=36; 
    public int doomDamage=84;


    void Start()
    {
        for(int i =0; i<9; i++)
            skills[i] = false;
        audioSource = GetComponent<AudioSource>();
//skills[2] = true;
        StartCoroutine(SwordMaster());
    }

    
    void Update()
    {
        
    }
    
    public void SkillCheck()    //아이템 선택했을 때마다 실행
    {
        if(!skills[swordMaster]&&GameManager.instance.itemChecker[0]&&GameManager.instance.itemChecker[1]&&GameManager.instance.itemChecker[4])
        {
            skills[swordMaster] = true; //1
            Time.timeScale = 0;
            skillAlarms[swordMaster].SetActive(true);

            Color color = skillImages[swordMaster].color;
            color.a = 255;
            skillImages[swordMaster].color = color;

            var ach = new Steamworks.Data.Achievement("swordMasterACH");
            ach.Trigger();
        }
            
        if(!skills[summoner]&&GameManager.instance.itemChecker[7]&&GameManager.instance.itemChecker[5])
        {
            skills[summoner] =true; //2
            Time.timeScale = 0;
            skillAlarms[summoner].SetActive(true);

            Color color = skillImages[summoner].color;
            color.a = 255;
            skillImages[summoner].color = color;
            var ach = new Steamworks.Data.Achievement("summonerACH");
            ach.Trigger();
        }

        if(!skills[hellWalker]&&GameManager.instance.itemChecker[7]&&GameManager.instance.itemChecker[10]&&GameManager.instance.itemChecker[1]&&GameManager.instance.itemChecker[9])
        {
            skills[hellWalker] = true; //5
            Time.timeScale = 0;
            skillAlarms[hellWalker].SetActive(true);

            Color color = skillImages[hellWalker].color;
            color.a = 255;
            skillImages[hellWalker].color = color;
            var ach = new Steamworks.Data.Achievement("hellWalkerACH");
            ach.Trigger();

        }

        if(!skills[magician]&&GameManager.instance.itemChecker[2]&&GameManager.instance.itemChecker[11]&&GameManager.instance.itemChecker[3])
        {
            skills[magician] = true;
            Time.timeScale =0;
            skillAlarms[magician].SetActive(true);

            Color color = skillImages[magician].color;
            color.a=255;
            skillImages[magician].color = color;

            var ach = new Steamworks.Data.Achievement("highTemplarACH");
            ach.Trigger();
        }

        if(!skills[gunMan]&&GameManager.instance.itemChecker[15]&&GameManager.instance.itemChecker[17]&&GameManager.instance.itemChecker[18])
        {
            skills[gunMan] = true;
            Time.timeScale =0;
            skillAlarms[gunMan].SetActive(true);

            Color color = skillImages[gunMan].color;
            color.a=255;
            skillImages[gunMan].color = color;

            var ach = new Steamworks.Data.Achievement("sharpShooterACH");
            ach.Trigger();
        }

        if(!skills[ironMan]&&GameManager.instance.itemChecker[16]&&GameManager.instance.itemChecker[20]&&GameManager.instance.itemChecker[21])
        {
            skills[ironMan] = true;
            Time.timeScale =0;
            skillAlarms[ironMan].SetActive(true);

            Color color = skillImages[ironMan].color;
            color.a=255;
            skillImages[ironMan].color = color;
            var ach = new Steamworks.Data.Achievement("ironManACH");
            ach.Trigger();
        
        }

        if(!skills[barbarian]&&GameManager.instance.itemChecker[22]&&GameManager.instance.itemChecker[13]&&GameManager.instance.itemChecker[14])
        {
            skills[barbarian] = true;
            Time.timeScale =0;
            skillAlarms[barbarian].SetActive(true);

            Color color = skillImages[barbarian].color;
            color.a=255;
            skillImages[barbarian].color = color;

            var ach = new Steamworks.Data.Achievement("barbarianACH");
            ach.Trigger();
        }

        CoroutineStarter();
    }
    
    public void AlramOK(int num)
    {
        skillAlarms[num].SetActive(false);
        Time.timeScale=1;
    }

    public void CoroutineStarter()
    {
        if(skills[swordMaster] && !rDefense1)
            StartCoroutine(SwordMaster());

        if(skills[summoner] && !rDefense2)
            StartCoroutine(Summoner());

        if(skills[hellWalker] && !rDefense5)
            StartCoroutine(HellWalker());

        if(skills[magician] && !rDefense4)
            StartCoroutine(Magician());

        if(skills[gunMan] && !rDefense3)
            StartCoroutine(GunMan());

        if(skills[ironMan]&& !rDefense6)
            StartCoroutine(IronMan());

        if(skills[barbarian]&& !rDefense8)
            StartCoroutine(Barbarian());
    }


    private IEnumerator Barbarian()
    {
        if(PlayerPrefs.GetInt("barbarian",0)==0)
            PlayerPrefs.SetInt("barbarian",1);

        rDefense8=true;
        gameObject.GetComponent<PlayerHealth>().maxHealth+=100;
        GameObject doombringer = Instantiate(doombringerPrefab) as GameObject;
        doombringer.transform.SetParent(gameObject.transform.GetChild(0),false);
        gameObject.GetComponent<PlayerHealth>().maxHealthUpdate(100);

        while(true)
        {
            if(gameObject.GetComponentInChildren<Gun>()!=null)
            {
                float before=0;
                if(gameObject.GetComponentInChildren<Gun>().ammoRemain>0)
                    before=gameObject.GetComponentInChildren<Gun>().ammoRemain*0.02f;
                int up = (int)before;
                doombringer.GetComponentInChildren<Doombringer>().damage = doomDamage+up;                
            }
            yield return new WaitForSeconds(1f);            
        }

    }



    private IEnumerator IronMan()
    {
        if(PlayerPrefs.GetInt("ironMan",0)==0)
            PlayerPrefs.SetInt("ironMan",1);

        rDefense6 = true;
        gameObject.GetComponent<PlayerHealth>().maxShield+=50;
        gameObject.GetComponent<PlayerHealth>().maxShieldUpdate(50);
        gameObject.GetComponentInChildren<Gun>().reloadTime+=0.4f;
        gameObject.GetComponent<PlayerMovement>().moveSpeed-=1;

        while(true)
        {
            gameObject.GetComponent<PlayerHealth>().ChrageShield(1);
            yield return new WaitForSeconds(1.5f);
        }
        
    }

    private IEnumerator GunMan()
    {
        if(PlayerPrefs.GetInt("sharpShooter",0)==0)
            PlayerPrefs.SetInt("sharpShooter",1);

        rDefense3=true;

        gameObject.GetComponentInChildren<Gun>().damage+=24;
        gameObject.GetComponentInChildren<Gun>().gunMan=true;
        gameObject.GetComponentInChildren<MusketHolder>().ShaprShooter();
        gameObject.GetComponentInChildren<MachinegunHolder>().SharpShooter();
        gameObject.GetComponentInChildren<SniperHolder>().SharpShooter();
        gameObject.GetComponent<PlayerMovement>().moveSpeed-=1;
        

        yield return new WaitForSeconds(1);
    }


    private IEnumerator Magician()
    {
        if(PlayerPrefs.GetInt("highTemplar",0)==0)
            PlayerPrefs.SetInt("highTemplar",1);

        rDefense4 =true;
        gameObject.GetComponent<Shooter>().magician = true;
        gameObject.GetComponentInChildren<Gun>().damage -=20;
        for(int i =0; i< 40; i++)
        {
            GameObject energey = Instantiate(EnergyPrefab) as GameObject;
            energyQ.Enqueue(energey);
            energey.SetActive(false);
        }

        yield return new WaitForSeconds(1);
    }


    public void MagicianShot()//우클릭 할 때마다 실행
    {
        if(Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            audioSource.PlayOneShot(energyBallSound);

            GameObject energy = energyQ.Dequeue();
            energy.SetActive(true);
            energy.transform.position= transform.position;
            Vector3 angle = transform.GetChild(0).localEulerAngles;
            energy.transform.eulerAngles=angle;
            energy.GetComponent<Rigidbody>().velocity = energy.transform.forward*13;
            energy.GetComponent<EnergyBall>().energyDamage = energyDamage;
            energy.GetComponent<EnergyBall>().off+=()=>Add(energy);
            energy.GetComponent<EnergyBall>().off+=()=>energy.SetActive(false);

            gameObject.GetComponentInChildren<Gun>().magAmmo--;
        }
    }

    private void Add(GameObject energy)
    {
        if(!energyQ.Contains(energy))
            energyQ.Enqueue(energy);
    }


    private IEnumerator HellWalker()
    {
        if(PlayerPrefs.GetInt("hellWalker",0)==0)
            PlayerPrefs.SetInt("hellWalker",1);

        rDefense5 =true;
        GameObject inferno = Instantiate(infernoPrefab) as GameObject;
        inferno.transform.SetParent(gameObject.transform);

        inferno.transform.localPosition = new Vector3(-0.2f,4.7f,1.7f);
        yield return new WaitForSeconds(1);
    }

    private IEnumerator Summoner()
    {
        if(PlayerPrefs.GetInt("summoner",0)==0)
            PlayerPrefs.SetInt("summoner",1);

        rDefense2 =true;
        GameObject dragon = Instantiate(dragonPrefab);
        

        yield return new WaitForSeconds(1);
    }


    private IEnumerator SwordMaster()
    {
        if(PlayerPrefs.GetInt("swordMaster",0)==0)
            PlayerPrefs.SetInt("swordMaster",1);

        //while(true)
        //{
        if(skills[swordMaster])
        {
            rDefense1 =true;
            for(int j = 0; j< 8; j++)
            {
                GameObject slash = Instantiate(slashPrefab) as GameObject;
                slashQ.Enqueue(slash);
                slash.SetActive(false);                
            }


            while(true)
            {
                for(int i=0;i<slashRepeat;i++)
                {
                    Collider[] colliders =
                    Physics.OverlapSphere(transform.position, 10f,whatIsTarget);

                    for(int j = 0; j<colliders.Length; j++)
                    {
                        LivingEntity livingEntity =
                        colliders[j].GetComponent<LivingEntity>();

                        if(livingEntity!=null&& !livingEntity.dead)
                        {
                            targetTrans =
                            colliders[j].GetComponent<Transform>();

                            livingEntity.OnDamage(slashDamage);
                            break;
                        }
                    }

                    if(targetTrans!=null)
                    {
                        GameObject slash = slashQ.Dequeue();
                        slash.SetActive(true);

                        slash.transform.position = targetTrans.position;
                        slash.GetComponent<SwordMaster>().off +=()=>slashQ.Enqueue(slash);
                        slash.GetComponent<SwordMaster>().off +=()=>slash.SetActive(false);
                    }
                }//4번 반복


                yield return new WaitForSeconds(1);
            }       
        }
        //yield return new WaitForSeconds(1);
        //}

    }
    





}
