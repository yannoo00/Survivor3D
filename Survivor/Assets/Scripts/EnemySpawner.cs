using System.Collections.Generic;
using UnityEngine;
using System.Collections;

// 적 게임 오브젝트를 주기적으로 생성
public class EnemySpawner : MonoBehaviour {

    public CrabBoss crabPrefab;
    public Ghoul ghoulPrefab; // 생성할 적 AI
    public Ghoul metalGhoulPrefab;
    public Walker walkerPrefab;
    public GameObject balloonPrefab;
    public ShootingEnemy snowmanPrefab;
    public Walker balloonYellowPrefab;
    public Walker insectPrefab;
    public Walker zombiePrefab;
    public Walker redSkulPrefab;
    public Walker redSkulMiniPrefab;
    public Walker SkulPrefab;
    public Walker HellSkulPrefab;
    public CrossShootingEnemy bulDogPrefab;
    public CrossShootingEnemy rhinoPrefab;
    public StrongerGolem strongerGolem;
    public StrongerGolem strongerGolemNoneBoss;
    public Chirone chironePrefab;

    public ShootingEnemy blueSkulPrefab;
    public ShootingEnemy blueSkulMiniPrefab;

    public Spider spiderPrefab;

    public ShootingEnemy golemBossPrefab;
    public OrcBoss orcBossPrefab;
    public OrcBoss redOrcPrefab;
    public OrcBoss rhinoMutantPrefab;
    public shotGunEnemy shootingZombiePrefab;
    public WalkingBoss GaintSkulPrefab;
    public SpiderMutant spiderMutant;

////////////////////////////////////////////////////////////////////
    public Transform[] spawnPoints; // 적 AI를 소환할 위치들

    public GameObject itemAllocater;

    public ItemSpawner itemSpawner;
    public PlayerSkill playerSkill;


    public float damageMax = 40f; // 최대 공격력
    public float damageMin = 10f; // 최소 공격력

    public float healthMax = 250f; // 최대 체력
    public float healthMin = 150f; // 최소 체력

    public float speedMax = 2f; // 최대 속도
    public float speedMin = 1f; // 최소 속도
    private bool delay=false;

    public Color strongEnemyColor = Color.red; // 강한 적 AI가 가지게 될 피부색

    //private List<GameObject> Enemies = new List<GameObject>();

    private List<Ghoul> GhoulEnemies = new List<Ghoul>(); // 생성된 적들을 담는 리스트
    public int LastEnemies = 0;
    public int wave{get; private set;} // 현재 웨이브


////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////오브젝트 풀링//////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////



    //public Queue<GameObject> balloonQ = new Queue<GameObject>();
    //public Queue<Ghoul> GhoulQ = new Queue<Ghoul>();


    void Start()
    {
        // for(int i = 0; i < 30; i++) //balloon을 30마리 생성.
        // {
        //     GameObject balloon = Instantiate(balloonPrefab);    
        //     balloonQ.Enqueue(balloon);
        //     balloon.SetActive(false);        
        // }
        //StartCoroutine(Timer());
    }

    // IEnumerator Timer()
    // {
    //     yield return new WaitForSeconds(30);
    //     Debug.Log(Time.time);
    // }
 

