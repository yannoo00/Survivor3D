using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;



public class ItemAllocater : MonoBehaviour
{

    //아이템 리스트를 프리팹으로 다 받고
    //웨이브 클리어시 아이템 선택지 랜덤으로 3개 등장
    //플레이어는 그 중 하나 선택 -> 선택된 아이템을 생성
    //선택한 아이템은 플레이어의 자식 오브젝트가 되는 등 각자 효과에 맞게끔 구현
    
    
    public GameObject[] itemList; //아이템 담은 프리팹 배열
    public GameObject[] powerupList; //파워업 아이템 담은 프리팹 배열

    //GameManager gameManager = GameManager.instance;

    public GameObject buttonUI;     //프리팹 SelectionUI에 해당(활성/비활성화 용)
    public GameObject player;       //Use메소드에 전달

    public GameObject reinforceUI;
    public GameObject RchooseUI;
    public GameObject RpurchaseUI;
    public GameObject RchooseDone;

    //public GameObject buttonR[0],buttonR[1],buttonR[2],buttonR[3],buttonR[4],buttonR[5];
    public GameObject[] buttonR;
    public TextMeshProUGUI firstText, secondText, thirdText;    //아이템 이름 표시용
    //public TextMeshProUGUI firstText_r, secondText_r, thirdText_r; //강화 아이템 이름 표시용
    public TextMeshProUGUI firstContent, secondContent, thirdContent;   //아이템 내용 표시용
    //public TextMeshProUGUI firstContent_r, secondContent_r, thirdContent_r; //강화 내용 표시용

    //public TextMeshProUGUI firstGem, secondGem, thirdGem; //강화에 필요한 젬 표시



    public Image firstImg, secondImg, thirdImg; //아이템 아이콘 표시용
    //public Image firstImg_r, secondImg_r,thirdImg_r; //강화 아이템 아이콘 표시용

    
    //아이템 설명 저장용 딕셔너리
    public Dictionary<int,string> itemContent = new Dictionary<int,string>();
    //숫자(키)가 아이템 넘버다. 아이템 스크립트에서도 이를 활용하자.
    public Dictionary<int,string> powerupContent = new Dictionary<int, string>();
    public Dictionary<int,string> itemSkill = new Dictionary<int, string>();
    //public Dictionary<int, string> reinfoceContent = new Dictionary<int, string>();
    //강화 정보 저장하려면
    //1. 어떤 아이템인지
    //2. 어떤 테크인지
    //3. 어떤 레벨인지
    //아이템 별로 자료형을 만든다고 치면 구조체 배열?
    //무슨 아이템인지 받으면 해당 아이템의 테크-레벨 / 테크-레벨 / 테크-레벨 보여줘야함.

    //public int ITEM=9999;
    
    public PlayerSkill playerSkill;
    public EnemySpawner enemySpawner;

    private int chosen1, chosen2, chosen3;
    private int power1, power2, power3; //전체 강화 항목 중 선택된 것의 숫자
    int[] invCheck = new int[100];
    List<int> inv = new List<int>(); //강화를 위해 인벤토리에서 선택 된 3개의 숫자
    int doneCnt=0;
    //private int  chosenR[1], chosenR[2], chosenR[3], chosenR[4], chosenR[5];
    private int[] chosenR= new int[6];
    // public TextMeshProUGUI skillName1;
    // public TextMeshProUGUI skillName2;
    // public TextMeshProUGUI skillName3;
    public bool fullItem=false;


    public struct reinforce 
    {
        public int itemNum;
        public int reinfoceCode;

        public int needGem;

        public string reinforceContent;

        public bool check;
    }
    // 강화의 필요 gem과 내용을 여기서 모두 정리.




