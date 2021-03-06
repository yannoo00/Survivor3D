using UnityEngine;

// 점수와 게임 오버 여부를 관리하는 게임 매니저
public class GameManager : MonoBehaviour {
    // 싱글톤 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    public int Gem = 0; // 현재 게임 보석
    //private float time;
    public GameObject death;


    ////////////////////////아이템 개수에 따라 수정/////////////////////////////
    public bool[] itemChecker = new bool[100];

    //////////////////////////////////////////////////////////////////////////

    
    public bool isGameover { get; private set; } // 게임 오버 상태

    private void Awake() {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }


    private void Start() {



        for(int i = 0; i < itemChecker.Length; i++)
        {
            itemChecker[i] = false;
        }


        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        //EndGame에 on Death 이벤트 구독 처리
        if(FindObjectOfType<PlayerHealth>()!=null)
            FindObjectOfType<PlayerHealth>().onDeath += EndGame;
        //FindObjectOfType<Base>().onDeath += EndGame;
    }


    // 점수를 추가하고 UI 갱신
    public void AddGem(int newGem) {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            Gem += newGem;
            // 점수 UI 텍스트 갱신
            UIManager.instance.UpdateGemText(Gem);
        }
    }

    public void MinusGem(int newGem) 
    {
        if(!isGameover)
        {
            Gem -= newGem;

            Debug.Log("Minus : "+newGem +" Current :" +Gem);

            UIManager.instance.UpdateGemText(Gem);

        }
    }

    public void AddItem(int itemNum)
    {
        itemChecker[itemNum] = true;

        UIManager.instance.UpdateInventory(itemNum);
    }


    // 게임 오버 처리
    public void EndGame() {
        // 게임 오버 상태를 참으로 변경
        
        isGameover = true;
        // 게임 오버 UI를 활성화
        UIManager.instance.SetActiveGameoverUI(true);
    }
}
