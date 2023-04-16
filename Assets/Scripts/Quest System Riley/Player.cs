using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 

{
    public int health = 5;
    public int experience = 40;
    public int gold = 1000; 

    public void GoBattle ()
    {
        health -= 1;
        experience += 2; 
        gold += 5;
    }
}
