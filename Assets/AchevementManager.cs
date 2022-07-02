using UnityEngine;
using Steamworks;

public class AchevementManager : MonoBehaviour
{
    public static AchevementManager singlton;
    private void Awake()
    {
        if (singlton == null)
        {
            singlton = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }

    private void StoreState()
    {
        if(!SteamManager.Initialized){return;}
        SteamUserStats.StoreStats();    
    }

    public void EscapeRock()
    {
        SteamUserStats.SetStat("escape_rock",1);
        StoreState();
    }
    public void EraseGame()
    {
        SteamUserStats.SetStat("erase_stats",1);
        StoreState();
    }
    public void Gold(int amt)
    {
        SteamUserStats.SetStat("gold",amt);
        StoreState();
    }
    public void Shield()
    {
        SteamUserStats.SetStat("shield",1);
        StoreState();
    }
    public void Death()
    {
        SteamUserStats.SetStat("death",1);
        StoreState();
    }
    public void TutOff()
    {
        SteamUserStats.SetStat("tut_off",1);
        StoreState();
    }
    public void DroneBought()
    {
        SteamUserStats.SetStat("drone_bought",1);
        StoreState();
    }
    public void FuelBought()
    {
        SteamUserStats.SetStat("fuel_bought",1);
        StoreState();
    }
    public void Egg()
    {
        SteamUserStats.SetStat("easter_egg",1);
        StoreState();
    }
    public void Ruby()
    {
        SteamUserStats.SetStat("ruby",1);
        StoreState();
    }
    public void Topaz()
    {
        SteamUserStats.SetStat("topaz",1);
        StoreState();
    }
    public void Diamond()
    {
        SteamUserStats.SetStat("diamond",1);
        StoreState();
    }

    public void ClearSteamStats()
    {
        SteamUserStats.ResetAllStats(true);
        StoreState();
    }
}
