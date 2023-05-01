using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveBehaviour{
    public abstract void Move();
}

public class FollowPlayer : MoveBehaviour{
    private Rigidbody2D _myRigidbody;
    private SpriteRenderer _mySprite;
    private Rigidbody2D _target;
    public float max_speed;
    public float acceleration_rate;

    public FollowPlayer(Enemy enemy, Rigidbody2D target, float speed = 4, float acc = 1){
        this._myRigidbody = enemy.GetComponent<Rigidbody2D>();
        this._mySprite = enemy.GetComponent<SpriteRenderer>();
        this._target = target;
        this.max_speed = speed;
        this.acceleration_rate = acc;
    }

    public override void Move()
    {
        // Used to normalize updates across various frame-rates
        float time_step = Time.fixedDeltaTime;

        // Constructs a unit-vector along one of the eight digital directions
        Vector2 delta_v = this._target.position - this._myRigidbody.position;
        this._mySprite.flipX = delta_v.x <= 0;
        delta_v *= time_step * this.acceleration_rate / delta_v.magnitude;

        this._myRigidbody.velocity = Vector2.ClampMagnitude(this._myRigidbody.velocity + delta_v, this.max_speed);
    }
}
