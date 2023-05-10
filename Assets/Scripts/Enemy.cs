using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class providing framework for Enemy design
/// </summary>
public abstract class Enemy : MonoBehaviour {
    /// <summary>
    /// EnemyBehaviour performed when Enemy is in idle ENEMY_STATE
    /// </summary>
    [SerializeReference]
    protected EnemyBehaviour _idleBehaviour;
    /// <summary>
    /// EnemyBehaviour performed when Enemy is in aware ENEMY_STATE
    /// </summary>
    [SerializeReference]
    protected EnemyBehaviour _awareBehaviour;
    /// <summary>
    /// EnemyBehaviour performed when Enemy is in engaged ENEMY_STATE
    /// </summary>
    [SerializeReference]
    protected EnemyBehaviour _engagedBehaviour;

    /// <summary>
    /// Enemy's current level of involvement with the player
    /// </summary>
    /// <remarks>
    /// Initialized as idle for most enemies.
    /// This reflects an assertion that most enemies should be unaware of the player when first instantiated.
    /// </remarks>
    protected ENEMY_STATE _state = ENEMY_STATE.idle;

    /// <summary>
    /// Whether the Enemy is capable of interrupting its current EnemyBehaviour and changing the value of _state
    /// </summary>
    /// <remarks>
    /// Initialized as true for most enemies.
    /// This reflects an assertion that most enemies should be able to determine their level of awareness of the player on their first active frame.
    /// </remarks>
    protected bool _can_change_state = true;

    /// <summary>
    /// Enemy's Rigidbody2D
    /// </summary>
    protected Rigidbody2D _myRigidbody;

    /// <summary>
    /// Initializes Enemy's behaviours and other instance variables
    /// </summary>
    /// <remarks>
    /// Runs on the first frame a script is enabled before Update is called
    /// </remarks>
    protected abstract void Start();

    /// <summary>
    /// Determines Enemy's _state and performs corresponding action for current physics tick
    /// </summary>
    /// <remarks>
    /// Enemies should be synced to the physics tickrate because they're physics objects.
    /// Virtual to allow overriding by custom non-state-based Enemies (such as bosses, potentially).
    /// </remarks>
    protected virtual void FixedUpdate() {
        if(this._can_change_state) {
            this.UpdateState();
        }
        try {
            this._can_change_state = this.PerformAction();
        } catch(InvalidEnemyStateException ex) {
            Debug.LogException(ex);
            this._state = ENEMY_STATE.idle;
            this._can_change_state = true;
        }
    }

    /// <summary>
    /// Updates _state based on enemy-specific conditions
    /// </summary>
    protected abstract void UpdateState();

    /// <summary>
    /// Performs the EnemyBehaviour for current physics tick based on _state
    /// </summary>
    /// <returns> Whether Enemy can change _state on the next frame </returns>
    /// <exception cref="InvalidEnemyStateException"> Thrown if _state is not a valid ENEMY_STATE value </exception>
    /// <remarks>
    /// Virtual to allow overriding by custom non-state-based enemies (such as bosses, potentially)
    /// </remarks>
    protected virtual bool PerformAction() {
        switch(this._state) {
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
