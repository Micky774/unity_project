/// \file Framework classes for Enemy actions and implementations thereof

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Enemy's level of involvement with the player
/// </summary>
public enum ENEMY_STATE {
    /// <summary>
    /// Enemy is unaware of player
    /// </summary>
    idle = 0,
    /// <summary>
    /// Enemy is aware of player but not engaged in combat
    /// </summary>
    aware = 1,
    /// <summary>
    /// Enemy is engaged in combat with player
    /// </summary>
    engaged = 2
}

/// <summary>
/// Exception thrown in the event that an ENEMY_STATE value is incompatible with a function
/// </summary>
public class InvalidEnemyStateException : System.ArgumentException {
    /// <summary>
    /// Throws an exception with a given error message.
    /// </summary>
    /// <param name="message"> Error message </param>
    public InvalidEnemyStateException(string message) : base(message) { }
}

public struct EnemyData {
    public EnemyBehaviour behaviour;
    public Action animate;

    public EnemyData(EnemyBehaviour inputBehaviour, Action inputAnimate) {
        this.behaviour = inputBehaviour;
        this.animate = inputAnimate;
    }
}

/// <summary>
/// Abstract class providing framework for Enemy actions
/// </summary>
public abstract class EnemyBehaviour {
    /// <summary>
    /// Array storing information about intended use of the EnemyBehaviour in different ENEMY_STATEs
    /// </summary>
    /// <remarks>
    /// Given an ENEMY_STATE state, has the property that _use_case[(int)state] iff the EnemyBehaviour is designed for use with state.
    /// An EnemyBehaviour is not designed for use with any state by default, so _use_case is initialized as all false.
    /// </remarks>
    protected bool[] _use_case = new bool[3];

    /// <summary>
    /// Enemy's Rigidbody2D
    /// </summary>
    protected Rigidbody2D _enemyBody;

    /// <summary>
    /// Checks if an ENEMY_STATE is an intended use case for the EnemyBehaviour
    /// </summary>
    /// <param name="state"> The ENEMY_STATE in which the EnemyBehaviour will be used by an Enemy. Can be set to null to skip check. </param>
    /// <exception cref="InvalidEnemyStateException"> Thrown if state is not an intended use case for the EnemyBehaviour </exception>
    protected void _CheckUseCase(ENEMY_STATE? state) {
        if(
            state is ENEMY_STATE state_val
            && !this._use_case[(int)state_val]
        ) {
            throw new InvalidEnemyStateException("INVALID ENEMY STATE ERROR: " + this.GetType() + " does not support " + state_val.ToString() + " enemy state.");
        }
    }

    /// <summary>
    /// Sets intended use cases for an EnemyBehaviour
    /// </summary>
    /// <param name="states"> ENEMY_STATES in which the EnemyBehaviour is intended to be used</param>
    protected void _SetUseCase(params ENEMY_STATE[] states) {
        foreach(ENEMY_STATE state in states) {
            this._use_case[(int)state] = true;
        }
    }

    /// <summary>
    /// Performs the next frame of action required for an EnemyBehaviour
    /// </summary>
    /// <param name="acceleration_dir"> Vector in direction of current Enemy acceleration </param>
    /// <returns> Whether behaviour can be interrupted on the next frame by a change of ENEMY_STATE </returns>
    public abstract bool Act(out Vector2 acceleration_dir);
}

/// <summary>
/// EnemyBehaviour in which Enemy does not move or perform any action
/// </summary>
/// <remarks>
/// Intended for use in idle and aware ENEMY_STATEs
/// </remarks>
public class DoNothing : EnemyBehaviour {
    /// <summary>
    /// Constructs a new DoNothing behaviour
    /// </summary>
    /// <param name="enemy"> Enemy who will use the DoNothing behaviour </param>
    /// <param name="state"> ENEMY_STATE in which the DoNothing behaviour will be used. Null to skip use-case checking </param>
    public DoNothing(Enemy enemy, ENEMY_STATE? state) {
        this._SetUseCase(ENEMY_STATE.idle, ENEMY_STATE.aware);

        try {
            this._CheckUseCase(state);
        } catch(InvalidEnemyStateException ex) {
            Debug.LogException(ex);
        }

        this._enemyBody = enemy.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Sets the Enemy's velocity to zero on the current frame
    /// </summary>
    /// <param name="acceleration_dir"> Vector in direction of current Enemy acceleration </param>
    /// <returns> true </returns>
    public override bool Act(out Vector2 acceleration_dir) {
        acceleration_dir = -this._enemyBody.velocity;
        this._enemyBody.velocity = Vector2.zero;
        return true;
    }
}

/// <summary>
/// EnemyBehaviour in which Enemy increases velocity in direction of a Rigidbody2D target up to a maximum speed
/// </summary>
/// <remarks>
/// Intended for use in aware and engaged states
/// </remarks>
public class ApproachTarget : EnemyBehaviour {
    /// <summary>
    /// Target of the Enemy
    /// </summary>
    /// <remarks>
    /// Usually the player
    /// </remarks>
    protected Rigidbody2D _target;
    /// <summary>
    /// Maximum speed at which enemy can move
    /// </summary>
    public float max_speed;
    /// <summary>
    /// Magnitude of enemy acceleration
    /// </summary>
    public float acceleration_rate;

    /// <summary>
    /// Constructs a new ApproachTarget behaviour
    /// </summary>
    /// <param name="enemy"> Enemy who will use the behaviour </param>
    /// <param name="target"> Target that the Enemy will approach (e.g. the player character) </param>
    /// <param name="max_speed"> Maximum speed at which Enemy can move </param>
    /// <param name="acceleration_rate"> Magnitude of Enemy acceleration </param>
    /// <param name="state"> ENEMY_STATE in which the behaviour will be used. Null to skip use-case checking </param>
    public ApproachTarget(Enemy enemy, Rigidbody2D target, float max_speed, float acceleration_rate, ENEMY_STATE? state) {
        this._SetUseCase(ENEMY_STATE.aware, ENEMY_STATE.engaged);

        try {
            this._CheckUseCase(state);
        } catch(InvalidEnemyStateException ex) {
            Debug.LogException(ex);
        }

        this._enemyBody = enemy.GetComponent<Rigidbody2D>();
        this._target = target;
        this.max_speed = max_speed;
        this.acceleration_rate = acceleration_rate;
    }

    /// <summary>
    /// Increases Enemy's velocity in direction of _target
    /// </summary>
    /// <param name="acceleration_dir"> Vector in direction of current Enemy acceleration </param>
    /// <returns> true </returns>
    public override bool Act(out Vector2 acceleration_dir) {
        // Used to normalize updates across various frame-rates
        float time_step = Time.fixedDeltaTime;

        // Calculates change in velocity for current frame
        Vector2 delta_v = this._target.position - this._enemyBody.position;
        delta_v *= time_step * this.acceleration_rate / delta_v.magnitude;
        acceleration_dir = delta_v;

        // Adds change to enemy's velocity
        this._enemyBody.velocity = Vector2.ClampMagnitude(this._enemyBody.velocity + delta_v, this.max_speed);

        return true;
    }
}