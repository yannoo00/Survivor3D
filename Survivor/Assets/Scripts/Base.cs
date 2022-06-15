using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : LivingEntity
{

    public Slider healthSlider;
    private AudioSource audioSource;
    public AudioClip hitSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnDamage(float damage)
    {
        if(!dead)
        {
            audioSource.Play();
        }
        GameObject hudText = Instantiate(hudDamageText); // 생성할 텍스트 오브젝트
        hudText.transform.position = transform.position + Vector3.up; // 표시될 위치
        hudText.GetComponent<FloatingDamage>().damage = (int)damage; // 데미지 전달

        base.OnDamage(damage);
    }



    void Update()
    {
        
    }

    public override void Die()
    {
        base.Die();
    }
}
