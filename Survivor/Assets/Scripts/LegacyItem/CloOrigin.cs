using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CloOrigin : MonoBehaviour
{
    private AudioSource audioSource;
    public event Action off;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    private void OnEnable()
    {
        StartCoroutine(Clo());
    }

    void Update()
    {
        
    }

    private IEnumerator Clo()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        yield return new WaitForSeconds(0.8f);
        if(off!=null)
            off();
    }
}