    private void Update() 
    {
        // 게임 오버 상태일때는 생성하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        // 적을 모두 물리친 경우 다음 스폰 실행
        if (LastEnemies <= 0&&!delay)
        {
            StartCoroutine(Delay());
            
            //playerSkill.CoroutineStarter();
        }

        // UI 갱신
        UpdateUI();
        UIManager.instance.UpdateTimer(180);
    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI() {
        // 현재 웨이브와 남은 적의 수 표시
        UIManager.instance.UpdateWaveText(wave,LastEnemies);
    }
    




    // 현재 웨이브에 맞춰 적을 생성
    private void SpawnWave() {
        
        if(10<=itemAllocater.GetComponent<ItemAllocater>().ItemNumChecker())
            itemAllocater.GetComponent<ItemAllocater>().fullItem=true;

        //웨이브++ 하기 전에 아이템 선택 창이 먼저 열린다.
        if(wave==20)
        {
            Time.timeScale=0;
        }
    
        else if(wave<20)
            itemAllocater.GetComponent<ItemAllocater>().ItemSet();


        itemAllocater.GetComponent<ItemAllocater>().RenewButton();
        wave++;

        if(wave == 10||wave==7||wave==20)
            UIManager.instance.lastTime = 150;
        else
            UIManager.instance.lastTime = 120;

        SkillState();
        

        switch(wave)
        {
            case 1:
                for(int i =0; i< 5; i++)
                    CreateWalker();
                
                StartCoroutine(ItemSpawn3());
                CreateShootingZombie();
                CreateBalloon();

                break;


            case 2:
                for(int i =0; i< 6; i++)
                    CreateWalker();
                CreateBalloon();
                CreateShootingZombie();
                CreateBalloonYellow();
                CreateBalloonYellow();
                CreateBalloonYellow();
                CreateSkul();
                CreateSkul();
                CreateSkul();
                StartCoroutine(ItemSpawn3());

                CreateInsect();
                break;


            case 3:
                for(int i =0; i< 3; i++)
                {
                    CreateWalker();
                    CreateBalloonYellow();
                    CreateBalloon();
                }
                for(int i =0; i<2;i++)
                    CreateSkul();

                StartCoroutine(ItemSpawn5());

                CreateShootingZombie();
                CreateShootingZombie();
                CreateSpider();
                CreateRhino();
                break;


            case 4:
                StartCoroutine(EnemySpawn4());
                StartCoroutine(ItemSpawn5());
                CreateSpiderMutant();
                break;


            case 5:
                StartCoroutine(EnemySpawn5());
                StartCoroutine(WeekGen50());
                StartCoroutine(Ghoul30());
                StartCoroutine(ItemSpawn5());
                break;


            case 6:
                StartCoroutine(EnemySpawn6());
                StartCoroutine(WeekGen50());
                StartCoroutine(Ghoul30());
                StartCoroutine(ItemSpawn5());
                break;


            case 7:
                StartCoroutine(EnemySpawn7());
                StartCoroutine(WeekGen50());
                StartCoroutine(Ghoul30());
                StartCoroutine(ItemSpawn5());
                break;


            case 8:
                StartCoroutine(EnemySpawn8());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(ItemSpawn5());
                break;


            case 9:
                StartCoroutine(EnemySpawn9());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(ItemSpawn5());
                break;


            case 10:
                StartCoroutine(EnemySpawn10());
                StartCoroutine(WeekGen50());
                StartCoroutine(Ghoul30());
                StartCoroutine(MetalGhoul30());
                StartCoroutine(ItemSpawn10());
                break;


            case 11:
                StartCoroutine(EnemySpawn11());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(MetalGhoul30());
                StartCoroutine(ItemSpawn10());
                break;


            case 12:
                StartCoroutine(EnemySpawn12());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(MetalGhoul30());
                StartCoroutine(ItemSpawn10());
                break;


            case 13:
                StartCoroutine(EnemySpawn13());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(MetalGhoul30());
                StartCoroutine(ItemSpawn10());
                break;


            case 14:
                StartCoroutine(EnemySpawn14());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(Ghoul30());
                StartCoroutine(MetalGhoul30());
                StartCoroutine(ItemSpawn10());
                break;


            case 15:
                CreateChirone();
                StartCoroutine(EnemySpawn15());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(Ghoul30());
                StartCoroutine(MetalGhoul30());
                StartCoroutine(ItemSpawn10());
                break;


            case 16:
                StartCoroutine(EnemySpawn16());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(Ghoul30());
                StartCoroutine(MetalGhoul30());
                StartCoroutine(ItemSpawn10());
                break;


            case 17:
                StartCoroutine(EnemySpawn17());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(Ghoul30());
                StartCoroutine(MetalGhoul30());
                StartCoroutine(ItemSpawn10());
                break;


            case 18:
                StartCoroutine(EnemySpawn18());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(Ghoul30());
                StartCoroutine(MetalGhoul30());
                StartCoroutine(ItemSpawn10());
                break;


            case 19:
                StartCoroutine(EnemySpawn19());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(Ghoul30());
                StartCoroutine(MetalGhoul30());
                StartCoroutine(ItemSpawn10());
                break;


            case 20:
                StartCoroutine(EnemySpawn20());
                StartCoroutine(WeekGen100());
                StartCoroutine(Ghoul30());
                StartCoroutine(Ghoul30());
                StartCoroutine(MetalGhoul30());
                StartCoroutine(ItemSpawn10());
                
                break;
            case 21:
                CreateSnowman();
                
                break;
    
        }

        int spawnCount = Mathf.RoundToInt(wave * 1f);

        for(int i =0; i < wave ; i++)
        {
            CreateInsect();
        }




        if(wave > 10&&wave%2==0)
        {
            CreateOrcBoss();
            CreateRedSkul();
            CreateBlueSkul();
        }
        else if(wave>10&&wave%2==1)
        {
            CreateGolemBoss();
            CreateRhino();
            CreateRhinoMutant();
        }
    }

    private void SkillState()
    {
        if(playerSkill.skills[4])
            playerSkill.energyDamage+=2;

        if(playerSkill.skills[3])
            playerSkill.gameObject.GetComponentInChildren<Gun>().damage+=3;

        if(playerSkill.skills[6])
        {
            playerSkill.gameObject.GetComponent<PlayerHealth>().maxShield+=5;  
            playerSkill.gameObject.GetComponent<PlayerHealth>().maxShieldUpdate(5);
        }


        if(playerSkill.skills[8])
            playerSkill.doomDamage+=8;
    }


    private void ActiveBalloon()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];
        LastEnemies++;

        //GameObject balloon = balloonQ.Dequeue();
        //balloon.SetActive(true);
        //balloon.transform.position = spawnPoint.position + Vector3.up*3;