 ////////////////////강화 개수에 따라 수정////////////////////
    reinforce[] reinforces = new reinforce[114];
   //////////////////////////////////////////////////////////
    void Start()
    {
        
        
        for(int i = 0; i < reinforces.Length; i++)
            reinforces[i].check = false;

//////////////////////////////////////////////////////////////////////////
//////////// 강화 배열과 딕셔너리 늘리기, 게임매니저->아이템체커 늘리기 ///////
//////////////////////////////////////////////////////////////////////////

        reinforces[0].itemNum = 0;
        reinforces[0].reinfoceCode = 0;
        reinforces[0].needGem = 500;
        reinforces[0].reinforceContent
        ="Guardian Sword : Damage + 30";

        reinforces[1].itemNum = 0;
        reinforces[1].reinfoceCode = 1;
        reinforces[1].needGem = 800;
        reinforces[1].reinforceContent
        ="Guardian Sword : Speed + 100";

        reinforces[10].itemNum = 0;
        reinforces[10].reinfoceCode = 2;
        reinforces[10].needGem = 2000;
        reinforces[10].reinforceContent
        ="Guardian Sword : sword + 1";

        reinforces[25].itemNum = 0;
        reinforces[25].reinfoceCode = 3;
        reinforces[25].needGem = 1900;
        reinforces[25].reinforceContent
        ="Guardian Sword : Make sword bigger";

        reinforces[50].itemNum = 0;
        reinforces[50].reinfoceCode = 4;
        reinforces[50].needGem = 1400;
        reinforces[50].reinforceContent
        ="Guardian Sword : Speed + 150";

        reinforces[65].itemNum = 0;
        reinforces[65].reinfoceCode = 5;
        reinforces[65].needGem = 1600;
        reinforces[65].reinforceContent
        ="Guardian Sword : Speed + 50, damage+20";

/////////////////////////////////////////////////////////////

        reinforces[2].itemNum = 1;
        reinforces[2].reinfoceCode = 0;
        reinforces[2].needGem = 300;
        reinforces[2].reinforceContent
        ="Hell Sword : Damage + 100";

        reinforces[3].itemNum = 1;
        reinforces[3].reinfoceCode = 1;
        reinforces[3].needGem = 1200;
        reinforces[3].reinforceContent
        ="Hell Sword : Damage + 500";

        reinforces[39].itemNum = 1;
        reinforces[39].reinfoceCode = 2;
        reinforces[39].needGem = 400;
        reinforces[39].reinforceContent
        ="Hell Sword : cooldown - 3sec";

        reinforces[40].itemNum = 1;
        reinforces[40].reinfoceCode = 3;
        reinforces[40].needGem = 1200;
        reinforces[40].reinforceContent
        ="Hell Sword : cooldown - 2sec";

        reinforces[49].itemNum = 1;
        reinforces[49].reinfoceCode = 4;
        reinforces[49].needGem = 2100;
        reinforces[49].reinforceContent
        ="Hell Sword : Sword + 1";

        reinforces[48].itemNum = 1;
        reinforces[48].reinfoceCode = 5;
        reinforces[48].needGem = 2100;
        reinforces[48].reinforceContent
        ="Hell Sword : Sword + 1";


/////////////////////////////////////////////////////////////

        reinforces[4].itemNum = 2;
        reinforces[4].reinfoceCode = 0;
        reinforces[4].needGem = 900;
        reinforces[4].reinforceContent
        ="Templar's ring : Damage +15";

        reinforces[5].itemNum = 2;
        reinforces[5].reinfoceCode = 1;
        reinforces[5].needGem = 800;
        reinforces[5].reinforceContent
        ="Templar's ring : cooldown - 1 sec";

        reinforces[23].itemNum = 2;
        reinforces[23].reinfoceCode = 2;
        reinforces[23].needGem = 2200;
        reinforces[23].reinforceContent
        ="Templar's ring : projectile + 1";

        reinforces[24].itemNum = 2;
        reinforces[24].reinfoceCode = 3;
        reinforces[24].needGem = 2200;
        reinforces[24].reinforceContent
        ="Templar's ring : projectile + 1";

        reinforces[51].itemNum = 2;
        reinforces[51].reinfoceCode = 4;
        reinforces[51].needGem = 1300;
        reinforces[51].reinforceContent
        ="Templar's ring : Damage+10, cooldown-1";

/////////////////////////////////////////////////////////////

        reinforces[6].itemNum = 3;
        reinforces[6].reinfoceCode = 0;
        reinforces[6].needGem = 1000;
        reinforces[6].reinforceContent
        ="Templar's diary : Damage + 50, cooldown-0.5";

        reinforces[7].itemNum = 3;
        reinforces[7].reinfoceCode = 1;
        reinforces[7].needGem = 1300;
        reinforces[7].reinforceContent
        ="Templar's diary : Damage + 20, cooldown - 1sec";

        reinforces[11].itemNum = 3;
        reinforces[11].reinfoceCode = 2;
        reinforces[11].needGem = 400;
        reinforces[11].reinforceContent
        ="Templar's diary : Boundary Up";

        reinforces[46].itemNum = 3;
        reinforces[46].reinfoceCode = 3;
        reinforces[46].needGem = 1700;
        reinforces[46].reinforceContent
        ="Templar's diary : Explosion +1";

        reinforces[47].itemNum = 3;
        reinforces[47].reinfoceCode = 4;
        reinforces[47].needGem = 3500;
        reinforces[47].reinforceContent
        ="Templar's diary : cooldown - 2sec";

        reinforces[54].itemNum = 3;
        reinforces[54].reinfoceCode = 5;
        reinforces[54].needGem = 700;
        reinforces[54].reinforceContent
        ="Templar's diary : Damage + 30 cooldown -2sec";

        reinforces[53].itemNum = 3;
        reinforces[53].reinfoceCode = 6;
        reinforces[53].needGem = 2500;
        reinforces[53].reinforceContent
        ="Templar's diary : Damage + 100";

        reinforces[67].itemNum = 3;
        reinforces[67].reinfoceCode = 7;
        reinforces[67].needGem = 3500;
        reinforces[67].reinforceContent
        ="Templar's diary : Explosion +1";

/////////////////////////////////////////////////////////////

        reinforces[8].itemNum = 4;
        reinforces[8].reinfoceCode = 0;
        reinforces[8].needGem = 400;
        reinforces[8].reinforceContent
        ="Increasing Sword : cooldown -1, Damage + 10";

        reinforces[9].itemNum = 4;
        reinforces[9].reinfoceCode = 1;
        reinforces[9].needGem = 600;
        reinforces[9].reinforceContent
        ="Increasing Sword : Speed + 4, Damage + 10";

        reinforces[26].itemNum = 4;
        reinforces[26].reinfoceCode = 2;
        reinforces[26].needGem = 1100;
        reinforces[26].reinforceContent
        ="Increasing Sword : Throw swords twice";

        reinforces[27].itemNum = 4;
        reinforces[27].reinfoceCode = 3;
        reinforces[27].needGem = 1300;
        reinforces[27].reinforceContent
        ="Increasing Sword : Throw swords in eight direction";

        reinforces[66].itemNum = 4;
        reinforces[66].reinfoceCode = 4;
        reinforces[66].needGem = 300;
        reinforces[66].reinforceContent
        ="Increasing Sword : cooldown - 1.5sec, damage + 15";

        reinforces[71].itemNum = 4;
        reinforces[71].reinfoceCode = 5;
        reinforces[71].needGem = 600;
        reinforces[71].reinforceContent
        ="Increasing Sword : Go through 3 more enemies";

/////////////////////////////////////////////////////////////



        reinforces[12].itemNum = 5;
        reinforces[12].reinfoceCode = 0;
        reinforces[12].needGem = 400;
        reinforces[12].reinforceContent
        ="Drone Guardian : Magaizne capacity + 10, ammo + 200";


        reinforces[13].itemNum = 5;
        reinforces[13].reinfoceCode = 1;
        reinforces[13].needGem = 600;
        reinforces[13].reinforceContent
        ="Drone Guardian : reload delay - 0.2";


        reinforces[14].itemNum = 5;
        reinforces[14].reinfoceCode = 2;
        reinforces[14].needGem = 900;
        reinforces[14].reinforceContent
        ="Drone Guardian : reload delay - 0.3";

        reinforces[43].itemNum = 5;
        reinforces[43].reinfoceCode = 3;
        reinforces[43].needGem = 800;
        reinforces[43].reinforceContent
        ="Drone Guardian : Magazine capacity + 15, ammo + 400";

        reinforces[42].itemNum = 5;
        reinforces[42].reinfoceCode = 4;
        reinforces[42].needGem = 2000;
        reinforces[42].reinforceContent
        ="Drone Guardian : Gun Damage + 9";


        reinforces[44].itemNum = 5;
        reinforces[44].reinfoceCode = 5;
        reinforces[44].needGem = 6800;
        reinforces[44].reinforceContent
        ="Drone Guardian : Gun Damage + 36";


        reinforces[55].itemNum = 5;
        reinforces[55].reinfoceCode = 6;
        reinforces[55].needGem = 1100;
        reinforces[55].reinforceContent
        ="Drone Guardian : Generate 11 Shield per 25sec";



/////////////////////////////////////////////////////////////

        reinforces[15].itemNum = 6;
        reinforces[15].reinfoceCode = 0;
        reinforces[15].needGem = 800;
        reinforces[15].reinforceContent
        ="Soul Eater : Damage + 5";

        reinforces[16].itemNum = 6;
        reinforces[16].reinfoceCode = 1;
        reinforces[16].needGem = 1800;
        reinforces[16].reinforceContent
        ="Soul Eater : Delay - 0.2";

        reinforces[17].itemNum = 6;
        reinforces[17].reinfoceCode = 2;
        reinforces[17].needGem = 1000;
        reinforces[17].reinforceContent
        ="Soul Eater : Damage + 10";

        reinforces[18].itemNum = 6;
        reinforces[18].reinfoceCode = 3;
        reinforces[18].needGem = 2400;
        reinforces[18].reinforceContent
        ="Soul Eater : Delay - 0.3";

        reinforces[19].itemNum = 6;
        reinforces[19].reinfoceCode = 4;
        reinforces[19].needGem = 1700;
        reinforces[19].reinforceContent
        ="Soul Eater : Boundary Up";

        reinforces[45].itemNum = 6;
        reinforces[45].reinfoceCode = 5;
        reinforces[45].needGem = 2800;
        reinforces[45].reinforceContent
        ="Soul Eater : Damage + 10";

        reinforces[52].itemNum = 6;
        reinforces[52].reinfoceCode = 6;
        reinforces[52].needGem = 400;
        reinforces[52].reinforceContent
        ="Soul Eater : Drain HP(1 Enemy 1 Heal)";

/////////////////////////////////////////////////////////////


        reinforces[20].itemNum = 7;
        reinforces[20].reinfoceCode = 0;
        reinforces[20].needGem = 900;
        reinforces[20].reinforceContent
        ="Hell Pet : Damage +20, delay - 0.5";

        reinforces[21].itemNum = 7;
        reinforces[21].reinfoceCode = 1;
        reinforces[21].needGem = 700;
        reinforces[21].reinforceContent
        ="Hell Pet : Speed + 4, delay - 0.5";


        reinforces[22].itemNum = 7;
        reinforces[22].reinfoceCode = 2;
        reinforces[22].needGem = 1200;
        reinforces[22].reinforceContent
        ="Hell Pet : Damage + 40, Delay - 0.5";

        reinforces[38].itemNum = 7;
        reinforces[38].reinfoceCode = 3;
        reinforces[38].needGem = 1200;
        reinforces[38].reinforceContent
        ="Hell Pet : Drop fire bullets";


        reinforces[41].itemNum = 7;
        reinforces[41].reinfoceCode = 4;
        reinforces[41].needGem = 1300;
        reinforces[41].reinforceContent
        ="Hell Pet : Shot fire balls in three way ";

        reinforces[112].itemNum = 7;
        reinforces[112].reinfoceCode = 5;
        reinforces[112].needGem = 1700;
        reinforces[112].reinforceContent
        ="Hell Pet : Duplicate hell pet ";

        reinforces[113].itemNum = 7;
        reinforces[113].reinfoceCode = 6;
        reinforces[113].needGem = 1700;
        reinforces[113].reinforceContent
        ="Hell Pet : Duplicate hell pet ";

/////////////////////////////////////////////////////////////


        reinforces[28].itemNum = 8;
        reinforces[28].reinfoceCode = 0;
        reinforces[28].needGem = 700;
        reinforces[28].reinforceContent
        ="Silver Bolts : damage + 20";


        reinforces[29].itemNum = 8;
        reinforces[29].reinfoceCode = 1;
        reinforces[29].needGem = 300;
        reinforces[29].reinforceContent
        ="Silver Bolts : damage + 10";

        reinforces[30].itemNum = 8;
        reinforces[30].reinfoceCode = 2;
        reinforces[30].needGem = 5800;
        reinforces[30].reinforceContent
        ="Silver Bolts : damage + 100";

/////////////////////////////////////////////////////////////

        reinforces[31].itemNum = 9;
        reinforces[31].reinfoceCode = 0;
        reinforces[31].needGem = 2300;
        reinforces[31].reinforceContent
        ="Hell Breath : damage + 5, duration +1";

        reinforces[32].itemNum = 9;
        reinforces[32].reinfoceCode = 1;
        reinforces[32].needGem = 2100;
        reinforces[32].reinforceContent
        ="Hell Breath : cooldown - 1";

        reinforces[33].itemNum = 9;
        reinforces[33].reinfoceCode = 2;
        reinforces[33].needGem = 1800;
        reinforces[33].reinforceContent
        ="Hell Breath : Duration + 1";

        reinforces[34].itemNum = 9;
        reinforces[34].reinfoceCode = 3;
        reinforces[34].needGem = 1900;
        reinforces[34].reinforceContent
        ="Hell Breath : damage + 10";

        reinforces[35].itemNum = 9;
        reinforces[35].reinfoceCode = 4;
        reinforces[35].needGem = 3600;
        reinforces[35].reinforceContent
        ="Hell Breath : Breath in two ways";

        reinforces[36].itemNum = 9;
        reinforces[36].reinfoceCode = 5;
        reinforces[36].needGem = 2400;
        reinforces[36].reinforceContent
        ="Hell Breath : Duration + 2";

        reinforces[37].itemNum = 9;
        reinforces[37].reinfoceCode = 6;
        reinforces[37].needGem = 2500;
        reinforces[37].reinforceContent
        ="Hell Breath : cooldown - 2, damage +5";

/////////////////////////////////////////////////////////////


        reinforces[56].itemNum = 10;
        reinforces[56].reinfoceCode = 0;
        reinforces[56].needGem = 900;
        reinforces[56].reinforceContent
        ="Hell Scythe : Duration +1, damage +15";

        reinforces[57].itemNum = 10;
        reinforces[57].reinfoceCode = 1;
        reinforces[57].needGem = 800;
        reinforces[57].reinforceContent
        ="Hell Scythe : cooldown - 1, damage +8";

        reinforces[58].itemNum = 10;
        reinforces[58].reinfoceCode = 2;
        reinforces[58].needGem = 2500;
        reinforces[58].reinforceContent
        ="Hell Scythe : Duration + 1, cooldown-1";

        reinforces[59].itemNum = 10;
        reinforces[59].reinfoceCode = 3;
        reinforces[59].needGem = 700;
        reinforces[59].reinforceContent
        ="Hell Scythe : damage + 10";

        reinforces[60].itemNum = 10;
        reinforces[60].reinfoceCode = 4;
        reinforces[60].needGem = 3200;
        reinforces[60].reinforceContent
        ="Hell Scythe : Scythe + 1";

        reinforces[61].itemNum = 10;
        reinforces[61].reinfoceCode = 5;
        reinforces[61].needGem = 1700;
        reinforces[61].reinforceContent
        ="Hell Scythe : cooldown -3";

        reinforces[62].itemNum = 10;
        reinforces[62].reinfoceCode = 6;
        reinforces[62].needGem = 2300;
        reinforces[62].reinforceContent
        ="Hell Scythe : cooldown -1, damage +10, duration +1";

        reinforces[63].itemNum = 10;
        reinforces[63].reinfoceCode = 7;
        reinforces[63].needGem = 3200;
        reinforces[63].reinforceContent
        ="Hell Scythe : Scythe + 1";

        reinforces[64].itemNum = 10;
        reinforces[64].reinfoceCode = 8;
        reinforces[64].needGem = 600;
        reinforces[64].reinforceContent
        ="Hell Scythe : cooldown -2, damage +10";

        reinforces[72].itemNum = 10;
        reinforces[72].reinfoceCode = 9;
        reinforces[72].needGem = 3500;
        reinforces[72].reinforceContent
        ="Hell Scythe : Gain bigger";

/////////////////////////////////////////////////////////////


        reinforces[68].itemNum = 11;
        reinforces[68].reinfoceCode = 0;
        reinforces[68].needGem = 1500;
        reinforces[68].reinforceContent
        ="Templar's wand : cooldown -1";    

        reinforces[69].itemNum = 11;
        reinforces[69].reinfoceCode = 1;
        reinforces[69].needGem = 2900;
        reinforces[69].reinforceContent
        ="Templar's wand : cooldown -2";    

        reinforces[70].itemNum = 11;
        reinforces[70].reinfoceCode = 2;
        reinforces[70].needGem = 1800;
        reinforces[70].reinforceContent
        ="Templar's wand : cooldown -1.5";    


/////////////////////////////////////////////////////////////

        reinforces[73].itemNum = 12;
        reinforces[73].reinfoceCode = 0;
        reinforces[73].needGem = 900;
        reinforces[73].reinforceContent
        ="Poison rune : cooldown -1, damage +5";    

        reinforces[74].itemNum = 12;
        reinforces[74].reinfoceCode = 1;
        reinforces[74].needGem = 900;
        reinforces[74].reinforceContent
        ="Poison rune : duration +2, damage +5";    

        reinforces[75].itemNum = 12;
        reinforces[75].reinfoceCode = 2;
        reinforces[75].needGem = 900;
        reinforces[75].reinforceContent
        ="Poison rune : Scale up";    


/////////////////////////////////////////////////////////////

        reinforces[76].itemNum = 13;
        reinforces[76].reinfoceCode = 0;
        reinforces[76].needGem = 700;
        reinforces[76].reinforceContent
        ="Chief Dagger : cooldown -0.5, damage +50";    

        reinforces[77].itemNum = 13;
        reinforces[77].reinfoceCode = 1;
        reinforces[77].needGem = 700;
        reinforces[77].reinforceContent
        ="Chief Dagger : speed+3, damage +30";    

        reinforces[78].itemNum = 13;
        reinforces[78].reinfoceCode = 2;
        reinforces[78].needGem = 2100;
        reinforces[78].reinforceContent
        ="Chief Dagger : throw cycle + 1";    

        reinforces[98].itemNum = 13;
        reinforces[98].reinfoceCode = 3;
        reinforces[98].needGem = 2100;
        reinforces[98].reinforceContent
        ="Chief Dagger : throw cycle + 1";    

        reinforces[99].itemNum = 13;
        reinforces[99].reinfoceCode = 4;
        reinforces[99].needGem = 2100;
        reinforces[99].reinforceContent
        ="Chief Dagger : throw cycle + 1";    

        reinforces[100].itemNum = 13;
        reinforces[100].reinfoceCode = 5;
        reinforces[100].needGem = 2800;
        reinforces[100].reinforceContent
        ="Chief Dagger : throw dagger in three way";    


/////////////////////////////////////////////////////////////


        reinforces[79].itemNum = 14;
        reinforces[79].reinfoceCode = 0;
        reinforces[79].needGem = 1400;
        reinforces[79].reinforceContent
        ="Brutal Axe : cooldown -0.5, damage +20";    

        reinforces[80].itemNum = 14;
        reinforces[80].reinfoceCode = 1;
        reinforces[80].needGem = 2300;
        reinforces[80].reinforceContent
        ="Brutal Axe : throw cycle + 1";    

        reinforces[81].itemNum = 14;
        reinforces[81].reinfoceCode = 2;
        reinforces[81].needGem = 2300;
        reinforces[81].reinforceContent
        ="Brutal Axe : throw cycle + 1";    

        reinforces[84].itemNum = 14;
        reinforces[84].reinfoceCode = 3;
        reinforces[84].needGem = 1100;
        reinforces[84].reinforceContent
        ="Brutal Axe : rotatingSpeed+25%"; 


/////////////////////////////////////////////////////////////


        reinforces[82].itemNum = 15;
        reinforces[82].reinfoceCode = 0;
        reinforces[82].needGem = 900;
        reinforces[82].reinforceContent
        ="Musket : damage +5 ";    

        reinforces[83].itemNum = 15;
        reinforces[83].reinfoceCode = 1;
        reinforces[83].needGem = 1800;
        reinforces[83].reinforceContent
        ="Musket : damage +7";    


        reinforces[85].itemNum = 15;
        reinforces[85].reinfoceCode = 2;
        reinforces[85].needGem = 2100;
        reinforces[85].reinforceContent
        ="Musket : damage +10";    

/////////////////////////////////////////////////////////////

        reinforces[86].itemNum = 16;
        reinforces[86].reinfoceCode = 0;
        reinforces[86].needGem = 1500;
        reinforces[86].reinforceContent
        ="Epic Shield : Size up ";    

        reinforces[87].itemNum = 16;
        reinforces[87].reinfoceCode = 1;
        reinforces[87].needGem = 1500;
        reinforces[87].reinforceContent
        ="Epic Shield : Size up ";    


        reinforces[88].itemNum = 16;
        reinforces[88].reinfoceCode = 2;
        reinforces[88].needGem = 1500;
        reinforces[88].reinforceContent
        ="Epic Shield : Speed up 1.5 times";    

/////////////////////////////////////////////////////////////

        reinforces[89].itemNum = 17;
        reinforces[89].reinfoceCode = 0;
        reinforces[89].needGem = 1300;
        reinforces[89].reinforceContent
        ="Sniper : fire rate - 0.1sec faster ";    

        reinforces[90].itemNum = 17;
        reinforces[90].reinfoceCode = 1;
        reinforces[90].needGem = 1200;
        reinforces[90].reinforceContent
        ="Sniper : damage +12 ";    

        reinforces[91].itemNum = 17;
        reinforces[91].reinfoceCode = 2;
        reinforces[91].needGem = 1300;
        reinforces[91].reinforceContent
        ="Sniper : fire rate - 0.1sec faster";    

/////////////////////////////////////////////////////////////


        reinforces[92].itemNum = 18;
        reinforces[92].reinfoceCode = 0;
        reinforces[92].needGem = 700;
        reinforces[92].reinforceContent
        ="Machinegun : damage +3 ";    

        reinforces[93].itemNum = 18;
        reinforces[93].reinfoceCode = 1;
        reinforces[93].needGem = 1300;
        reinforces[93].reinforceContent
        ="Machinegun : damage +5 ";    


        reinforces[94].itemNum = 18;
        reinforces[94].reinfoceCode = 2;
        reinforces[94].needGem = 1500;
        reinforces[94].reinforceContent
        ="Machinegun : fire rate up";    

/////////////////////////////////////////////////////////////

        reinforces[95].itemNum = 19;
        reinforces[95].reinfoceCode = 0;
        reinforces[95].needGem = 600;
        reinforces[95].reinforceContent
        ="Space mine : cooldown -1sec, duration +3sec ";    

        reinforces[96].itemNum = 19;
        reinforces[96].reinfoceCode = 1;
        reinforces[96].needGem = 1700;
        reinforces[96].reinforceContent
        ="Space mine : damage +100, cooldown -1.5sec ";    


        reinforces[97].itemNum = 19;
        reinforces[97].reinfoceCode = 2;
        reinforces[97].needGem = 2100;
        reinforces[97].reinforceContent
        ="Space mine : Create one more object ";    

/////////////////////////////////////////////////////////////


        reinforces[101].itemNum = 20;
        reinforces[101].reinfoceCode = 0;
        reinforces[101].needGem = 500;
        reinforces[101].reinforceContent
        ="Shield slam : Make shield size bigger";    

        reinforces[102].itemNum = 20;
        reinforces[102].reinfoceCode = 1;
        reinforces[102].needGem = 1200;
        reinforces[102].reinforceContent
        ="Shield slam : Base damage +20, cooldown -0.5sec ";    

        reinforces[103].itemNum = 20;
        reinforces[103].reinfoceCode = 2;
        reinforces[103].needGem = 2100;
        reinforces[103].reinforceContent
        ="Shield slam : Shooting Cycle +1 ";    

        reinforces[107].itemNum = 20;
        reinforces[107].reinfoceCode = 3;
        reinforces[107].needGem = 700;
        reinforces[107].reinforceContent
        ="Shield slam : Gain 5 shield when shield hits enemy ";

        reinforces[108].itemNum = 20;
        reinforces[108].reinfoceCode = 4;
        reinforces[108].needGem = 600;
        reinforces[108].reinforceContent
        ="Shield slam : Damage increase by amount of the shield ";

/////////////////////////////////////////////////////////////
        reinforces[104].itemNum = 21;
        reinforces[104].reinfoceCode = 0;
        reinforces[104].needGem = 700;
        reinforces[104].reinforceContent
        ="Burning Shield : Base damage +5, boundary +1";    

        reinforces[105].itemNum = 21;
        reinforces[105].reinfoceCode = 1;
        reinforces[105].needGem = 1200;
        reinforces[105].reinforceContent
        ="Burning Shield : cooldown-0.3sec, boundary +1";    

        reinforces[106].itemNum = 21;
        reinforces[106].reinfoceCode = 2;
        reinforces[106].needGem = 1500;
        reinforces[106].reinforceContent
        ="Burning Shield : Damage increase by shield X 0.1 ";    

/////////////////////////////////////////////////////////////

        reinforces[109].itemNum = 22;
        reinforces[109].reinfoceCode = 0;
        reinforces[109].needGem = 1600;
        reinforces[109].reinforceContent
        ="Bloody Mace : Mace +2";    

        reinforces[110].itemNum = 22;
        reinforces[110].reinfoceCode = 1;
        reinforces[110].needGem = 1600;
        reinforces[110].reinforceContent
        ="Bloody Mace : Mace +2";    

        reinforces[111].itemNum = 22;
        reinforces[111].reinfoceCode = 2;
        reinforces[111].needGem = 700;
        reinforces[111].reinforceContent
        ="Bloody Mace : speed + 50";    

/////////////////////////////////////////////////////////////

        itemContent.Add(0,"Rotate around player");
        itemContent.Add(1,"Fall down to random point");
        itemContent.Add(2,"Fire energy to the closest enemyIf gun damage is less or 20, increase damage by 25");
        itemContent.Add(3,"Cause explosion to random enemy within the range.\nIf gun damage is less or 20, decrease cooldown by 1sec");
        itemContent.Add(4,"Throw knife in four direction");
        itemContent.Add(5,"Gun damage+2, magazine capacity+10, ammo+500, reloadTime-0.3");
        itemContent.Add(6,"Create aura around player");
        itemContent.Add(7,"Spit out a fire ball");
        itemContent.Add(8,"Reinforce every third attack. \nIf gun's damage is more than 40, increase damage by 20");
        itemContent.Add(9,"Breathe flame.");
        itemContent.Add(10,"Generate rotating scythe");
        itemContent.Add(11,"Cause cracks in the earth.\nIf gun damage is less or 20, increase damage by 70");
        itemContent.Add(12,"Call Poison Storm. \nIf gun damage is less or 20, increase damage by 24");
        itemContent.Add(13,"Throw Knife forward");
        itemContent.Add(14,"Throw rotating axe forward");
        itemContent.Add(15,"Active additional Rifle");
        itemContent.Add(16,"Two shields rotate around player. gain shield by 5 when Epic shield blocks bullet");
        itemContent.Add(17,"Active additional Siper Gun : Low fire rate, High damage, Through enemies");
        itemContent.Add(18,"Active additional Machinegun : faster fire rate");
        itemContent.Add(19,"Creates an object that explodes when enemy touches it.\nIncrease damage by amount of shield.");
        itemContent.Add(20,"Fire a shield. Increases damage by three times the shield.");
        itemContent.Add(21,"Max shield + 25, Deal damage to surrounding enemies(max :20).\n Damage and boudary increase by 10% of shield amount");
        itemContent.Add(22,"Rotate aruond player");

        //powerup item info
        powerupContent.Add(0,"moving speed +0.4");
        powerupContent.Add(1,"Gun fire damage+4, Dash Cooldown-2, Max Shield+10, Max HP+10");
        powerupContent.Add(2,"Gem +2000");
        powerupContent.Add(3,"Dash Speed Up, Dash cooldown-1");
        powerupContent.Add(4,"moving speed +0.8");
        powerupContent.Add(5,"magazine capacity +40");
        powerupContent.Add(6,"Gun fire damage +8");
        powerupContent.Add(7,"Shield +100, Health +100");
        powerupContent.Add(8,"Ammo +700");
        powerupContent.Add(9,"Max Shield+45");
        powerupContent.Add(10,"Dash Cooldown -3");


        //item skill info
        itemSkill.Add(0,"[Sword Master]");
        itemSkill.Add(1,"[Sword Master], [Hell Walker]");
        itemSkill.Add(2,"[High Templar]");
        itemSkill.Add(3,"[High Templar]");
        itemSkill.Add(4,"[Sword Master]");
        itemSkill.Add(5,"[Summoner]");
        itemSkill.Add(6,"[-]");
        itemSkill.Add(7,"[Hell Walker], [Summoner]");
        itemSkill.Add(8,"[-]");
        itemSkill.Add(9,"[Hell Walker]");
        itemSkill.Add(10,"[Hell Walker]");
        itemSkill.Add(11,"[High Templar]");
        itemSkill.Add(12,"[-]");
        itemSkill.Add(13,"[Barbarian]");
        itemSkill.Add(14,"[Barbarian]");
        itemSkill.Add(15,"[Sharp Shooter]");
        itemSkill.Add(16,"[Iron man]");
        itemSkill.Add(17,"[Sharp Shooter]");
        itemSkill.Add(18,"[Sharp Shooter]");
        itemSkill.Add(19,"[-]");
        itemSkill.Add(20,"[Iron man]");
        itemSkill.Add(21,"[Iron man]");
        itemSkill.Add(22,"[Barbarian]");

///////////////////////////////////////////////////////////////
        
    }
    public void RenewButton()
    {
        buttonUI.transform.GetChild(8).gameObject.SetActive(true);
    }



