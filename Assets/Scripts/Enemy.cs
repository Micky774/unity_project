using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Abstract class providing framework for enemy script design
 */
public abstract class Enemy : MonoBehaviour
{
    // Each enemy state has a corresponding behaviour the enemy performs while in that state
    /* 
     * TODO: Research if there's a more computationally efficient way to allow editing of EnemyBehaviour members in Inspector than serializing the EnemyBehaviours by reference.
     * Example possibilities include defining a custom Editor subclass to add EnemyBehaviour members to Inspector
     * As of 5/6/2023, the project is small enough that the overhead of [SerializeReference] is negligible. This is impermanent and experimentation will be required.
     */
    [SerializeReference]
    protected EnemyBehaviour _idleBehaviour, _awareBehaviour, _engagedBehaviour;
    // We assume by default that every enemy is initially unaware of the player
    protected ENEMY_STATE _state = ENEMY_STATE.idle;
    // By initializing this as true, we ensure an enemy can change state on its first frame
    protected bool _can_change_state = true;

    // We assume all enemies will have a Rigidbody2D for collision purposes
    protected Rigidbody2D _myRigidbody;

    // We enforce that an enemy define a Start method to initialize its enemy-specific behaviours
    protected abstract void Start();

    // We perform updates to enemies on FixedUpdate since they are physics objects
    // FixedUpdate is virtual to allow overriding by custom non-state-based enemies (such as bosses, potentially)
    protected virtual void FixedUpdate()
    {
        if (this._can_change_state)
        {
            this.ChangeState();
        }
        try
        {
            this._can_change_state = this.PerformAction();
        }
        catch (InvalidEnemyStateException ex)
        {
            Debug.LogException(ex);
            this._state = ENEMY_STATE.idle;
            this._can_change_state = true;
        }
    }

    // We currently enforce that an enemy define a ChangeState method to determine the enemy-specific conditions under which it becomes idle, aware, and engaged.
    protected abstract void ChangeState();

    // PerformAction is virtual to allow overriding by custom non-state-based enemies (such as bosses, potentially)
    protected virtual bool PerformAction()
    {
        switch (this._state)
        {
            case ENEMY_STATE.idle:
                return this._idleBehaviour.Act();
            case ENEMY_STATE.aware:
                return this._awareBehaviour.Act();
            case ENEMY_STATE.engaged:
                return this._engagedBehaviour.Act();
            default:
                throw new InvalidEnemyStateException("INVALID ENEMY STATE ERROR: " + this.GetType() + " has a non-standard enemy state while using base PerformAction function.");
        }
    }
}
