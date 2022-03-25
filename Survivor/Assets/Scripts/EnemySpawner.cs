using System.Collections.Generic;
using UnityEngine;
using System.Collections;

// 적 게임 오브젝트를 주기적으로 생성
public class EnemySpawner : MonoBehaviour {
    public Ghoul ghoulPrefab; // 생성할 적 AI

    public Transform[] spawnPoints; // 적 AI를 소환할 위치들

    public GameObject itemAllocater;

    //public ItemSpawner itemSpawner;

    public float damageMax = 40f; // 최대 공격력
    public float damageMin = 10f; // 최소 공격력

    public float healthMax = 250f; // 최대 체력
    public float healthMin = 150f; // 최소 체력

    public float speedMax = 2f; // 최대 속도
    public float speedMin = 1f; // 최소 속도

    public Color strongEnemyColor = Color.red; // 강한 적 AI가 가지게 될 피부색

    private List<Ghoul> GhoulEnemies = new List<Ghoul>(); // 생성된 적들을 담는 리스트
    private int wave; // 현재 웨이브



 

    private void Update() {
        // 게임 오버 상태일때는 생성하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        // 적을 모두 물리친 경우 다음 스폰 실행
        if (GhoulEnemies.Count <= 0)
        {
            //StartCoroutine(Delay());
            SpawnWave();
        }

        // UI 갱신
        UpdateUI();
    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI() {
        // 현재 웨이브와 남은 적의 수 표시
        //UIManager.instance.UpdateWaveText(wave, GhoulEnemies.Count);
    }
    




    // 현재 웨이브에 맞춰 적을 생성
    private void SpawnWave() {


        //일시정지하고 아이템 획득하게 하는 함수 실행
        itemAllocater.GetComponent<ItemAllocater>().ItemSet();

        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        for (int i = 0; i < spawnCount; i++)
        {
            float enemyIntensity = Random.Range(0f,1f);
            CreateEnemy(enemyIntensity);
        }

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



        //람다식을 이용해 익명 함수 만들기
        ghoul.onDeath += () => GhoulEnemies.Remove(ghoul);

        ghoul.onDeath += () => Destroy(ghoul.gameObject, 10f);

        //ghoul.onDeath += () => GameManager.instance.AddScore(100);

        // if(intensity >= 0.8f)
        //     ghoul.onDeath += () => itemSpawner.Drop(enemy.transform.position);
    }



    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        SpawnWave();
    }


}