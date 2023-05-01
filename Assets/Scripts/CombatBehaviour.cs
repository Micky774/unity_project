using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatBehaviour
{
    public MoveBehaviour moveBehaviour;
    
    public abstract void Act();
}

public class JustMove : CombatBehaviour{
    public JustMove(MoveBehaviour move){
        this.moveBehaviour = move;
    }

    public override void Act()
    {
        moveBehaviour.Move();
    }
}