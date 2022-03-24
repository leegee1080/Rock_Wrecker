using UnityEngine;
public class EnemyBeh_GhostTowardsPlayerThenAttack :EnemyBeh_Base, IBeh, IcollectParent
{
    [SerializeField]private GameObject _target;
    [SerializeField]private float _speed;

    private void Start()
    {
        _target = Instantiate(_target);
        _target.SetActive(false);
    }
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

        Vector2Int desired_coord = Vector2Int.zero;

        neh = Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(Levelplay_Controller_Script.levelplay_controller_singleton.current_player.grid_pos).Return_Neighbors();

        for (int i = 0; i < 4; i++)
        {
            if(neh[i].resident == null)
            {
                _target.SetActive(true);
                desired_coord = neh[i].grid_pos;
                _target.transform.position = Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(desired_coord).actual_pos;
                _parentScript.move_timer = _speed;
                _parentScript.transform.LookAt(Levelplay_Controller_Script.levelplay_controller_singleton.Find_Grid_Data(Levelplay_Controller_Script.levelplay_controller_singleton.current_player.grid_pos).actual_pos, Vector3.back);
                _parentScript.enemyAnimator.SetBool("Run", true);
                Levelplay_Controller_Script.levelplay_controller_singleton.x_lead_map_coord_array[ _parentScript.grid_pos.x][ _parentScript.grid_pos.y].resident = null;
                _parentScript.PlaceResidentTween(desired_coord, _parentScript.move_timer,false);
                _parentScript.Change_Level_Actor_State(Level_Actor_States_Enum.Moving);
                return;
            }
        }
    }
}
