using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class UIManager : MonoBehaviour
{
        // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }
    
    private static UIManager m_instance; //싱글톤이 할당 될 변수


    //public Text ammoText;

    public TextMeshProUGUI GemText; //강화용 젬 몇개 남았나 표시
    public GameObject ReloadText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI waveText; //웨이브 표시
    public TextMeshProUGUI ammoText;

    public TextMeshProUGUI DamageText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI SpeedText;
    public TextMeshProUGUI DelayText;
    public TextMeshProUGUI ShieldText;
    public TextMeshProUGUI ClearInfo;
    public TextMeshProUGUI dashcool;
    public TextMeshProUGUI dashspeed;
    public TextMeshProUGUI dashduration;

    public GameObject gameOverUI; //게임 오버시 활성화할 UI
    public GameObject clearUI; //클리어시 활성화할 UI

    public ItemAllocater itemAllocater;
    public Image[] itemImage;
    public TextMeshProUGUI[] stepText;
    public GameObject[] inventoryList;
    public GameObject[] RinvList;
    public GameObject optionUI;
    public GameObject inventoryUI;

    private int slotNum = 0;
    public float lastTime=180;

    private bool optionOn=false;
    private bool inventoryOn=false;
    private bool clear = false;
    private float currentTime=0;
    private float newTime=0;
    private int trial=0;

    private int[] numToSlot = new int[100];

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        currentTime = PlayerPrefs.GetFloat("currentTime",0);
        trial = PlayerPrefs.GetInt("trial",0);
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            newTime += 0.5f;
        }
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("escape!");
            OptionFreeze();
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoryFreeze();
        }
    }

    public void OptionFreeze()
    {
        if(Time.timeScale==1)
        {
            optionUI.gameObject.SetActive(true);
            Time.timeScale = 0;
            optionOn=true;
        }

        else if(Time.timeScale==0 && optionOn)
        {
            optionUI.gameObject.SetActive(false);
            Time.timeScale = 1;
            optionOn=false;
        }        
    }
    void InventoryFreeze()
    {
        if(Time.timeScale==1)
        {
            optionUI.gameObject.SetActive(true);
            optionOn=true;
            inventoryUI.gameObject.SetActive(true);
            Time.timeScale = 0;
            inventoryOn=true;
        }

        else if(Time.timeScale==0 && inventoryOn)
        {
            optionUI.gameObject.SetActive(false);
            optionOn=false;
            inventoryUI.gameObject.SetActive(false);
            Time.timeScale = 1;
            inventoryOn=false;
        }        
    }

    public void OptionClose()
    {
        Time.timeScale = 1;
        optionOn=false;
    }
    public void UpdateGemText(int newGem)
    {
        GemText.text = "Gem : " + newGem;
    }


    public void UpdateDelayText(float newDelay)
    {
        DelayText.text = "Reload Delay: " + string.Format("{0:0.#}",newDelay);
    }
    public void UpdateDamageText(float newDamage)
    {
        DamageText.text = "Damage: " + (int)newDamage;
    }
    public void UpdateSpeedText(float newSpeed)
    {
        SpeedText.text = "Speed: " + string.Format("{0:0.#}",newSpeed);
    }
    public void UpdateHPText(float newHP,float maxHP)
    {
        //HPText.text = "HP: " + string.Format("{0:0}",newHP) + "/100";
        HPText.text = "HP: " + (int)newHP + "/"+(int)maxHP;
    }
    public void UpdateShieldText(float newShield,float maxShield)
    {
        ShieldText.text = "Shield: " + (int)newShield + "/" + (int)maxShield;
    }

    public void UpdateTimer(float additionalTime)
    {
        if(lastTime <= 0&&-1<=lastTime)
        {
            GameObject death = Instantiate(GameManager.instance.death);
            death.transform.position = new Vector3(-30,0,20);
        }

        lastTime -= Time.deltaTime;
        int seconds = Mathf.RoundToInt(lastTime);
        int min;
        int sec;
        if(seconds/60>=1)
        {
            min  = seconds/60;
            sec = seconds%60;
        }
        else
        {
            min = 0;
            sec = seconds;
        }            
        timeText.text = min+ " : " +sec;        
    }

    public void SetTimer()
    {

    }



    public void UpdateDashCooldown(float cooldown)
    {
        dashcool.text = "Dash cooldown: "+string.Format("{0:0.00}",cooldown);
    }
    public void UpdateDashSpeed(float speed)
    {
        dashspeed.text = "Dash speed: "+string.Format("{0:0.0}",speed);
    }
    public void UpdateDashDuration(float duration)
    {
        dashduration.text = "Dash duration: "+string.Format("{0:0.0}",duration);
    }



    public void UpdateAmmoText(int magAmmo, int remainAmmo) {
        ammoText.text ="Ammo: "+ magAmmo + "/" + remainAmmo;
    }

    public void UpdateWaveText(int waves, int count) {
        waveText.text = "Wave : " + waves + "/20" + "\nEnemy : " + count;
        if(waves > 20&&!clear)
        {
            clear =true;
            trial++;
            PlayerPrefs.SetInt("trial",trial);

            clearUI.SetActive(true);         
            //float newTime = Time.time;
            ClearInfo.text = "Records: "+newTime+"seconds";

            if(newTime<currentTime||currentTime==0)
            {
                currentTime = newTime;
                PlayerPrefs.SetFloat("currentTime",currentTime);
            }            
        }

        PlayerPrefs.Save();

    }

    public void SetActiveGameoverUI(bool active) 
    {
        trial++;
        PlayerPrefs.SetInt("trial",trial);
        gameOverUI.SetActive(active);

        var ach = new Steamworks.Data.Achievement("firstDieACH");
        ach.Trigger();

        PlayerPrefs.Save();
    }
    

    public void UpdateInventory(int num)
    {
        numToSlot[num] = slotNum; //몇 번 아이템이 몇 번째 슬롯에 들어가있는지 저장.

        //SpriteRenderer sprite = itemAllocater.itemList[num].GetComponent<SpriteRenderer>();
        
        //if(sprite != null)
        //    itemImage[slotNum].sprite = sprite.sprite;

        Image image = itemAllocater.itemList[num].GetComponent<Image>();

        if(image != null)
            itemImage[slotNum].sprite = image.sprite;
    
        itemImage[slotNum].enabled = true;
        
        itemAllocater.itemList[num].GetComponent<ReinforceState>().slotNum =slotNum;
        
        
        inventoryList[slotNum].transform.GetChild(0).GetComponent<Image>().sprite=image.sprite;
        inventoryList[slotNum].transform.GetChild(0).gameObject.SetActive(true);
        inventoryList[slotNum].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text
        = itemAllocater.itemList[num].name;
        inventoryList[slotNum].transform.GetChild(2).gameObject.SetActive(true);
        inventoryList[slotNum].transform.GetChild(1).gameObject.SetActive(true);
        inventoryList[slotNum].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
        = itemAllocater.itemSkill[num];
        

        RinvList[slotNum].transform.GetChild(0).GetComponent<Image>().sprite=image.sprite;
        RinvList[slotNum].transform.GetChild(0).gameObject.SetActive(true);
        RinvList[slotNum].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text
        = itemAllocater.itemList[num].name;
        RinvList[slotNum].transform.GetChild(2).gameObject.SetActive(true);
        RinvList[slotNum].transform.GetChild(1).gameObject.SetActive(true);
        RinvList[slotNum].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
        = itemAllocater.itemSkill[num];

        RinvList[slotNum].GetComponent<Button>().enabled=true;
        RinvList[slotNum].GetComponent<Button>().onClick.AddListener(()=>itemAllocater.InvButton(num,RinvList[numToSlot[num]]));

        slotNum++;
    }

    public void UpdatePowerupInventory(int num)
    {
        Image image = itemAllocater.powerupList[num].GetComponent<Image>();

        if(image != null)
            itemImage[slotNum].sprite = image.sprite;

        itemImage[slotNum].enabled = true;


        inventoryList[slotNum].transform.GetChild(0).GetComponent<Image>().sprite=image.sprite;
        inventoryList[slotNum].transform.GetChild(0).gameObject.SetActive(true);
        inventoryList[slotNum].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text
        = itemAllocater.powerupList[num].name;
        inventoryList[slotNum].transform.GetChild(2).gameObject.SetActive(true);

        slotNum++;
    }
    
    public void UpdateReinforceState(int num)
    {
        stepText[itemAllocater.itemList[num].GetComponent<ReinforceState>().slotNum].enabled = true;
        stepText[itemAllocater.itemList[num].GetComponent<ReinforceState>().slotNum].text
        = "+"+itemAllocater.itemList[num].GetComponent<ReinforceState>().step;
    }

    public void SelectionRenew()
    {
        itemAllocater.buttonUI.SetActive(false);
        itemAllocater.ItemSet();
    }



    // 게임 재시작
    public void GameRestart() {
        SceneManager.LoadScene("Title");
        Time.timeScale = 1;
    }


}
