using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;



public class ItemAllocater : MonoBehaviour
{

    //아이템 리스트를 프리팹으로 다 받고
    //웨이브 클리어시 아이템 선택지 랜덤으로 3개 등장
    //플레이어는 그 중 하나 선택 -> 선택된 아이템을 생성
    //선택한 아이템은 플레이어의 자식 오브젝트가 되는 등 각자 효과에 맞게끔 구현
    
    
    public GameObject[] itemList;

    public GameObject buttonUI;
    public GameObject player;

    private Text firstText, secondText, thirdText;


    public EnemySpawner enemySpawner;

    private int chosen1, chosen2, chosen3;
    


    void Start()
    {

    }

    public void ItemSet()
    {
        Time.timeScale = 0;

        while(true)
        {
        chosen1 = Random.Range(0,itemList.Length);
        chosen2 = Random.Range(0,itemList.Length);
        chosen3 = Random.Range(0,itemList.Length);

        if(chosen1 != chosen2 && chosen2 != chosen3 && chosen1 != chosen3)
            break;
        }

        if(itemList[chosen1].name != null)
            //firstText.text = itemList[chosen1].ToString();
        if(itemList[chosen2].name != null)
            //secondText.text = itemList[chosen2].ToString();
        if(itemList[chosen3].name != null)
            //thirdText.text = itemList[chosen3].ToString();

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

    public void OnClickButton(int num)
    {
        IItem item;

        switch(num) //선택된 아이템의 Use 메소드 실행(Use로 아이템 효과 다 구현해야함)
        {

            case 1:
                item = itemList[chosen1].GetComponent<IItem>();
                item.Use(player); 
                break;
            
            case 2:
                item = itemList[chosen2].GetComponent<IItem>();
                item.Use(player); 
                break;
            
            case 3:
                item = itemList[chosen3].GetComponent<IItem>();
                item.Use(player);
                break;
        }


        Time.timeScale = 1;
        buttonUI.SetActive(false);
    }

    
    void Update()
    {
        
    }
}
