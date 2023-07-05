using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Abstract class providing framework for Enemy design
/// </summary>
/// <remarks>
/// Enemy expects that, after the end of the Start() method, _behaviours contains a valid EnemyData for each ENEMY_STATE value that _state achieves. <para/>
/// Achievable ENEMY_STATEs will vary by Enemy subclass. <para/>
/// Note that this EnemyData is mutable and can be changed during the Enemy's lifetime, but it must always have a fully functional EnemyData assigned.
/// </remarks>
public abstract class Enemy : MonoBehaviour {
    // TODO: Make custom serializable dictionary subclass
    /// <summary>
    /// Dictionary of EnemyBehaviours and animation functions to be used by Enemy in associated ENEMY_STATE
    /// </summary>
    /// <remarks>
    /// Enemy expects that, after the end of the Start() method, _behaviours contains a valid EnemyData for each ENEMY_STATE value that _state achieves. <para/>
    /// Achievable ENEMY_STATEs will vary by Enemy subclass. <para/>
    /// Note that this EnemyData is mutable and can be changed during the Enemy's lifetime, but it must always have a fully functional EnemyData assigned.
    /// </remarks>
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
    /// This is the direction in which an Enemy is currently trying to accelerate based on its EnemyBehaviours. <para/>
    /// 3. This is initialized as a zero vector on frame 1 for most enemies.
    /// </remarks>
    protected Vector2 _unscaled_acc = Vector2.zero;

    /// <summary>
    /// Initializes Enemy's behaviours and other instance variables
    /// </summary>
    /// <remarks>
    /// Enemy expects that, after the end of the Start() method, _behaviours contains a valid EnemyData for each ENEMY_STATE value that _state achieves. <para/>
    /// Achievable ENEMY_STATEs will vary by Enemy subclass. <para/>
    /// Note that this EnemyData is mutable and can be changed during the Enemy's lifetime, but it must always have a fully functional EnemyData assigned.
    /// </remarks>
    protected abstract void Start();

    /// <summary>
    /// Determines Enemy's _state and performs corresponding action for current physics tick
    /// </summary>
    /// <remarks>
    /// Enemy expects that when FixedUpdate() is called, _behaviours contains a valid EnemyData for each ENEMY_STATE value that _state achieves. <para/>
    /// Achievable ENEMY_STATEs will vary by Enemy subclass. <para/>
    /// Note that this EnemyData is mutable and can be changed during the Enemy's lifetime, but it must always have a fully functional EnemyData assigned. <para/>
    /// Enemies should be synced to the physics tickrate because they're physics objects. <para/>
    /// Virtual to allow overriding by custom non-state-based Enemies (such as bosses, potentially).
    /// </remarks>
    protected virtual void FixedUpdate() {
        if(this._can_change_state) {
            this.UpdateState();
        }
        this._can_change_state = this.Act();
    }

    /// <summary>
    /// Updates _state based on enemy-specific conditions
    /// </summary>
    protected abstract void UpdateState();

    /// <summary>
    /// Performs the EnemyBehaviour and corresponding Animation action for current physics tick based on _state
    /// </summary>
    /// <returns> Whether Enemy can change _state on the next frame </returns>
    /// <exception cref="InvalidEnemyStateException"> Thrown if Enemy does not have an EnemyBehaviour associated with _state </exception>
    /// <remarks>
    /// Enemy expects that when Act() is called, _behaviours contains a valid EnemyData for each ENEMY_STATE value that _state achieves. <para/>
    /// Achievable ENEMY_STATEs will vary by Enemy subclass. <para/>
    /// Note that this EnemyData is mutable and can be changed during the Enemy's lifetime, but it must always have a fully functional EnemyData assigned. <para/>
    /// Virtual to allow overriding by custom non-state-based enemies (such as bosses, potentially)
    /// </remarks>
    protected virtual bool Act() {
        // Note that we assume that this._behaviours contains a value corresponding to key this._state
        EnemyData data = this._behaviours[this._state];
        
        bool return_val = data.Act(out this._unscaled_acc);
        data.Animate();
        return return_val;
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
