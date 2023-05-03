using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Abstract class providing framework for enemy script design
 */
public abstract class Enemy : MonoBehaviour{

    protected EnemyBehaviour _idleBehaviour, _awareBehaviour, _engagedBehaviour;
    protected ENEMY_STATE _state = ENEMY_STATE.idle;
    protected bool _can_change_state = true;
    protected abstract void Start();
    protected virtual void FixedUpdate(){
        if(this._can_change_state){
            this.ChangeState();
        }
        this._can_change_state = this.PerformAction();
    }
    protected abstract void ChangeState();
    protected virtual bool PerformAction(){
        switch(this._state){
            case ENEMY_STATE.idle:
                return _idleBehaviour.Act();
            case ENEMY_STATE.aware:
                return _awareBehaviour.Act();
            case ENEMY_STATE.engaged:
                return _engagedBehaviour.Act();
            default:
                return true;
        }
    }
}
