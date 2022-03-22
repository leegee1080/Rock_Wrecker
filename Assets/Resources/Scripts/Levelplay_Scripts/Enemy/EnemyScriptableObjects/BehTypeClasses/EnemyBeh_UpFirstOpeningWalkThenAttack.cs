using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeh_UpFirstOpeningWalkThenAttack :EnemyBeh_Base, IBeh, IcollectParent
{
    public override void ProcessEnemyTurn()
    {
        base.ProcessEnemyTurn();

        Grid_Data[] neh = Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(_parentScript.grid_pos).Return_Neighbors();
        for (int i = 0; i < 4; i++)
        {
            if(neh[i].resident == null)
            {
                _parentScript.Move(i);
                return;
            }
        }
        return;
    }
}
