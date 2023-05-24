using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Abstract class providing framework for Enemy design
/// </summary>
public abstract class Enemy : MonoBehaviour {
    /// <summary>
    /// Dictionary of EnemyBehaviours and animation functions to be used by Enemy in associated ENEMY_STATE
    /// </summary>
    protected IDictionary<ENEMY_STATE, EnemyData> _behaviours = new Dictionary<ENEMY_STATE, EnemyData>();

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
    /// Enemy's SpriteRenderer
    /// </summary>
    protected SpriteRenderer _mySprite;

    /// <summary>
    /// Vector in direction of attempted Enemy acceleration
    /// </summary>
    /// <remarks>
    /// In current implementation, there are multiple important notes: <para/>
    /// 1. This is not necessarily a unit vector. <para/>
    /// 2. This is not the direction of actual enemy acceleration after collisions are factored in.
    /// This is the direction in which an Enemy is currently trying to accelerate based on its EnemyBehaviours. <para/
    /// 3. Initialized as a zero vector on frame 1 for most enemies.
    /// </remarks>
    protected Vector2 _acceleration_dir = Vector2.zero;

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
        this._can_change_state = this.PerformAction();
    }

    /// <summary>
    /// Updates _state based on enemy-specific conditions
    /// </summary>
    protected abstract void UpdateState();

    /// <summary>
    /// Performs the EnemyBehaviour for current physics tick based on _state
    /// </summary>
    /// <returns> Whether Enemy can change _state on the next frame </returns>
    /// <exception cref="InvalidEnemyStateException"> Thrown if Enemy does not have an EnemyBehaviour associated with _state </exception>
    /// <remarks>
    /// Virtual to allow overriding by custom non-state-based enemies (such as bosses, potentially)
    /// </remarks>
    protected virtual bool PerformAction() {
        if(this._behaviours.TryGetValue(this._state, out EnemyData data)) {
            bool return_val = data.behaviour.Act(out this._acceleration_dir);
            data.animate();
            return return_val;
        } else {
            throw new InvalidEnemyStateException("INVALID ENEMY STATE ERROR: " + this.GetType() + " has no behaviour defined for " + this._state + " state.");
        }
    }

    /// <summary>
    /// Handles Enemy animations after performing an idle EnemyBehaviour
    /// </summary>
    protected abstract void AfterIdleAnimate();

    /// <summary>
    /// Handles Enemy animations after performing an aware EnemyBehaviour
    /// </summary>
    protected abstract void AfterAwareAnimate();

    /// <summary>
    /// Handles Enemy Animations after performing an engaged EnemyBehaviour
    /// </summary>
    protected abstract void AfterEngagedAnimate();
}