    public void ItemSet()
    {
        Time.timeScale = 0;

        while(true)
        {   //chosen을 랜덤생성(0 1 2 3 ...)
            chosen1 = Random.Range(0,itemList.Length); //itemList.Length는 포함 안됨.
            chosen2 = Random.Range(0,itemList.Length);
            chosen3 = Random.Range(0,itemList.Length);

            if(chosen1 != chosen2 && chosen2 != chosen3 && chosen1 != chosen3)
                break;
        }

        while(true)
        {
            power1 = Random.Range(0,powerupList.Length);
            power2 = Random.Range(0,powerupList.Length);
            power3 = Random.Range(0,powerupList.Length);

            if(power1 != power2 && power2 != power3 && power1 != power3)
                break;
        }
//Debug.Log("chsoen1 :" + chosen1);
        if(itemList[chosen1].name != null && !GameManager.instance.itemChecker[chosen1]&&!fullItem)
        {   
            firstText.text = itemList[chosen1].name+"   "+itemSkill[chosen1];
            firstContent.text = itemContent[chosen1];
            firstImg.sprite = itemList[chosen1].GetComponent<Image>().sprite;
            //skillName1.text = itemSkill[chosen1];
        }
        else
        {
            firstText.text = "Power Up: " + powerupList[power1].name;
            firstContent.text = powerupContent[power1];
            firstImg.sprite = powerupList[power1].GetComponent<Image>().sprite;
        }
        
//Debug.Log("chsoen2 :" + chosen2);            
        if(itemList[chosen2].name != null && !GameManager.instance.itemChecker[chosen2]&&!fullItem)
        {   
            secondText.text = itemList[chosen2].name+"   "+itemSkill[chosen2];
            secondContent.text = itemContent[chosen2];
            secondImg.sprite = itemList[chosen2].GetComponent<Image>().sprite;
            //skillName2.text = itemSkill[chosen2];
        }
        else
        {
            secondText.text = "Power Up: " + powerupList[power2].name;
            secondContent.text = powerupContent[power2];
            secondImg.sprite = powerupList[power2].GetComponent<Image>().sprite;
        }

//Debug.Log("chsoen3 :" + chosen3);
        if(itemList[chosen3].name != null && !GameManager.instance.itemChecker[chosen3]&&!fullItem)
        {   
            thirdText.text = itemList[chosen3].name+"   "+itemSkill[chosen3];
            thirdContent.text = itemContent[chosen3];
            thirdImg.sprite = itemList[chosen3].GetComponent<Image>().sprite;
            //skillName3.text = itemSkill[chosen3];
        }
        else
        {
            thirdText.text = "Power Up: " + powerupList[power3].name;
            thirdContent.text = powerupContent[power3];
            thirdImg.sprite = powerupList[power3].GetComponent<Image>().sprite;
        }


        buttonUI.SetActive(true);
        //UI에 선택된 숫자에 따른 아이템 등장(3개)

        return;
    }

 
    //플레이어가 선택하면 -> 선택했을 때 움직여야함. input처럼..
    // => 버튼 클릭시 실행되는 함수에서 나머지 일 처리하면 됨
    //해당 아이템 생성(Player 자식으로 gogo)
    //플레이어가 선택한 버튼의 숫자(chosen)을 가져와서,
    //아이템 게임오브젝트 배열 중 해당 숫자가 인덱스인 게임오브젝트를 생성,
    //플레이어의 자식 오브젝트로 추가하고 USE메소드 사용.
    public void InvButton(int num, GameObject button)
    {
        // 한 번 누르면 세 개 체커 ++.
        // 이미 눌린걸 한 번 더 누르면 세 개 체커 --.
        // 이렇게 해서 만약 세 개 체커가 3이라면 done을 활성화 시킨다.
        // 세 개 체커는 강화 창 활성화 시 마다 초기화시킨다.
        //Debug.Log(button.name);
        // if(invIndex>=3) //선택을 취소했다가 다시 고르느라 선택 횟수가 3보다 큰 경우,
        //                 //취소했던 index를 찾아 거길 다시 채움.
        // {
        //     bool find=false;

        //     for(int i=0; i<3; i++)
        //     {
        //         if(inv[i]==999)
        //         {
        //             invIndex=i;      
        //             find= true;              
        //         }
        //     }                
        //     if(!find) //3개 누르고 취소 없이 더 누른 상황
        //     {
                
        //         return;//취소 없이 더 누르면 선택 안됨
        //     }
        // }

        // if(invCheck[num]==2)//3번누름==활성화, 비활성화, 활성화
        //     invCheck[num]=0;

        // if(invCheck[num]==1) //같은거 두 번 눌렀을 때(첨엔 0, 두 번째에는 1)
        // {
        //     button.transform.parent.GetComponent<Image>().color=new Color(255,255,255,255);
        //     for(int i =0; i<3; i++)
        //     {
        //         if(inv[i]==num)
        //             inv[i]=999; //첨에 할당했던 값을 999로 바꿔버림.
        //     }
        //     doneCnt--; //done이 못나오게 donecnt줄여줌
        // }

        // else if(invCheck[num]==0) //처음 누르는 상황일 때(혹은 비활성화 후 활성화)
        // {
        //     button.transform.parent.GetComponent<Image>().color=new Color(0,255,255,255);
        //     inv[invIndex] = num;
        //     //Debug.Log(inv[invIndex]);
        //     doneCnt++; //취소 제외 선택된 아이템의 갯수
        //     invIndex++; //할당할 아이템 번호들 배열의 인덱스(0->1->2)
        // }

        
        // invCheck[num]++; //같은 아이템 버튼을 누른 횟수

        // //Debug.Log(doneCnt);

        ///////////////////////////////////////////////////////////////////

        invCheck[num]++; //한 아이템 버튼을 누른 횟수. 시작부터 1.

        if(doneCnt>=3) //전체 선택된 것의 개수가 이미 3개 이상
        {
            if(invCheck[num]%2==0)//한(이 것) 아이템 버튼을 누른 횟수가 짝수면
            {
                inv.Remove(num);//선택된 것들의 List에서 이 num을 빼주고
                doneCnt--; //전체 선택된 것의 개수를 줄인다.
                button.transform.parent.GetComponent<Image>().color=new Color(255,255,255,255);
            }

            if(invCheck[num]%2==1)//한(이 것) 아이템 버튼을 누른 횟수가 홀수면
            {
                invCheck[num]--;
                return;//아무 일도 안일어남
            }
        }

        else if(doneCnt<3) //전체 선택된 것의 개수가 3개 미만
        {
            if(invCheck[num]%2==0)//한(이 것) 아이템 버튼을 누른 횟수가 짝수면
            {
                inv.Remove(num);//선택된 것들의 List에서 이 num을 빼주고
                doneCnt--; //전체 선택된 것의 개수를 줄인다.
                button.transform.parent.GetComponent<Image>().color=new Color(255,255,255,255);
            }

            if(invCheck[num]%2==1)//한(이 것) 아이템 버튼을 누른 횟수가 홀수면
            {
                inv.Add(num);
                doneCnt++; //전체 선택된 것의 개수를 증가시킨다.
                button.transform.parent.GetComponent<Image>().color=new Color(0,255,255,255);
            }
        }


        if(doneCnt==3)
            RchooseDone.SetActive(true); //3개 선택한 상황이라면 done활성화
        else if(doneCnt<3)
            RchooseDone.SetActive(false);
    }

