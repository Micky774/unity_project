using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected CombatBehaviour myCombatBehaviour;

    protected abstract void Start();
    protected void Update(){
        myCombatBehaviour.Act();
    }
}
