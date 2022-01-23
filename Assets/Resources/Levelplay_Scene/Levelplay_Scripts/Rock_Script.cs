using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Script : GridResident_Script, IInteractable_GridResident
{
    public void Tapped(){
        //swap with player
        
        // if(Levelplay_Controller_Script.levelplay_controller_singleton.current_player.Swap_With_Rock(this)){
        //     Debug.Log("Swapped: " + this.name);
        // }else{
        //     Debug.Log("Swap failed: " + this.name);
        // }

    }
    
}
