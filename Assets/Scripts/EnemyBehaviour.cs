using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy state corresponding to level of awareness of player
/// </summary>
public enum ENEMY_STATE{
    /// <summary>
    /// Enemy is unaware of player
    /// </summary>
    idle = 0,
    /// <summary>
    /// Enemy is aware of player but not engaged in conflict
    /// </summary>
    aware = 1,
    /// <summary>
    /// Enemy is engaged in conflict with player
    /// </summary>
    engaged = 2
}

/// <summary>
/// Exception thrown if a behaviour is used in an unintended state
/// </summary>
public class InvalidEnemyStateException : System.Exception{
    /// <summary>
    /// Throws an exception with given error message.
    /// </summary>
    /// <param name="message"> Error message </param>
    public InvalidEnemyStateException(string message)
        : base(message)
    {
    }
}

/// <summary>
/// Abstract class providing framework for enemy actions
/// </summary>
public abstract class EnemyBehaviour{
    /// <summary>
    /// Array that stores intended use data for a behaviour
    /// </summary>
    /// <remarks>
    /// Given an ENEMY_STATE state, has the property that _use_case[(int)state] iff state is an intended state for use with current behaviour
    /// </remarks>
    protected bool[] _use_case = new bool[3];

    /// <summary>
    /// Enemy's Rigidbody2D
    /// </summary>
    protected Rigidbody2D _enemyBody;

    /// <summary>
    /// Checks if a state is an intended use case for the current behaviour
    /// </summary>
    /// <param name="state"> The state in which the behaviour will be used by an Enemy; null to skip check </param>
    /// <exception cref="InvalidEnemyStateException"> Thrown if state is an unintended use case </exception>
    protected void _CheckUseCase(ENEMY_STATE? state){
        if(state is ENEMY_STATE state_val
        && !this._use_case[(int)state_val]){
            throw new InvalidEnemyStateException(
                this._GenerateExceptionMessage(state_val));
        }
    }

    /// <summary>
    /// Produces error message to be thrown as an InvalidEnemyStateException
    /// </summary>
    /// <param name="invalid_state"> The enemy state whose use is unintended with the behaviour </param>
    /// <returns> Error message string with information about unintended state </returns>
    protected virtual string _GenerateExceptionMessage(ENEMY_STATE invalid_state){
        return "INVALID ENEMY STATE ERROR: An EnemyBehaviour in use does not support enemy state " + invalid_state.ToString();
    }

    /// <summary>
    /// Performs the next frame of action required for an EnemyBehaviour
    /// </summary>
    /// <returns> Whether behaviour can be interrupted by state change. </returns>
    public abstract bool Act();
}

/// <summary>
/// Behaviour in which Enemy does not move or perform any action
/// </summary>
/// <remarks>
/// Intended for use in idle and aware states
/// </remarks>
public class DoNothing : EnemyBehaviour{
    /// <summary>
    /// Constructs a new DoNothing behaviour
    /// </summary>
    /// <param name="enemy"> Enemy who will use the behaviour </param>
    /// <param name="state"> State in which the behaviour will be used </param>
    public DoNothing(Enemy enemy, ENEMY_STATE? state){
        this._use_case[(int)ENEMY_STATE.idle] = true;
        this._use_case[(int)ENEMY_STATE.aware] = true;

        try{this._CheckUseCase(state);}
        catch (InvalidEnemyStateException ex){
            System.Console.WriteLine(ex.Message);
        }

        this._enemyBody = enemy.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Produces error message to be thrown as an InvalidEnemyStateException
    /// </summary>
    /// <param name="invalid_state"> The enemy state whose use is unintended with the behaviour </param>
    /// <returns> Error message string with information about the combination of behaviour and unintended state </returns>
    protected override string _GenerateExceptionMessage(ENEMY_STATE invalid_state)
    {
        return "INVALID ENEMY STATE ERROR: DoNothing does not support enemy state " + invalid_state.ToString();
    }

    /// <summary>
    /// Sets the enemy's velocity to zero on the current frame
    /// </summary>
    /// <returns> true </returns>
    public override bool Act(){
        this._enemyBody.velocity = Vector2.zero;
        return true;
    }
}

/// <summary>
/// Behaviour in which enemy increases velocity in direction of a given Rigidbody2D up to a maximum speed
/// </summary>
/// <remarks>
/// Intended for use in aware and engaged states
/// </remarks>
public class ApproachTarget : EnemyBehaviour{
    protected SpriteRenderer _enemySprite;
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
    /// <param name="target"> Target enemy will approach (e.g. the player character) </param>
    /// <param name="max_speed"> Maximum speed at which enemy can move </param>
    /// <param name="acceleration_rate"> Magnitude of enemy acceleration </param>
    /// <param name="state"> State in which the behaviour will be used </param>
    public ApproachTarget(Enemy enemy, Rigidbody2D target, float max_speed, float acceleration_rate, ENEMY_STATE? state){
        this._use_case[(int)ENEMY_STATE.aware] = true;
        this._use_case[(int)ENEMY_STATE.engaged] = true;

        try{this._CheckUseCase(state);}
        catch (InvalidEnemyStateException ex){
            System.Console.WriteLine(ex.Message);
        }

        this._enemyBody = enemy.GetComponent<Rigidbody2D>();
        this._enemySprite = enemy.GetComponent<SpriteRenderer>();
        this._target = target;
        this.max_speed = max_speed;
        this.acceleration_rate = acceleration_rate;
    }

    /// <summary>
    /// Produces error message to be thrown as an InvalidEnemyStateException
    /// </summary>
    /// <param name="invalid_state"> The enemy state whose use is unintended with the behaviour </param>
    /// <returns> Error message string with information about the combination of behaviour and unintended state </returns>
    protected override string _GenerateExceptionMessage(ENEMY_STATE invalid_state)
    {
        return "INVALID ENEMY STATE ERROR: ApproachTarget does not support enemy state " + invalid_state.ToString();
    }

    /// <summary>
    /// Adds a change to enemy's velocity in direction of target for current frame
    /// </summary>
    /// <returns> true </returns>
    public override bool Act(){
        // Used to normalize updates across various frame-rates
        float time_step = Time.fixedDeltaTime;

        // Calculates change in velocity for current frame
        Vector2 delta_v = this._target.position - this._enemyBody.position;
        this._enemySprite.flipX = delta_v.x <= 0;
        delta_v *= time_step * this.acceleration_rate / delta_v.magnitude;

        // Adds change to enemy's velocity
        this._enemyBody.velocity = Vector2.ClampMagnitude(this._enemyBody.velocity + delta_v, this.max_speed);
        
        return true;
    }
}