    public void OnClickButton(int num)
    {
        IItem item;

        switch(num) //선택된 아이템의 Use 메소드 실행(Use로 아이템 효과 다 구현해야함)
        {

            case 1:

                if(!GameManager.instance.itemChecker[chosen1]&&!fullItem)
                {
                    item = itemList[chosen1].GetComponent<IItem>();
                    item.Use(player); 
                    GameManager.instance.AddItem(chosen1);

                    break;                    
                }
                else
                {
                    item = powerupList[power1].GetComponent<IItem>();
                    item.Use(player);
                    UIManager.instance.UpdatePowerupInventory(power1);
                    break;
                }

            
            case 2:

                if(!GameManager.instance.itemChecker[chosen2]&&!fullItem)
                {
                    item = itemList[chosen2].GetComponent<IItem>();
                    item.Use(player); 
                    GameManager.instance.AddItem(chosen2);
                    break;                    
                }
                else
                {
                    item = powerupList[power2].GetComponent<IItem>();
                    item.Use(player);
                    UIManager.instance.UpdatePowerupInventory(power2);
                    break;
                }
            
            case 3:

                if(!GameManager.instance.itemChecker[chosen3]&&!fullItem)
                {
                    item = itemList[chosen3].GetComponent<IItem>();
                    item.Use(player); 
                    GameManager.instance.AddItem(chosen3);
                    break;                    
                }
                else
                {
                    item = powerupList[power3].GetComponent<IItem>();
                    item.Use(player);
                    UIManager.instance.UpdatePowerupInventory(power3);
                    break;
                }
        }
        //Time.timeScale = 1;
        buttonUI.SetActive(false);

        //아이템 선택하고 나면 곧바로 강화창 등장
        // if(enemySpawner.GetComponent<EnemySpawner>().wave >= 3) //강화창 on
        

        if(ItemNumChecker()>=3)
        {            
            
            doneCnt=0;
            for(int i =0; i<100;i++)
                invCheck[i]=0;          //변수들 초기화
            inv.Clear();

            reinforceUI.SetActive(true); //강화 UI 활성화
            RchooseUI.SetActive(true);
            RpurchaseUI.SetActive(false);
            RchooseDone.SetActive(false);
            //ReinforcePurchase();       
        }
        else
        {
            Time.timeScale = 1; //일단 일시정지 해제하고
            playerSkill.SkillCheck(); //만약 스킬을 획득하면 다시 일시정지 걸림
            
        }
            
    }
    

