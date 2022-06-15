using System.Collections;
using UnityEngine;

// 총을 구현한다
public class Gun : MonoBehaviour {
    // 총의 상태를 표현하는데 사용할 타입을 선언한다
    public enum State {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; } // 현재 총의 상태
    public LayerMask layerMask;
    public Transform fireTransform; // 총알이 발사될 위치
    private Vector3 realFireTransform;

    public PlayerMovement playerMovement;


    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    //public ParticleSystem shellEjectEffect; // 탄피 배출 효과
    public Material redLaser;
    Material originLaser;

    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기
    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리
    public AudioClip emptyFire;


    public bool spirit = false;
    public bool gunMan =false;
    public int thirdDamage = 0; //아이템 효과에 의한 3번째 공격의 추가 데미지

    public float damage = 20; // 공격력

    private float fireDistance = 20f; // 사정거리

    public int ammoRemain = 1000; // 남은 전체 탄약
    public int magCapacity = 25; // 탄창 용량
    public int magAmmo; // 현재 탄창에 남아있는 탄약


    int shootCount = 0;
    public float timeBetFire = 0.12f; // 총알 발사 간격
    public float reloadTime = 2f; // 재장전 소요 시간
    private float lastFireTime; // 총을 마지막으로 발사한 시점


    private void Awake() {
        // 사용할 컴포넌트들의 참조를 가져오기

        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;

        bulletLineRenderer.enabled =false;

        originLaser = bulletLineRenderer.material;
        
        
    }

    private void OnEnable() {
        // 총 상태 초기화

        magAmmo = magCapacity;

        state = State.Ready;

        lastFireTime = 0;

        StartCoroutine(GunUIUpdate());
    }

    private void Update(){
        realFireTransform = fireTransform.position + Vector3.up*1.1f + transform.right * 0.2f +transform.forward * 0.2f ;
        //muzzleFlashEffect.transform.position = realFireTransform;
    }


    // 발사 시도
    public void Fire() {

        if (state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot(); //총 발사

        }
        else if(state == State.Empty)
            gunAudioPlayer.PlayOneShot(emptyFire);
    }

    // 실제 발사 처리
    private void Shot() {
        
        //레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;

        //탄알이 맞은 곳을 저장할 변수
        Vector3 hitPosition = Vector3.zero;

        if(spirit) //아이템 먹은 경우에만 shootCount 증가
            shootCount++;


        //레이캐스트 (시작지점, 방향, 충돌정보 컨테이너, 사정거리)
        if (Physics.Raycast(realFireTransform, fireTransform.forward, out hit, fireDistance, layerMask))
        {


            //충돌한 상대방으로부터 IDamageable 오브젝트 가져오기 시도
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            // 상대방으로부터 IDamageable 오브젝트를 가져오는 데 성공했다면
            if (target != null && hit.collider.tag=="Enemy")
            {
                //상대방의 ondamage함수를 실행시켜 상대방에 대미지 주기
                //RaycastHit 타입의 normal은 맞은 표면 방향 정보

                if(shootCount ==3 ) //아이템 있고, 3타라면.
                {
                    target.OnDamage(damage+thirdDamage);
                }
                else
                    target.OnDamage(damage);
            }

            //레이의 충돌 위치 저장
            hitPosition = hit.point;
        }

            //73 255 242
        else
        {    
            hitPosition = realFireTransform + fireTransform.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        if(magAmmo <= 0)
        {
            state = State.Empty;
        }

    }

    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    private IEnumerator ShotEffect(Vector3 hitPosition) {

        muzzleFlashEffect.Play();
        //shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(shotClip); //playOneShot은 재생중인 오디오와 중첩 가능
//Debug.Log("shootCount : "+shootCount);
        if(shootCount == 3)
        {
            bulletLineRenderer.material = redLaser;
            // Color c1 = new Color(255,0,0);
            // lineRenderer.startColor = c1;
            // lineRenderer.endColor = Color.white;
            // Gradient gradient = new Gradient();
            // gradient.SetKeys(
            // new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            // new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });

            //lineRenderer.colorGradient = gradient;
        }

        bulletLineRenderer.SetPosition(0, realFireTransform); //라인 렌더러 시작 위치 = 총구 위치 
        bulletLineRenderer.SetPosition(1,hitPosition);

        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);



        if(shootCount >= 3)
        {
            
            bulletLineRenderer.material = originLaser;
            // Color c2 = new Color(73,255,242);
            // lineRenderer.startColor = c2;
            // lineRenderer.endColor = Color.white;

            shootCount = 0;
        }
        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload() {

        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
        {
            return false;
        }

        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine() {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
        
        UIManager.instance.ReloadText.SetActive(true);
        gunAudioPlayer.PlayOneShot(reloadClip);

        bool reloading=false;

        if(!gunMan)
        {
            playerMovement.moveSpeed -= 3f;     
            reloading = true;       
        }

        // 재장전 소요 시간 만큼 처리를 쉬기
        yield return new WaitForSeconds(reloadTime);

        UIManager.instance.ReloadText.SetActive(false);
        int ammoToFill = magCapacity - magAmmo;

        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;
        if(!gunMan||reloading)
        {
            playerMovement.moveSpeed += 3f;     
            reloading=false;  
        }

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }

    private IEnumerator GunUIUpdate()
    {
        while(true)
        {
            UIManager.instance.UpdateDelayText(reloadTime);
            UIManager.instance.UpdateDamageText(damage);

            yield return new WaitForSeconds(0.25f);            
        }

    }
}