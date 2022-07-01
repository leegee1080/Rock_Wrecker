using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class AchevementManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!SteamManager.Initialized){return;}
        SteamUserStats.StoreStats();

        SteamUserStats.SetAchievement("first_off_rock");
        
    }
}