    public void ReinforcePurchase() //아이템 선택창에서 done누르면 실행
    {
        RpurchaseUI.SetActive(true);

        List<int> randomNumber = new List<int>();    
        int temp = Random.Range(0,reinforces.Length);//temp는 강화항목의 넘버
        int defense = 0;

        while(randomNumber.Count<6) //갖고 있는 아이템의 강화인지도 체크해야함
        {
            while(defense < 10001)
            {
                if(!randomNumber.Contains(temp) && !reinforces[temp].check)//리스트에 없고, 구매되지 않은.
                {
                    //Debug.Log("now:"+temp);
                    if(reinforces[temp].itemNum==inv[0]||reinforces[temp].itemNum==inv[1]||reinforces[temp].itemNum==inv[2])
                    {
                        randomNumber.Add(temp);
                        //Debug.Log("added:"+temp);
                        break;                        
                    }
                    else
                        temp = Random.Range(0,reinforces.Length);
                }// 아이템체커==트루인지 확인할 필요 없이 선택된 세 개의 숫자 중 하나인지 확인한다.
                
                else
                    temp = Random.Range(0,reinforces.Length);
                

                defense ++;
            }       

            if(defense>=9999)
                break;
        }// randomNumber에 중복 없이 강화항목 중 숫자 6개 선발 완료(while종료)
        
        for(int i=0;i<randomNumber.Count;i++)
        {
            ReinforceUIupdate(buttonR[i], randomNumber[i]);

            chosenR[i]  = randomNumber[i];
                
        }

    
    
    }



