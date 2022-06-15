using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleScene : MonoBehaviour
{
    public string sceneName = "SurvivorMain";
    public GameObject mapUI;

    private void Awake()
    {
        Screen.SetResolution(2560, (2560/16) * 9, true);


    }

    public void ClickMap()
    {
        mapUI.SetActive(true);
    }



    public void ClickStart()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ClickQuit()
    {
        Application.Quit();
    }

}
