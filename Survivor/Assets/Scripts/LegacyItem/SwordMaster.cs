using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwordMaster : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip sound;
    public event Action off;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() 
    {
        StartCoroutine(SoundPlay());
    }

    void Update()
    {
        
    }

    private IEnumerator SoundPlay()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(sound);
        yield return new WaitForSeconds(0.2f);
        audioSource.PlayOneShot(sound);
        yield return new WaitForSeconds(0.2f);
        audioSource.PlayOneShot(sound);
        if(off!=null)
            off();
    }
}
