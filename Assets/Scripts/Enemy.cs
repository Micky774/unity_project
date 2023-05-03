using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Abstract class providing framework for enemy script design
 */
public abstract class Enemy : MonoBehaviour{
    // Each enemy state has a corresponding behaviour the enemy performs while in that state
    [SerializeReference]
    protected EnemyBehaviour _idleBehaviour, _awareBehaviour, _engagedBehaviour;
    // We assume by default that every enemy is initially unaware of the player
    protected ENEMY_STATE _state = ENEMY_STATE.idle;
    // By initializing this as true, we ensure an enemy can change state on its first frame
    protected bool _can_change_state = true;

    // We assume all enemies will have a Rigidbody2D for collision purposes
    protected Rigidbody2D _myRigidBody;

    // We enforce that an enemy define a Start method to initialize its enemy-specific behaviours
    protected abstract void Start();

    // We perform updates to enemies on FixedUpdate since they are physics objects
    // FixedUpdate is virtual to allow overriding by custom non-state-based enemies (such as bosses, potentially)
    protected virtual void FixedUpdate(){
        if(this._can_change_state){
            this.ChangeState();
        }
        this._can_change_state = this.PerformAction();
    }

    // We currently enforce that an enemy define a ChangeState method to determine the enemy-specific conditions under which it becomes idle, aware, and engaged.
    protected abstract void ChangeState();

    // PerformAction is virtual to allow overriding by custom non-state-based enemies (such as bosses, potentially)
    protected virtual bool PerformAction(){
        switch(this._state){
            case ENEMY_STATE.idle:
                return _idleBehaviour.Act();
            case ENEMY_STATE.aware:
                return _awareBehaviour.Act();
            case ENEMY_STATE.engaged:
                return _engagedBehaviour.Act();
            // In practice, this condition check should never be reached
            default:
                System.Console.WriteLine("WARNING: Enemy unexpectedly missing state.");
                return true;
        }
    }
}
