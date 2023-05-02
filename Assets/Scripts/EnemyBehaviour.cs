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

public class InvalidEnemyStateException : System.Exception{
    public InvalidEnemyStateException(string message)
        : base(message)
    {
    }
}

/// <summary>
/// Abstract class providing framework for enemy actions
/// </summary>
public abstract class EnemyBehaviour{
    // Array with the property that use_case[ENEMY_STATE.state] <=> behaviour designed to be used in ENEMY_STATE.state
    protected bool[] _use_case = new bool[3];

    protected void _CheckUseCase(ENEMY_STATE? state){
        if(state is ENEMY_STATE state_val
        && !this._use_case[(int)state_val]){
            throw new InvalidEnemyStateException(
                this._GenerateExceptionMessage(state_val));
        }
    }

    protected virtual string _GenerateExceptionMessage(ENEMY_STATE invalid_state){
        return "INVALID ENEMY STATE ERROR: An EnemyBehaviour in use does not support enemy state " + invalid_state.ToString();
    }

    /// <summary>
    /// Performs the next frame of action required for a behaviour
    /// </summary>
    /// <returns> Whether behaviour can be interrupted by state change. </returns>
    public abstract bool Act();
}

public class DoNothing : EnemyBehaviour{
    private Rigidbody2D _enemyBody;
    public DoNothing(Enemy enemy, ENEMY_STATE? state){
        // DoNothing supports idle and aware state by default
        this._use_case[(int)ENEMY_STATE.idle] = true;
        this._use_case[(int)ENEMY_STATE.aware] = true;

        // Checks if developer is using DoNothing in unintended EnemyState
        try{this._CheckUseCase(state)}
        catch(InvalidEnemyStateException ex){
            System.Console.WriteLine(ex.Message);
        }

        _enemyBody = enemy.GetComponent<Rigidbody2D>();
    }

    protected override string _GenerateExceptionMessage(ENEMY_STATE invalid_state)
    {
        return "INVALID ENEMY STATE ERROR: DoNothing does not support enemy state " + invalid_state.ToString();
    }

    public override bool Act(){
        _enemyBody.velocity = Vector2.zero;
        return true;
    }
}
