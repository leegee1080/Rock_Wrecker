using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Crystal Prize", menuName = "Scriptable Objects/New Crystal Prize")]
public class CrystalPrize_ScriptableObject : ScriptableObject
{
    [Header("Name")]
    public new string name;
    public string desc;
    public Sprite art;
    public string sound;
    public GameObject particle;

    
    [Header("Amount")]
    public int tier;
    public int amount;

    [Header("Function")]
    public PlayerUpgradeTypes UpgradeType;
    public Action<int,PlayerData> StatChangeFunc;

    public void RunStatChange()
    {
        StatChangeFunc = Global_Vars.PlayerUpgradeFuncDict[UpgradeType];
        StatChangeFunc(amount, Overallgame_Controller_Script.overallgame_controller_singleton.CurrentPlayer);
    }
}
