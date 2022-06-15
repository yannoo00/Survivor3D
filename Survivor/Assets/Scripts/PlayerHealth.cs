using UnityEngine;
using UnityEngine.UI; // UI 관련 코드넌
using System.Collections;
using TMPro;

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity {
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더
    public Slider healthSlider2;
    //public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public AudioClip itemPickupClip; // 아이템 습득 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포트
    private Shooter Shooter; // 플레이어 슈터 컴포넌트

    public Slider shieldSlider;
    public Slider shieldSlider2;
    public int maxShield =100;
    public float Shield = 0;

    void Update()
    {
        GetComponent<PlayerMovement>().DashInput();
    }

    private void Awake() {
        // 사용할 컴포넌트를 가져오기

        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerMovement>();
        Shooter = GetComponent<Shooter>();

    
    }

    protected override void OnEnable() {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);

        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;
        healthSlider2.maxValue = startingHealth;
        healthSlider2.value = health;

        shieldSlider.gameObject.SetActive(true);
        shieldSlider.value = Shield;
        shieldSlider2.value = Shield;

        playerMovement.enabled = true;

        Shooter.enabled = true;

        
        StartCoroutine(HealthUIUpdate());
        StartCoroutine(ShieldUIUpdate());
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth) {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);

        healthSlider.value = health;
        healthSlider2.value=health;
    }

    public void ChrageShield(float newShield) {

        if(Shield+newShield>maxShield)
            Shield = maxShield;
        else
            Shield += newShield;

        shieldSlider.value = Shield;
        shieldSlider2.value = Shield;
    }

    // 데미지 처리
    public override void OnDamage(float damage) {
        // LivingEntity의 OnDamage() 실행(데미지 적용)

        if(!playerMovement.dashing)
        {
            if(!dead)
                {
                    playerAudioPlayer.PlayOneShot(hitClip);
                }
            //Debug.Log("Damaged!");
            //Debug.Log(damage);

            if(damage > Shield) //데미지 > 쉴드면, 쉴드는 0이되고 데미지는 (쉴드-데미지)로 바꿈.
            {
                Shield = 0;
                damage -= Shield;  
                base.OnDamage(damage);          
            }
            else  //데미지 < 쉴드면, 데미지만큼 쉴드만 깎아줌.
                Shield -= damage;

            GameObject hudText = Instantiate(hudDamageText);
            hudText.transform.position=transform.position+Vector3.up;
            hudText.GetComponent<FloatingDamage>().damage =(int)damage;
            hudText.GetComponent<TextMeshPro>().color= Color.red;


            shieldSlider.value = Shield;
            healthSlider.value = health;       
            shieldSlider2.value = Shield;
            healthSlider2.value = health;       
        }
        else
        {
            Debug.Log("Dash Miss!");
        }
    }

    // 사망 처리
    public override void Die() {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        healthSlider.gameObject.SetActive(false);



        //playerAudioPlayer.PlayOneShot(deathClip);

        playerAnimator.SetTrigger("Die");



        playerMovement.enabled = false;
        Shooter.enabled = false;

    } 

    private void OnTriggerEnter(Collider other) {
         // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리

         if(!dead)
         {
            //  IItemDrop item = other.GetComponent<IItemDrop>();

            //  if(item != null)
            //  {
            //     item.Use(gameObject);
            //     playerAudioPlayer.PlayOneShot(itemPickupClip);
            //     // if(gameObject.GetComponent<PlayerSkill>().skills[6])
            //     //     ChrageShield(10);
            //  }

            if(other.tag == "Finish")
                Die();
             
         }
    }

    private IEnumerator HealthUIUpdate()
    {
        while(true)
        {
            UIManager.instance.UpdateHPText(health,maxHealth);

            yield return new WaitForSeconds(0.25f);            
        }
    }
    private IEnumerator ShieldUIUpdate()
    {
        while(true)
        {
            UIManager.instance.UpdateShieldText(Shield,maxShield);

            yield return new WaitForSeconds(0.25f);            
        }
    }

    public void maxHealthUpdate(int max)
    {
        healthSlider.maxValue+=max;
        healthSlider2.maxValue+=max;
    }
    public void maxShieldUpdate(int max)
    {
        shieldSlider.maxValue+=max;
        shieldSlider2.maxValue+=max;
    }
}