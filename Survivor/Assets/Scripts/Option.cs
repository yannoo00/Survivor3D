using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Slider masterSlider;
    public AudioListener audioListener;
    private float masterVol=0.5f;
    public Slider musicSlider;
    public AudioSource musicAudio;
    private float musicVol=0.5f;

    public void masterAdjust()
    {
        AudioListener.volume = masterSlider.value;
        masterVol = masterSlider.value;
        PlayerPrefs.SetFloat("masterVol",masterVol);
    }
    public void musicAdjust()
    {
        musicAudio.volume = musicSlider.value;
        musicVol = musicSlider.value;
        PlayerPrefs.SetFloat("musicVol",musicVol);
    }


    void Start()
    {
        musicVol = PlayerPrefs.GetFloat("musicVol",0.5f);
        musicSlider.value = musicVol;
        musicAudio.volume = musicSlider.value;

        masterVol = PlayerPrefs.GetFloat("masterVol",0.5f);
        masterSlider.value = masterVol;
        AudioListener.volume = masterSlider.value;
    }

    void Update()
    {
        musicAdjust();
        masterAdjust();
    }
}
