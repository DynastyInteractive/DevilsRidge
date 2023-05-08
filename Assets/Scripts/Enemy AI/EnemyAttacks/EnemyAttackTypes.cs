using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackType {

    public virtual void Attack(int damage, GameObject[] players) 
    { 
        //do animation
        foreach(GameObject player in players)
        {
            //get health of player
            //make sure it is not null
            //do damage
        }
    }

}
