using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Enemy
{
    private const float _MAX_SPEED = 4;
    private const float _ACCELERATION_RATE = 1;
    private Rigidbody2D _myRigidBody, _player;

    protected override void Start(){
        _myRigidBody = this.GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        
        this._idleBehaviour = new DoNothing(this, ENEMY_STATE.idle);
        this._awareBehaviour = new ApproachTarget(this, _player, _MAX_SPEED, _ACCELERATION_RATE, ENEMY_STATE.aware);
        this._engagedBehaviour = new ApproachTarget(this, _player, _MAX_SPEED, _ACCELERATION_RATE, ENEMY_STATE.engaged);
    }

    protected override void ChangeState()
    {
        float dist_to_player = (_myRigidBody.position - _player.position).magnitude;
        if(dist_to_player < 50){
            this._state = ENEMY_STATE.engaged;
        }
        else if (dist_to_player < 100){
            this._state = ENEMY_STATE.aware;
        }
        else{
            this._state = ENEMY_STATE.idle;
        }

        Debug.Log("My distance to the player is " + dist_to_player + " so my current enemy state is " + this._state);
    }
}