    //플레이어가 얻은 아이템 중 랜덤으로 3개를 띄우기
    //0부터 아이템 리스트. length까지 중 랜덤으로 숫자 3개 get,
    //그 숫자들의 gameManager itemChecker가 true인지 검사. 다 true면 화면에 띄우기.


    public void OnClickButton2 (int num)
    {
        switch(num)
        {
            case 0:
                reinforceUI.SetActive(false);
                
                Time.timeScale = 1;
                playerSkill.SkillCheck();
                break;



            case 1: //1번 강화 선택됨 -> 충분한 젬이 있는지 확인 -> 있다면, 젬 제거 후 구매.
                if(GameManager.instance.Gem >= reinforces[chosenR[0]].needGem )
                {
                    GameManager.instance.MinusGem(reinforces[chosenR[0]].needGem);
                    IItem item = itemList[reinforces[chosenR[0]].itemNum].GetComponent<IItem>();
                    item.Reinforce(reinforces[chosenR[0]].reinfoceCode);

                    reinforces[chosenR[0]].check = true;
                    buttonR[0].SetActive(false);

                    UIManager.instance.UpdateReinforceState(reinforces[chosenR[0]].itemNum);
                
                    ReinforceGemText(buttonR[0],chosenR[0]);
                    ReinforceGemText(buttonR[1],chosenR[1]);
                    ReinforceGemText(buttonR[2],chosenR[2]);
                    ReinforceGemText(buttonR[3],chosenR[3]);
                    ReinforceGemText(buttonR[4],chosenR[4]);
                    ReinforceGemText(buttonR[5],chosenR[5]);

                }
                else
                    Debug.Log("Not Enough Gem !");

                break;



            case 2:
                if(GameManager.instance.Gem >= reinforces[chosenR[1]].needGem )
                {
                    GameManager.instance.MinusGem(reinforces[chosenR[1]].needGem);
                    IItem item = itemList[reinforces[chosenR[1]].itemNum].GetComponent<IItem>();
                    item.Reinforce(reinforces[chosenR[1]].reinfoceCode);

                    reinforces[chosenR[1]].check = true;
                    buttonR[1].SetActive(false);
                    UIManager.instance.UpdateReinforceState(reinforces[chosenR[1]].itemNum);
                
                    ReinforceGemText(buttonR[0],chosenR[0]);
                    ReinforceGemText(buttonR[1],chosenR[1]);
                    ReinforceGemText(buttonR[2],chosenR[2]);
                    ReinforceGemText(buttonR[3],chosenR[3]);
                    ReinforceGemText(buttonR[4],chosenR[4]);
                    ReinforceGemText(buttonR[5],chosenR[5]);
                
                }
                else
                    Debug.Log("Not Enough Gem !");

                break;



            case 3:
                if(GameManager.instance.Gem >= reinforces[chosenR[2]].needGem )
                {
                    GameManager.instance.MinusGem(reinforces[chosenR[2]].needGem);
                    IItem item = itemList[reinforces[chosenR[2]].itemNum].GetComponent<IItem>();
                    item.Reinforce(reinforces[chosenR[2]].reinfoceCode);

                    reinforces[chosenR[2]].check = true;
                    buttonR[2].SetActive(false);
                    UIManager.instance.UpdateReinforceState(reinforces[chosenR[2]].itemNum);
                
                    ReinforceGemText(buttonR[0],chosenR[0]);
                    ReinforceGemText(buttonR[1],chosenR[1]);
                    ReinforceGemText(buttonR[2],chosenR[2]);
                    ReinforceGemText(buttonR[3],chosenR[3]);
                    ReinforceGemText(buttonR[4],chosenR[4]);
                    ReinforceGemText(buttonR[5],chosenR[5]);

                }
                else
                    Debug.Log("Not Enough Gem !");

                break;



            case 4:
                if(GameManager.instance.Gem >= reinforces[chosenR[3]].needGem )
                {
                    GameManager.instance.MinusGem(reinforces[chosenR[3]].needGem);
                    IItem item = itemList[reinforces[chosenR[3]].itemNum].GetComponent<IItem>();
                    item.Reinforce(reinforces[chosenR[3]].reinfoceCode);

                    reinforces[chosenR[3]].check = true;
                    buttonR[3].SetActive(false);
                    UIManager.instance.UpdateReinforceState(reinforces[chosenR[3]].itemNum);
                
                    ReinforceGemText(buttonR[0],chosenR[0]);
                    ReinforceGemText(buttonR[1],chosenR[1]);
                    ReinforceGemText(buttonR[2],chosenR[2]);
                    ReinforceGemText(buttonR[3],chosenR[3]);
                    ReinforceGemText(buttonR[4],chosenR[4]);
                    ReinforceGemText(buttonR[5],chosenR[5]);
                
                }
                else
                    Debug.Log("Not Enough Gem !");

                break;



            case 5:
                if(GameManager.instance.Gem >= reinforces[chosenR[4]].needGem )
                {
                    GameManager.instance.MinusGem(reinforces[chosenR[4]].needGem);
                    IItem item = itemList[reinforces[chosenR[4]].itemNum].GetComponent<IItem>();
                    item.Reinforce(reinforces[chosenR[4]].reinfoceCode);

                    reinforces[chosenR[4]].check = true;
                    buttonR[4].SetActive(false);
                    UIManager.instance.UpdateReinforceState(reinforces[chosenR[4]].itemNum);
                
                    ReinforceGemText(buttonR[0],chosenR[0]);
                    ReinforceGemText(buttonR[1],chosenR[1]);
                    ReinforceGemText(buttonR[2],chosenR[2]);
                    ReinforceGemText(buttonR[3],chosenR[3]);
                    ReinforceGemText(buttonR[4],chosenR[4]);
                    ReinforceGemText(buttonR[5],chosenR[5]);
                
                }
                else
                    Debug.Log("Not Enough Gem !");

                break;



            case 6:
                if(GameManager.instance.Gem >= reinforces[chosenR[5]].needGem )
                {
                    GameManager.instance.MinusGem(reinforces[chosenR[5]].needGem);
                    IItem item = itemList[reinforces[chosenR[5]].itemNum].GetComponent<IItem>();
                    item.Reinforce(reinforces[chosenR[5]].reinfoceCode);

                    reinforces[chosenR[5]].check = true;
                    buttonR[5].SetActive(false);
                    UIManager.instance.UpdateReinforceState(reinforces[chosenR[5]].itemNum);
                
                    ReinforceGemText(buttonR[0],chosenR[0]);
                    ReinforceGemText(buttonR[1],chosenR[1]);
                    ReinforceGemText(buttonR[2],chosenR[2]);
                    ReinforceGemText(buttonR[3],chosenR[3]);
                    ReinforceGemText(buttonR[4],chosenR[4]);
                    ReinforceGemText(buttonR[5],chosenR[5]);
                
                }
                else
                    Debug.Log("Not Enough Gem !");

                break;
        }
    }






