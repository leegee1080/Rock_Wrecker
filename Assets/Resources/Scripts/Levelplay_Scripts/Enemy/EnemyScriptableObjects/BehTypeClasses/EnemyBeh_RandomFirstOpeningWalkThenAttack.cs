using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EnemyBeh_RandomFirstOpeningWalkThenAttack :EnemyBeh_Base, IBeh, IcollectParent
{
    [SerializeField]private float _speedAddition;
    public override void ProcessEnemyTurn()
    {
        base.ProcessEnemyTurn();

        Grid_Data[] neh = Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(_parentScript.grid_pos).Return_Neighbors();

        for (int i = 0; i < 4; i++)
        {
            if(neh[i].resident != null && neh[i].resident.GetComponent<LevelPlayer_Script>())
            {
                _parentScript.Move(i);
                return;
            }
        }
        
        IEnumerable<int> possibleMoveLocations = 
            from cell in neh
            where cell.resident == null
            select Array.IndexOf(neh, cell);

        if(possibleMoveLocations.Count() <= 0){return;}

        int randIndex = Global_Vars.rand_num_gen.Next(0,possibleMoveLocations.Count());
        _parentScript.Move(possibleMoveLocations.ElementAt(randIndex),_speedAddition);
    }
}
