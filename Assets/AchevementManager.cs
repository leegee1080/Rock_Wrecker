using UnityEngine;
using Steamworks;

public class AchevementManager : MonoBehaviour
{
    public static AchevementManager singlton;
    [SerializeField]private bool Steam;
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
        if(!Steam){return;}
        if(!SteamManager.Initialized){return;}
        SteamUserStats.StoreStats();    
    }

    public void EscapeRock()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("escape_rock",1);
        StoreState();
    }
    public void EraseGame()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("erase_stats",1);
        StoreState();
    }
    public void Gold(int amt)
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("gold",amt);
        StoreState();
    }
    public void Shield()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("shield",1);
        StoreState();
    }
    public void Death()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("death",1);
        StoreState();
    }
    public void TutOff()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("tut_off",1);
        StoreState();
    }
    public void DroneBought()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("drone_bought",1);
        StoreState();
    }
    public void FuelBought()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("fuel_bought",1);
        StoreState();
    }
    public void Egg()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("easter_egg",1);
        StoreState();
    }
    public void Ruby()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("ruby",1);
        StoreState();
    }
    public void Topaz()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("topaz",1);
        StoreState();
    }
    public void Diamond()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("diamond",1);
        StoreState();
    }
    public void ChangeSkin()
    {
        if(!Steam){return;}
        SteamUserStats.SetStat("drone_color_change", 1);
        StoreState();
    }

    public void ClearSteamStats()
    {
        if(!Steam){return;}
        SteamUserStats.ResetAllStats(true);
        StoreState();
    }
}