   public void ReinforceUIupdate(GameObject button, int num) //랜덤 숫자 로직
   {
       TextMeshProUGUI content = button.GetComponentInChildren<TextMeshProUGUI>();
       Image img = button.transform.GetChild(1).GetComponent<Image>();
       TextMeshProUGUI GemText = button.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
       

        content.text = reinforces[num].reinforceContent;
        GemText.text = reinforces[num].needGem.ToString() + " Gem";
        if(GameManager.instance.Gem<reinforces[num].needGem)
            GemText.text = "<color=#ff0000>"+ reinforces[num].needGem.ToString() + " Gem";
        img.sprite = itemList[reinforces[num].itemNum].GetComponent<Image>().sprite;
        button.SetActive(true);
   }

    void ReinforceGemText(GameObject button, int num)
    {
        TextMeshProUGUI GemText = button.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        
        GemText.text = reinforces[num].needGem.ToString() + " Gem";
        
        if(GameManager.instance.Gem<reinforces[num].needGem)
            GemText.text = "<color=#ff0000>"+ reinforces[num].needGem.ToString() + " Gem";
    }


    public int ItemNumChecker() //보유 중인 아이템 개수 반환
    {   
        int num = 0;

        for(int i = 0; i < GameManager.instance.itemChecker.Length; i++)
        {
            if(GameManager.instance.itemChecker[i])
                num++;
        }
        
        return num;
    }
    

    
    void Update()
    {
        
    }



    
}

//아이템별로 강화 항목이 여러 개 있고 그 중에서 랜덤으로 뜨게 할까?(단계별로 강화가 아니라). 어떤 강화는 조건부로 등장하는 식으로 하면 될거같음.

//아이템 이미지, 아이템 강화 내용, 필요 젬. (강화 단계 따위는 없음)
//젬 계산 필요
//아이템 하나당 여러가지 강화가 있음.
//아이템 0번의 1, 2, 3, 4, 5, ... 강화가 있고 이것들중에 랜덤 선택
