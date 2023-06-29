/// \file Framework classes for Enemy actions and implementations thereof

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

/// <summary>
/// Stores an EnemyBehaviour and a corresponding animation function
/// </summary>
/// <remarks>
/// We use this primarily for code readability in enemy implementations
/// </remarks>
public struct EnemyData {
    /// <summary>
    /// EnemyBehaviour to be performed
    /// </summary>
    public EnemyBehaviour behaviour;

    /// <summary>
    /// Function that handles Enemy animations
    /// </summary>
    public Action animate;

    /// <summary>
    /// Creates a new EnemyData
    /// </summary>
    /// <param name="inputBehaviour"> EnemyBehaviour to be stored </param>
    /// <param name="inputAnimate"> Animation function to be stored </param>
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
    /// Array storing all valid ENEMY_STATEs in which the EnemyBehaviour is intended to be used
    /// </summary>
    protected ENEMY_STATE[] _use_cases;

    /// <summary>
    /// Enemy's Rigidbody2D
    /// </summary>
    protected Rigidbody2D _enemyBody;

    /// <summary>
    /// Basic EnemyBehaviour constructor to perform operations necessary for all subclasses
    /// </summary>
    /// <param name="enemyBody"> Rigidbody2D of Enemy who will use the EnemyBehaviour </param>
    protected EnemyBehaviour(Rigidbody2D enemyBody) {
        this._enemyBody = enemyBody;
    }

    /// <summary>
    /// Checks if an ENEMY_STATE is an intended use case for the EnemyBehaviour
    /// </summary>
    /// <param name="state"> The ENEMY_STATE in which the EnemyBehaviour will be used by an Enemy. Can be set to null to skip check. </param>
    protected void _CheckUseCase(ENEMY_STATE? state) {
        if(
            state is ENEMY_STATE state_val
            && !_use_cases.Contains(state_val)
        ) {
            Debug.LogException(new InvalidEnemyStateException("INVALID ENEMY STATE ERROR: " + this.GetType() + " does not support " + state_val.ToString() + " enemy state."));
        }
    }

    /// <summary>
    /// Performs the next frame of action required for an EnemyBehaviour
    /// </summary>
    /// <param name="unscaled_acc"> Vector in direction of current Enemy acceleration </param>
    /// <returns> Whether behaviour can be interrupted on the next frame by a change of ENEMY_STATE </returns>
    public abstract bool Act(out Vector2 unscaled_acc);
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
    /// <param name="enemyBody"> Rigidbody2D of Enemy who will use the DoNothing behaviour </param>
    /// <param name="state"> ENEMY_STATE in which the DoNothing behaviour will be used. Null to skip use-case checking </param>
    public DoNothing(Rigidbody2D enemyBody, ENEMY_STATE? state) : base(enemyBody) {
        this._use_cases = new ENEMY_STATE[] { ENEMY_STATE.idle, ENEMY_STATE.aware };
        this._CheckUseCase(state);
    }

    /// <summary>
    /// Sets the Enemy's velocity to zero on the current frame
    /// </summary>
    /// <param name="unscaled_acc"> Vector in direction of current Enemy acceleration </param>
    /// <returns> true </returns>
    public override bool Act(out Vector2 unscaled_acc) {
        unscaled_acc = -this._enemyBody.velocity;
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
    [SerializeField]
    protected float _max_speed;

    /// <summary>
    /// Magnitude of enemy acceleration
    /// </summary>
    [SerializeField]
    protected float _acceleration_rate;

    /// <summary>
    /// Constructs a new ApproachTarget behaviour
    /// </summary>
    /// <param name="enemyBody"> Rigidbody2D of Enemy who will use the behaviour </param>
    /// <param name="target"> Target that the Enemy will approach (e.g. the player character) </param>
    /// <param name="max_speed"> Maximum speed at which Enemy can move </param>
    /// <param name="acceleration_rate"> Magnitude of Enemy acceleration </param>
    /// <param name="state"> ENEMY_STATE in which the behaviour will be used. Null to skip use-case checking </param>
    public ApproachTarget(Rigidbody2D enemyBody, Rigidbody2D target, float max_speed, float acceleration_rate, ENEMY_STATE? state) : base(enemyBody) {
        this._use_cases = new ENEMY_STATE[] { ENEMY_STATE.aware, ENEMY_STATE.engaged };
        this._CheckUseCase(state);

        this._target = target;
        this._max_speed = max_speed;
        this._acceleration_rate = acceleration_rate;
    }

    /// <summary>
    /// Increases Enemy's velocity in direction of _target
    /// </summary>
    /// <param name="unscaled_acc"> Vector in direction of current Enemy acceleration </param>
    /// <returns> true </returns>
    public override bool Act(out Vector2 unscaled_acc) {
        // Used to normalize updates across various frame-rates
        float time_step = Time.fixedDeltaTime;

        // Calculates change in velocity for current frame
        Vector2 delta_v = this._target.position - this._enemyBody.position;
        delta_v *= time_step * this._acceleration_rate / delta_v.magnitude;
        unscaled_acc = delta_v;

        // Adds change to enemy's velocity
        this._enemyBody.velocity = Vector2.ClampMagnitude(this._enemyBody.velocity + delta_v, this._max_speed);

        return true;
    }
}