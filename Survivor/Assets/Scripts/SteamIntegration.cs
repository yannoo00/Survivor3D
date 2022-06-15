using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SteamIntegration : MonoBehaviour
{
    

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        try
        {
            Steamworks.SteamClient.Init(1990620);
            PrintYourName();
        }

        catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private void PrintYourName()
    {
        Debug.Log(Steamworks.SteamClient.Name);
    }

    private void OnApplicationQuit() 
    {
        Steamworks.SteamClient.Shutdown();   
    }

    void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }

}