        //balloon.GetComponent<ShootingEnemy>().onDeath +=() => LastEnemies--;
        //balloon.GetComponent<ShootingEnemy>().onDeath +=() => balloonQ.Enqueue(balloon);
        //balloon.GetComponent<ShootingEnemy>().onDeath +=() => balloon.SetActive(false);
    }

    private void CreateCrabBoss()
    {
        Vector3 bossPosition = new Vector3(3.5f,0,-16);
        CrabBoss crab = Instantiate(crabPrefab,bossPosition,Quaternion.identity);
        LastEnemies++;

        crab.onDeath += () => Destroy(crab.gameObject,3f);
        crab.onDeath += () => LastEnemies--;
    }

    private void CreateBulDog()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];
        CrossShootingEnemy bulDog = Instantiate(bulDogPrefab, spawnPoint.position + Vector3.up*3,spawnPoint.rotation);
        LastEnemies++;

        //bulDog.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        bulDog.GetComponent<CrossShootingEnemy>().Setup(2500,1.5f);        

        bulDog.GetComponent<CrossShootingEnemy>().onDeath += () => LastEnemies--;
        ParticleSystem bomb = Instantiate(bulDog.GetComponent<CrossShootingEnemy>().die);
        bulDog.GetComponent<CrossShootingEnemy>().onDeath += () =>bomb.transform.position=bulDog.transform.position;
        bulDog.GetComponent<CrossShootingEnemy>().onDeath += () => Destroy(bulDog.gameObject, 0.125f);
        bulDog.GetComponent<CrossShootingEnemy>().onDeath += () =>bomb.Play();
        bulDog.GetComponent<CrossShootingEnemy>().onDeath += () =>CreateBalloonHere(bulDog.transform);
        //bulDog.GetComponent<CrossShootingEnemy>().onDeath += () => bulDog.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
    }
    private void CreateBalloonHere(Transform here)
    {
        for(int i =0; i<6; i++)
        {
            GameObject balloon = Instantiate(balloonPrefab, here.position + Vector3.up*3,here.rotation);
            LastEnemies++;
            balloon.GetComponent<ShootingEnemy>().Setup(100,4);        
            balloon.GetComponent<ShootingEnemy>().onDeath += () => Destroy(balloon.gameObject, 3f);
            balloon.GetComponent<ShootingEnemy>().onDeath += () => LastEnemies--;
            int degree = Random.Range(1,30);
            if(degree > 28)
                balloon.GetComponent<ShootingEnemy>().onDeath += () =>itemSpawner.GemDrop(balloon.transform.position,0);            
        }
    }

    private void CreateRhino()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        CrossShootingEnemy bulDog = Instantiate(rhinoPrefab, spawnPoint.position + Vector3.up*3,spawnPoint.rotation);
        LastEnemies++;

        bulDog.GetComponent<CrossShootingEnemy>().Setup(1250,3.5f);        

        bulDog.GetComponent<CrossShootingEnemy>().onDeath += () => Destroy(bulDog.gameObject, 3f);
        bulDog.GetComponent<CrossShootingEnemy>().onDeath += () => LastEnemies--;
    }


    private void CreateBalloon()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        GameObject balloon = Instantiate(balloonPrefab, spawnPoint.position + Vector3.up*3,spawnPoint.rotation);
        LastEnemies++;

        balloon.GetComponent<ShootingEnemy>().Setup(100,4);        

        balloon.GetComponent<ShootingEnemy>().onDeath += () => Destroy(balloon.gameObject, 3f);
        balloon.GetComponent<ShootingEnemy>().onDeath += () => LastEnemies--;

        int degree = Random.Range(1,25);
        if(degree > 23)
            balloon.GetComponent<ShootingEnemy>().onDeath += () =>itemSpawner.GemDrop(balloon.transform.position,0);
    }

    private void CreateSkul()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];
        Walker skul = Instantiate(SkulPrefab,spawnPoint.transform.position,spawnPoint.transform.rotation);
        LastEnemies++;

        skul.GetComponent<Walker>().Setup(185,8,7);
        skul.GetComponent<Walker>().onDeath+=()=>Destroy(skul.gameObject,3f);
        skul.GetComponent<Walker>().onDeath+=()=>LastEnemies--;
        skul.GetComponent<Walker>().onDeath+=()=>skul.GetComponentInChildren<ParticleSystem>().Stop();

        int degree = Random.Range(1,25);
        if(degree > 23)
            skul.GetComponent<Walker>().onDeath += () =>itemSpawner.GemDrop(skul.transform.position,0);
    }

    private void CreateHellSkul()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];
        Walker skul = Instantiate(HellSkulPrefab,spawnPoint.transform.position,spawnPoint.transform.rotation);
        LastEnemies++;

        skul.GetComponent<Walker>().Setup(425,12,9);
        skul.GetComponent<Walker>().onDeath+=()=>Destroy(skul.gameObject,3f);
        skul.GetComponent<Walker>().onDeath+=()=>LastEnemies--;
        skul.GetComponent<Walker>().onDeath+=()=>skul.GetComponentInChildren<ParticleSystem>().Stop();

        int degree = Random.Range(1,20);
        if(degree > 18)
            skul.GetComponent<Walker>().onDeath += () =>itemSpawner.GemDrop(skul.transform.position,0);
    }

    private void CreateSnowman()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        ShootingEnemy snowman = Instantiate(snowmanPrefab, spawnPoint.position + Vector3.up*3,spawnPoint.rotation);
        LastEnemies++;

        snowman.GetComponent<ShootingEnemy>().Setup(200,3);        

        snowman.GetComponent<ShootingEnemy>().onDeath += () => Destroy(snowman.gameObject, 3f);
        snowman.GetComponent<ShootingEnemy>().onDeath += () => LastEnemies--;
    }
    private void CreateShootingZombie()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        shotGunEnemy shootingZombie = Instantiate(shootingZombiePrefab, spawnPoint.position + Vector3.up*3,spawnPoint.rotation);
        LastEnemies++;

        shootingZombie.GetComponent<shotGunEnemy>().Setup(200,3.5f);        

        shootingZombie.GetComponent<shotGunEnemy>().onDeath += () => Destroy(shootingZombie.gameObject, 3f);
        shootingZombie.GetComponent<shotGunEnemy>().onDeath += () => LastEnemies--;
        int degree = Random.Range(1,25);
        if(degree > 23)
            shootingZombie.GetComponent<shotGunEnemy>().onDeath += () =>itemSpawner.GemDrop(shootingZombie.transform.position,0);
    }

    private void CreateWalker()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        Walker walker = Instantiate(walkerPrefab, spawnPoint.position,spawnPoint.rotation);
        LastEnemies++;

        //walker.Setup(100,10,1.5f);
        walker.onDeath += () => Destroy(walker.gameObject, 4f);
        walker.onDeath += () => LastEnemies--; 
        int degree = Random.Range(1,25);
        if(degree > 23)
            walker.onDeath += () =>itemSpawner.GemDrop(walker.transform.position,0);
    }
    private void CreateZombie()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        Walker zombie = Instantiate(zombiePrefab, spawnPoint.position,spawnPoint.rotation);
        LastEnemies++;

        //walker.Setup(100,10,1.5f);
        zombie.onDeath += () => Destroy(zombie.gameObject, 4f);
        zombie.onDeath += () => LastEnemies--; 
        int degree = Random.Range(1,40);
        if(degree > 38)
            zombie.onDeath += () =>itemSpawner.GemDrop(zombie.transform.position,0);
    }
    private void CreateRedSkul()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        Walker skul = Instantiate(redSkulPrefab, spawnPoint.position,spawnPoint.rotation);
        LastEnemies++;

        //walker.Setup(100,10,1.5f);
        //skul.onDeath += () => Destroy(skul.gameObject, 1f);
        skul.onDeath +=() => SkulReborn(skul,redSkulMiniPrefab);
        skul.onDeath += () => LastEnemies--; 
        int degree = Random.Range(1,40);
        if(degree > 38)
            skul.onDeath += () =>itemSpawner.GemDrop(skul.transform.position,1);
    }
    private void CreateBlueSkul()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        ShootingEnemy skul = Instantiate(blueSkulPrefab, spawnPoint.position,spawnPoint.rotation);
        LastEnemies++;

        skul.Setup(1750,5.2f);
        //skul.onDeath += () => Destroy(skul.gameObject, 1f);
        skul.onDeath +=() => SkulRebornBlue(skul,blueSkulMiniPrefab);
        skul.onDeath += () => LastEnemies--; 
        int degree = Random.Range(1,40);
        if(degree > 38)
            skul.onDeath += () =>itemSpawner.GemDrop(skul.transform.position,1);
    }

    private void CreateGaintSkul()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        WalkingBoss skul = Instantiate(GaintSkulPrefab, spawnPoint.position,spawnPoint.rotation);
        LastEnemies++;

        skul.onDeath += () => LastEnemies--; 
        skul.onDeath += () => itemSpawner.GemDrop(skul.transform.position,2);
        skul.onDeath += () => Destroy(skul.gameObject,3f);
    }

    private void CreateChirone()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        Chirone chirone =  Instantiate(chironePrefab,spawnPoint.position,spawnPoint.rotation);
        LastEnemies++;

        chirone.onDeath+=()=>LastEnemies--;
        chirone.onDeath+=()=>itemSpawner.GemDrop(chirone.transform.position,2);
        chirone.onDeath+=()=>Destroy(chirone.gameObject,4f);
    }

    private void CreateBalloonYellow()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        Walker balloonYellow = Instantiate(balloonYellowPrefab, spawnPoint.position,spawnPoint.rotation);
        LastEnemies++;

        //balloonYellow.Setup(150,20,1.6f);

        balloonYellow.onDeath += () => Destroy(balloonYellow.gameObject, 3f);
        balloonYellow.onDeath += () => LastEnemies--; 
        int degree = Random.Range(1,25);
        if(degree > 23)
            balloonYellow.onDeath += () =>itemSpawner.GemDrop(balloonYellow.transform.position,0);
    }

    private void CreateGolemBoss()
    {
        Transform spawnPoint  = spawnPoints[Random.Range(0,spawnPoints.Length)];

        ShootingEnemy golemBoss = Instantiate(golemBossPrefab, spawnPoint.position, spawnPoint.rotation);
        LastEnemies++;
        golemBoss.Setup(1500,4);
        golemBoss.onDeath += () => LastEnemies--;
        int degree = Random.Range(1,10);
        if(degree > 8)
            golemBoss.onDeath += () => itemSpawner.GemDrop(golemBoss.transform.position, 1);
        golemBoss.onDeath += () => Destroy(golemBoss.gameObject,3f);
    }

    private void CreateStrongerGolem()
    {
        Transform spawnPoint  = spawnPoints[Random.Range(0,spawnPoints.Length)];

        StrongerGolem golemBoss = Instantiate(strongerGolem, spawnPoint.position, spawnPoint.rotation);
        LastEnemies++;
        golemBoss.onDeath +=()=>LastEnemies--;  
        golemBoss.onDeath += () => itemSpawner.GemDrop(golemBoss.transform.position, 2);
        golemBoss.onDeath +=()=>Destroy(golemBoss.gameObject,3f);
    }

    private void CreateStrongerGolemNoneBoss()
    {
        Transform spawnPoint  = spawnPoints[Random.Range(0,spawnPoints.Length)];

        StrongerGolem golemBoss = Instantiate(strongerGolemNoneBoss, spawnPoint.position, spawnPoint.rotation);
        LastEnemies++;
        golemBoss.onDeath +=()=>LastEnemies--;  
        golemBoss.onDeath += () => itemSpawner.GemDrop(golemBoss.transform.position, 1);
        golemBoss.onDeath +=()=>Destroy(golemBoss.gameObject,3f);
    }

    private void CreateOrcBoss()
    {
        Transform spawnPoint  = spawnPoints[Random.Range(0,spawnPoints.Length)];

        OrcBoss orcBoss = Instantiate(orcBossPrefab, spawnPoint.position, spawnPoint.rotation);
        LastEnemies++;

        orcBoss.onDeath += () => LastEnemies--;
        int degree = Random.Range(1,8);
        if(degree > 6)
            orcBoss.onDeath += () => itemSpawner.GemDrop(orcBoss.transform.position, 1);
        orcBoss.onDeath += () => Destroy(orcBoss.gameObject,3f);
    }

    private void CreateRhinoMutant()
    {
        Transform spawnPoint  = spawnPoints[Random.Range(0,spawnPoints.Length)];

        OrcBoss orcBoss = Instantiate(rhinoMutantPrefab, spawnPoint.position, spawnPoint.rotation);
        LastEnemies++;

        orcBoss.onDeath += () => LastEnemies--;
        int degree = Random.Range(1,12);
        if(degree > 10)
            orcBoss.onDeath += () => itemSpawner.GemDrop(orcBoss.transform.position, 1);
        orcBoss.onDeath += () => Destroy(orcBoss.gameObject,3f);
    }

    private void CreateRedOrc()
    {
        Transform spawnPoint  = spawnPoints[Random.Range(0,spawnPoints.Length)];

        OrcBoss orcBoss = Instantiate(redOrcPrefab, spawnPoint.position, spawnPoint.rotation);
        LastEnemies++;

        orcBoss.onDeath += () => LastEnemies--;
        int degree = Random.Range(1,8);
        if(degree > 5)
            orcBoss.onDeath += () => itemSpawner.GemDrop(orcBoss.transform.position, 1);
        orcBoss.onDeath += () => Destroy(orcBoss.gameObject,3f);
    }

    private void CreateInsect()
    {
        Transform spawnPoint  = spawnPoints[Random.Range(0,spawnPoints.Length)];

        Walker insect = Instantiate(insectPrefab, spawnPoint.position, spawnPoint.rotation);
        LastEnemies++;

        insect.Setup(400, 30, 1.8f);

        insect.onDeath += () => Destroy(insect.gameObject, 3f);
        insect.onDeath += () => LastEnemies--;
        int degree = Random.Range(1,8);
        if(degree > 6)
            insect.onDeath += () => itemSpawner.GemDrop(insect.transform.position, 0);
    }

    public void CreateSpider()
    {
        Transform spawnPoint  = spawnPoints[Random.Range(0,spawnPoints.Length)];

        //spider.SetActive(true);
        Spider spider = Instantiate(spiderPrefab,spawnPoint.position,Quaternion.identity);
        
        spider.transform.position = spawnPoint.position;
        LastEnemies++;

        //spider.SpiderSetup(2000,2f);

        spider.onDeath += () => Destroy(spider);
        spider.onDeath += () => LastEnemies--;
        int degree = Random.Range(1,10);
        if(degree > 8)
            spider.onDeath += () => itemSpawner.GemDrop(spider.transform.position, 0);
    }

    void CreateSpiderMutant()
    {
        Transform spawnPoint  = spawnPoints[Random.Range(0,spawnPoints.Length)];

        //spider.SetActive(true);
        SpiderMutant spider = Instantiate(spiderMutant,spawnPoint.position,Quaternion.identity);
        
        spider.transform.position = spawnPoint.position;
        LastEnemies++;
        spider.Setup(750,5);
        spider.onDeath += () => Destroy(spider);
        spider.onDeath += () => LastEnemies--;
        int degree = Random.Range(1,10);
        if(degree > 8)
            spider.onDeath += () => itemSpawner.GemDrop(spider.transform.position, 0);
    }


    // 적을 생성하고 생성한 적에게 추적할 대상을 할당
    private void CreateEnemy(float intensity) {

        //생성한 적에게 할당할 수치 결정
        //Lerp는 선형 보간
        float health = Mathf.Lerp(healthMin, healthMax, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);

        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Ghoul ghoul = Instantiate(ghoulPrefab, spawnPoint.position, spawnPoint.rotation);

        ghoul.Setup(health, damage, speed, skinColor);

        GhoulEnemies.Add(ghoul);
        LastEnemies++;


        //람다식을 이용해 익명 함수 만들기
        ghoul.onDeath += () => GhoulEnemies.Remove(ghoul);

        ghoul.onDeath += () => Destroy(ghoul.gameObject, 3f);
        ghoul.onDeath += () => LastEnemies--;

        //ghoul.onDeath += () => GameManager.instance.AddScore(100);

        if(intensity >= 0.8f)
             ghoul.onDeath += () => itemSpawner.GemDrop(ghoul.transform.position,0);
    }
    private void CreateMetalGhoul(float intensity) {

        //생성한 적에게 할당할 수치 결정
        //Lerp는 선형 보간
        float health = Mathf.Lerp(200, 500, intensity);
        float damage = Mathf.Lerp(10, 20, intensity);
        float speed = Mathf.Lerp(1.5f, 7f, intensity);

        Color skinColor = Color.Lerp(Color.green, Color.white, intensity);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Ghoul metalGhoul = Instantiate(metalGhoulPrefab, spawnPoint.position, spawnPoint.rotation);

        metalGhoul.Setup(health, damage, speed, skinColor);
        LastEnemies++;


        //람다식을 이용해 익명 함수 만들기
        metalGhoul.onDeath += () => Destroy(metalGhoul.gameObject, 3f);
        metalGhoul.onDeath += () => LastEnemies--;

        //ghoul.onDeath += () => GameManager.instance.AddScore(100);

        if(intensity >= 1.9f)
            metalGhoul.onDeath += () => itemSpawner.GemDrop(metalGhoul.transform.position,0);
    }
