using UnityEngine;
public class EnemyBeh_TowardsPlayerThenAttack :EnemyBeh_Base, IBeh, IcollectParent
{
    [SerializeField]private float _speedAddition;
    public override void ProcessEnemyTurn()
    {
        base.ProcessEnemyTurn();

        Grid_Data[] neh = Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(_parentScript.grid_pos).Return_Neighbors();

        for (int i = 0; i < 4; i++)
        {
            if(neh[i].resident != null && neh[i].resident.tag == "Player")
            {
                _parentScript.Move(i);
                return;
            }
        }

        Vector2 directionDif =  Levelplay_Controller_Script.levelplay_controller_singleton.current_player.grid_pos - _parentScript.grid_pos;
        Vector2Int direction = new Vector2Int((int)Mathf.Round(directionDif.normalized.x), (int)Mathf.Round(directionDif.normalized.y));
        if(Mathf.Abs(direction.x) == Mathf.Abs(direction.y))
        {
            if(Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(_parentScript.grid_pos + new Vector2Int(direction.x, 0)).resident != null)
            {
                direction = new Vector2Int(0, direction.y);
            }
            else
            {
                direction = new Vector2Int(direction.x, 0);
            }
        }

        if(Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(_parentScript.grid_pos + direction).resident != null)
        {
            return;
        }


        _parentScript.Move((int)_parentScript.NormalVectorToDirection(direction), _speedAddition);
    }
}