///////////////////////////////////////////////////////////////////////////////////////////////////////

    public void SkulReborn(Walker skul,Walker miniSkul)
    {
        Destroy(skul.gameObject,1f);
        Walker mini = Instantiate(miniSkul,skul.gameObject.transform.position,Quaternion.identity);
        LastEnemies++;
        mini.GetComponent<Walker>().onDeath+=()=>LastEnemies--;
        mini.GetComponent<Walker>().onDeath+=()=>Destroy(mini.gameObject,3f);
    }
    public void SkulRebornBlue(ShootingEnemy skul,ShootingEnemy miniSkul)
    {
        Destroy(skul.gameObject,1f);
        ShootingEnemy mini = Instantiate(miniSkul,skul.gameObject.transform.position,Quaternion.identity);
        LastEnemies++;
        mini.GetComponent<ShootingEnemy>().Setup(1000,6);
        mini.GetComponent<ShootingEnemy>().onDeath+=()=>LastEnemies--;
        mini.GetComponent<ShootingEnemy>().onDeath+=()=>Destroy(mini.gameObject,3f);
    }

    private IEnumerator ItemSpawn3()
    {
        itemSpawner.Spawn();
        yield return new WaitForSeconds(2f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(2f);
        itemSpawner.Spawn();
        //yield return new WaitForSeconds(5f);
    }
    private IEnumerator ItemSpawn5()
    {
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        //yield return new WaitForSeconds(5f);
    }
    private IEnumerator ItemSpawn10()
    {
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(5f);
        itemSpawner.Spawn();
    }



    private IEnumerator Delay()
    {
        delay = true;
        yield return new WaitForSeconds(2f);
        SpawnWave();
        //playerSkill.CoroutineStarter();
        delay = false;
    }

    private IEnumerator EnemySpawn4()
    {
        for(int i =0; i<3; i++)
        {
            CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            CreateShootingZombie();
        }
        itemSpawner.Spawn();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(2f);
        itemSpawner.Spawn();

        yield return new WaitForSeconds(2f);
        itemSpawner.Spawn();

        for(int i =0; i<5; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        itemSpawner.Spawn();

        for(int i =0; i<5; i++)
        {
            CreateBalloonYellow();
            CreateWalker();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<5; i++)
        {
            CreateBalloonYellow();
            CreateWalker();
        }
        yield return new WaitForSeconds(3f);

        CreateHellSkul();
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<2; i++)
        {
            CreateBalloon();
            CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<3;i++)
            CreateSkul();

        CreateRedSkul();
    }

    private IEnumerator EnemySpawn5()
    {
        for(int i =0; i<4; i++)
        {
            CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            CreateShootingZombie();
        }
        CreateBlueSkul();
        itemSpawner.Spawn();
        CreateSpider();
        CreateSpider();
        for(int i =0; i<4; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
     
        yield return new WaitForSeconds(3f);

        for(int i =0; i<4; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        //CreateGolemBoss();
        for(int i =0; i<3;i++)
            CreateSkul();
        CreateHellSkul();
    }

    private IEnumerator EnemySpawn6()
    {
        yield return new WaitForSeconds(1f);
        itemSpawner.Spawn();
        for(int i =0; i<5; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(2f);
        itemSpawner.Spawn();
        for(int i =0;i<2;i++)
        {
            CreateInsect();
            CreateShootingZombie();
        }   
        itemSpawner.Spawn();
        CreateOrcBoss();
        for(int i =0; i<10; i++)
        {
            CreateBalloonYellow();
            CreateWalker();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        itemSpawner.Spawn();
        for(int i =0; i<4; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        for(int i =0;i<2;i++)   
        {
            CreateInsect();
            CreateShootingZombie();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(4f);
        CreateSpider();
        CreateGolemBoss();
        CreateSpider();
        for(int i =0; i<3;i++)
            CreateSkul();

        CreateBulDog();
    }

    private IEnumerator EnemySpawn7()
    {
        for(int i =0; i<5; i++)
        {
            CreateBalloonYellow();
            CreateWalker();
        }
        CreateRhinoMutant();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);

        for(int i =0; i<5; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(2f);
        itemSpawner.Spawn();
        for(int i =0;i<2;i++)
        {
            CreateInsect();
            CreateShootingZombie();
        }
        CreateSpider();
        CreateSpider();
        CreateSpider();
        for(int i =0; i<10; i++)
        {
            CreateBalloonYellow();
            CreateWalker();
        }
        yield return new WaitForSeconds(2f);
        itemSpawner.Spawn();
        for(int i =0; i<4; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }

        for(int i =0;i<2;i++)
        {
            CreateInsect();
            CreateShootingZombie();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);
        for(int i =0; i<3;i++)
        {
            CreateSkul();
            CreateHellSkul();
        }    
        yield return new WaitForSeconds(2f);
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        CreateStrongerGolem();
    }

    private IEnumerator EnemySpawn8()
    {

        CreateGolemBoss();
        CreateRedSkul();
        CreateRhinoMutant();
        CreateRhino();
        ItemSpawn3();
        yield return new WaitForSeconds(5f);
        CreateSpider();
        CreateSpider();
        CreateBulDog();
        CreateSpider();
        yield return new WaitForSeconds(4f);
        CreateSpiderMutant();
        CreateSpiderMutant();
        ItemSpawn10();
        CreateBlueSkul();
        CreateOrcBoss();
        yield return new WaitForSeconds(4f);
        CreateBlueSkul();
        //CreateBlueSkul();
        yield return new WaitForSeconds(4f);
        CreateRhino();
        yield return new WaitForSeconds(4f);
        CreateRedSkul();
        ItemSpawn3();
        for(int i =0; i<12;i++)
            CreateSkul();
        yield return new WaitForSeconds(4f);
        CreateSpiderMutant();
        CreateSpiderMutant();
    }

    private IEnumerator EnemySpawn9()
    {

        CreateGolemBoss();
        CreateOrcBoss();

        yield return new WaitForSeconds(3f);
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        yield return new WaitForSeconds(3f);
        CreateSpider();
        CreateSpider();
        yield return new WaitForSeconds(3f);
        CreateSpiderMutant();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        yield return new WaitForSeconds(3f);
        CreateSpiderMutant();
        CreateSpider();
        CreateOrcBoss();
        CreateOrcBoss();
        yield return new WaitForSeconds(3f);
        for(int i =0; i<14;i++)
            CreateSkul();
        for(int i =0; i<10; i++)
        {
            CreateBalloonYellow();
            CreateWalker();
        }
        CreateInsect();
        yield return new WaitForSeconds(3f);

        for(int i =0; i<5; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        CreateInsect();
        yield return new WaitForSeconds(3f);

        for(int i =0;i<2;i++)
        {
            CreateInsect();
            CreateShootingZombie();
            CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);
        for(int i =0; i<12;i++)
        {
            CreateHellSkul();
        } 

        for(int i =0; i<10; i++)
        {
            CreateBalloonYellow();
            CreateWalker();
        }
        yield return new WaitForSeconds(2f);

        for(int i =0; i<4; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }

        for(int i =0;i<3;i++)
        {
           CreateGolemBoss();
        }
        CreateRhino();
        CreateBulDog();
        CreateRhino();
        
        yield return new WaitForSeconds(3f);

        CreateRedOrc();
    }

    private IEnumerator EnemySpawn10()
    {

 
        for(int i =0;i<10;i++)
        {
            CreateInsect();
            CreateZombie();
        }
        yield return new WaitForSeconds(2f);

        for(int i =0;i<10;i++)
        {
            CreateInsect();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(2f);

        for(int i =0;i<10;i++)
        {
            CreateInsect();
            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(2f);

        for(int i =0;i<10;i++)
        {
            CreateInsect();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(2f);

        for(int i =0;i<5;i++)
        {
            CreateInsect();
            CreateZombie();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(2f);

        for(int i =0;i<10;i++)
        {
            CreateInsect();
            CreateSpider();
            CreateSpider();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(2f);
        for(int i=0; i<25;i++)
        {
            CreateSkul();
            CreateHellSkul();
        }

        CreateGaintSkul();

    }

    private IEnumerator EnemySpawn11() //메탈구울 위주
    {

 
        for(int i =0;i<25;i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);
        }
        itemSpawner.Spawn();
        CreateBalloon();
        CreateBalloon();
        CreateShootingZombie();
        CreateShootingZombie();
        CreateSpiderMutant();
        CreateSpiderMutant();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);
        
        for(int i =0;i<25;i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);
        }
        CreateBalloon();
        CreateBalloon();
        CreateShootingZombie();
        CreateShootingZombie();
        for(int i =0; i<12;i++)
        {
            CreateHellSkul();
        } 
        CreateSpider();
        CreateSpider();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);

        for(int i =0;i<25;i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);
        }
        CreateBalloon();
        CreateBalloon();
        CreateShootingZombie();
        CreateShootingZombie();
        for(int i =0;i<25;i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);
        }
        CreateBlueSkul();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);
        for(int i =0;i<25;i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);
        }
        CreateBlueSkul();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(10f);
        for(int i =0;i<25;i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(10f);
        for(int i =0;i<25;i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);
        }
        CreateSpider();
        CreateSpider();
        CreateSpider();        
        CreateSpider();
        CreateSpider();
        CreateBlueSkul();
    }

    private IEnumerator EnemySpawn12() //슈팅 위주
    {

 
        for(int i =0;i<30;i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);
        }
        CreateSpiderMutant();
        CreateBulDog();
        CreateBulDog();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);
        
        for(int i =0;i<20;i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);
        }
        CreateBalloon();
        CreateBalloon();
        CreateRedSkul();
        CreateShootingZombie();
        CreateShootingZombie();
        CreateSpiderMutant();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);
        for(int i =0;i<15;i++)
        {
           CreateGolemBoss();
        }

        for(int i =0; i<25; i++)
        {
            CreateShootingZombie();
        }
        for(int i =0;i<40;i++)
        {
            CreateBalloonYellow();
        }
        for(int i =0; i<25; i++)
        {
            CreateShootingZombie();
        }
        for(int i =0;i<40;i++)
        {
            CreateBalloonYellow();
        }
        itemSpawner.Spawn();
        for(int i =0; i<20; i++)
        {
            CreateZombie();
            CreateShootingZombie();
        }
        itemSpawner.Spawn();
        for(int i =0;i<10;i++)
        {
           CreateGolemBoss();
        }
        yield return new WaitForSeconds(12f);

        for(int i =0; i < 10; i ++)
            CreateBalloon();
        CreateSpider();
        CreateSpider();
        CreateBlueSkul();
    
        CreateSpiderMutant();
    }

    private IEnumerator EnemySpawn13()
    {

        CreateGolemBoss();
        CreateGolemBoss();
        CreateOrcBoss();
        CreateOrcBoss();
        for(int i =0; i<12;i++)
        {
            CreateHellSkul();
        } 
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);

        CreateRhino();
        CreateRhino();
        CreateGolemBoss();
        CreateGolemBoss();
        for(int i =0; i<10; i++)
        {
            CreateBalloonYellow();
            CreateWalker();
            CreateZombie();
        }
        CreateInsect();
        CreateSpiderMutant();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);

        for(int i =0; i<5; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        CreateInsect();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);

        for(int i =0;i<3;i++)
        {
            CreateInsect();
            CreateShootingZombie();
            CreateShootingZombie();
            CreateRhino();
        }
        CreateSpiderMutant();
        CreateRhino();
        CreateOrcBoss();
        CreateGolemBoss();
        CreateGolemBoss();
        CreateRhino();
        CreateOrcBoss();
        CreateGolemBoss();
        CreateGolemBoss();
        CreateRhino();
        CreateOrcBoss();
        CreateGolemBoss();
        CreateGolemBoss();
        CreateSpiderMutant();
        for(int i =0; i<10; i++)
        {
            CreateBalloonYellow();
            CreateWalker();
            CreateZombie();
        }

        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);

        for(int i =0; i<4; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }

        itemSpawner.Spawn();
        for(int i =0;i<3;i++)
        {
            CreateInsect();
        }
        CreateSpider();
        CreateRhino();
        CreateRhino();
        CreateSpider();
        for(int i =0;i<15;i++)
        {
           CreateGolemBoss();
        }
    }


    private IEnumerator EnemySpawn14() //오크 위주
    {
    
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            CreateBalloonYellow();
            CreateWalker();
        }
        CreateInsect();
        CreateSpiderMutant();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);

        for(int i =0; i<5; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        CreateInsect();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);

        for(int i =0;i<7;i++)
        {
            CreateInsect();
            CreateShootingZombie();
            CreateShootingZombie();
        }
                CreateOrcBoss();
        CreateOrcBoss();
                CreateOrcBoss();
        CreateOrcBoss();
                CreateOrcBoss();
        CreateOrcBoss();
        CreateGolemBoss();
        CreateGolemBoss();

        for(int i =0; i<10; i++)
        {
            CreateBalloonYellow();
            CreateWalker();
        }

        itemSpawner.Spawn();
        yield return new WaitForSeconds(1f);

        for(int i =0; i<4; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();

        itemSpawner.Spawn();    
        for(int i =0;i<3;i++)
        {
            CreateInsect();
        }
        itemSpawner.Spawn();

        for(int i =0; i<10; i++)
        {
            CreateOrcBoss();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(10f);

        for(int i =0; i<10; i++)
        {
            CreateOrcBoss();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(10f);

        for(int i =0; i<15; i++)
        {
            CreateOrcBoss();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(10f);

        for(int i =0; i<10; i++)
        {
            CreateOrcBoss();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(10f);

        for(int i =0; i<15; i++)
        {
            CreateOrcBoss();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(10f);

        for(int i =0; i<10; i++)
        {
            CreateOrcBoss();
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(10f);
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateRedOrc();
    }

    private IEnumerator EnemySpawn15()
    {

        CreateGolemBoss();
        CreateGolemBoss();
        CreateOrcBoss();
        CreateOrcBoss();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        yield return new WaitForSeconds(3f);
        for(int i =0;i<15;i++)
        {
           CreateGolemBoss();
        }
        itemSpawner.Spawn();
        for(int i =0; i<14; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);           
            CreateZombie(); 
        }
        itemSpawner.Spawn();

        itemSpawner.Spawn();
        for(int i =0;i<6;i++)
        {
           CreateOrcBoss();
           CreateGolemBoss();
        }
        yield return new WaitForSeconds(3f);
        for(int i=0; i<20; i++)
        {
            CreateHellSkul();
        }
        yield return new WaitForSeconds(3f);
        for(int i =0; i<25;i++)
        {
            CreateZombie();
            yield return new WaitForSeconds(0.25f);            
        }
        for(int i =0;i<15;i++)
        {
           CreateGolemBoss();
        }
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        for(int i=0; i<20; i++)
        {
            CreateHellSkul();
        }
        yield return new WaitForSeconds(3f);
        itemSpawner.Spawn();
    }

    private IEnumerator EnemySpawn16()
    {
        for(int i =0; i<25;i++)
        {
            CreateZombie();
            yield return new WaitForSeconds(0.25f);            
        }
        CreateBulDog();
        CreateGolemBoss();
        CreateGolemBoss();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateRhinoMutant();
        CreateGolemBoss();
        CreateRedOrc();
        CreateGolemBoss();
        CreateOrcBoss();
        CreateRhinoMutant();
        CreateRedOrc();
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        for(int i=0; i<20; i++)
        {
            CreateHellSkul();
        }
        yield return new WaitForSeconds(3f);

        CreateGolemBoss();
        CreateGolemBoss();        
        CreateGolemBoss();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateGolemBoss();        
        CreateGolemBoss();
        CreateGolemBoss();    
        
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateRhino();
        CreateRhino();   
        CreateRedOrc();    
        CreateRhino();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateGolemBoss();        
        CreateGolemBoss();
        CreateRedOrc();
        for(int i=0; i<20; i++)
        {
            CreateHellSkul();
        }
        yield return new WaitForSeconds(3f);
        for(int i =0; i<25;i++)
        {
            CreateZombie();
            yield return new WaitForSeconds(0.25f);            
        }
        itemSpawner.Spawn();
        itemSpawner.Spawn();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(3f);
        CreateStrongerGolemNoneBoss();
        CreateRedOrc();
        yield return new WaitForSeconds(3f);
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateStrongerGolemNoneBoss();
        CreateRedOrc();
        yield return new WaitForSeconds(3f);
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateStrongerGolemNoneBoss();
        CreateRedOrc();
        yield return new WaitForSeconds(3f);
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateSpiderMutant();
    }


    private IEnumerator EnemySpawn17() //구울 위주
    {
        CreateBulDog();
        CreateGolemBoss();
        CreateGolemBoss();
        CreateRedOrc();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateOrcBoss();
        CreateOrcBoss();        
        CreateRedOrc();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateGolemBoss();
        CreateGolemBoss();
        CreateOrcBoss();
        CreateRhinoMutant();
        CreateRhinoMutant();
        CreateRedOrc();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateRhinoMutant();
        CreateRhinoMutant();
        CreateRedOrc();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateRhinoMutant();
        CreateRhinoMutant();
        CreateRedOrc();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateOrcBoss();
        CreateGolemBoss();
        CreateGolemBoss();        
 
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        for(int i=0; i<20; i++)
        {
            CreateHellSkul();
        }
        yield return new WaitForSeconds(3f);
  
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        for(int i=0; i<20; i++)
        {
            CreateHellSkul();
        }
        CreateRedOrc();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateRedOrc();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);
        CreateRedOrc();
        CreateSpiderMutant();
        yield return new WaitForSeconds(3f);

        
        for(int i =0; i<20; i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);            
        }
        itemSpawner.Spawn();        
        yield return new WaitForSeconds(3f);
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        for(int i=0; i<20; i++)
        {
            CreateHellSkul();
        }
        yield return new WaitForSeconds(3f);
        
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);            
        }
        itemSpawner.Spawn();
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);            
        }
        itemSpawner.Spawn();
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);            
        }
        itemSpawner.Spawn();
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);            
        }
        itemSpawner.Spawn();
        for(int i =0; i<50; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);            
        }
        itemSpawner.Spawn();
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        
        for(int i =0; i<10; i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);            
        }
        itemSpawner.Spawn();
        for(int i =0; i<40; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        
        for(int i =0; i<30; i++)
        {
            float enemyIntensity = Random.Range(0f,2f);
            CreateMetalGhoul(enemyIntensity);            
        }
        itemSpawner.Spawn();


    }


    private IEnumerator EnemySpawn18()
    {
        for(int i =0; i<25;i++)
        {
            CreateZombie();
            yield return new WaitForSeconds(0.25f);            
        }
        CreateGolemBoss();
        CreateGolemBoss();
        CreateOrcBoss();
        CreateOrcBoss();
        CreateRhino();
        CreateRhino();
        CreateRhino();
        CreateOrcBoss();
        CreateOrcBoss();
        CreateRhinoMutant();
        CreateRhinoMutant();
        CreateRhinoMutant();
        CreateGolemBoss();
        CreateGolemBoss();        
         CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        for(int i =0; i<25;i++)
        {
            CreateZombie();
            yield return new WaitForSeconds(0.25f);            
        }
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        for(int i=0; i<20; i++)
        {
            CreateHellSkul();
        }
        yield return new WaitForSeconds(3f);
  
        for(int i =0; i<14; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }

        yield return new WaitForSeconds(3f);
        for(int i=0; i<20; i++)
        {
            CreateHellSkul();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<14; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateMetalGhoul(enemyIntensity);            
        }
        itemSpawner.Spawn();
        for(int i =0; i<25;i++)
        {
            CreateZombie();
            yield return new WaitForSeconds(0.25f);            
        }
    }

    private IEnumerator EnemySpawn19()
    {

        CreateGolemBoss();
        CreateGolemBoss();
        CreateOrcBoss();
        CreateOrcBoss();
        CreateRhinoMutant();
        CreateRhinoMutant();
        CreateRhinoMutant();
        CreateRhinoMutant();
        CreateRhinoMutant();
        CreateRhino();
        CreateRhino();
        CreateRhino();
        CreateRhino();
        CreateRhino();
        CreateRhino();
        CreateOrcBoss();
        CreateRedOrc();
        CreateRedOrc();
        CreateGolemBoss();
        CreateGolemBoss();        
 
        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
  
        for(int i =0; i<14; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }

        itemSpawner.Spawn();
        yield return new WaitForSeconds(3f);
        for(int i=0; i<20; i++)
        {
            CreateSkul();
            CreateHellSkul();
        }
        yield return new WaitForSeconds(3f);
        for(int i=0; i<20; i++)
        {
            CreateHellSkul();
        }
        yield return new WaitForSeconds(3f);
        for(int i=0; i<20; i++)
        {
            CreateHellSkul();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateMetalGhoul(enemyIntensity);            
        }
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
    }

    private IEnumerator EnemySpawn20()
    {

        CreateGolemBoss();
        CreateGolemBoss();
        CreateOrcBoss();
        CreateOrcBoss();
        CreateGolemBoss();
        CreateGolemBoss();
        CreateOrcBoss();
        CreateOrcBoss();
        CreateOrcBoss();
        CreateOrcBoss();
        CreateGolemBoss();
        CreateGolemBoss();        
 
        yield return new WaitForSeconds(3f);
  
        for(int i =0; i<14; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }

        yield return new WaitForSeconds(3f);

        for(int i =0; i<14; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateMetalGhoul(enemyIntensity);            
        }
        yield return new WaitForSeconds(10);
        CreateCrabBoss();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
        CreateSpider();
    }

    private IEnumerator WeekGen50()
    {
        for(int i =0; i<10; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);
        
        for(int i =0; i<10; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
    }

    private IEnumerator WeekGen100()
    {
        for(int i =0; i<20; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<20; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<20; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<20; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<20; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<20; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
        yield return new WaitForSeconds(3f);
        
        for(int i =0; i<20; i++)
        {
            //CreateBalloon();
            CreateBalloonYellow();
            CreateWalker();
            //CreateShootingZombie();
        }
    }


    private IEnumerator MetalGhoul30()
    {
        for(int i =0; i<10; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateMetalGhoul(enemyIntensity);            
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateMetalGhoul(enemyIntensity);            
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateMetalGhoul(enemyIntensity);            
        }
        yield return new WaitForSeconds(3f);
    }

    private IEnumerator Ghoul30()
    {
        for(int i =0; i<10; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        yield return new WaitForSeconds(3f);

        for(int i =0; i<10; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);            
        }
        yield return new WaitForSeconds(3f);
    }


    
}