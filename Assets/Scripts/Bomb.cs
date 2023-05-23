using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic enemy that simply approaches player when player enters its awareness radius.
/// </summary>
/// <remarks>
/// Placeholder enemy whose graphics are stolen from Final Fantasy VII Remake
/// </remarks>
public class Bomb : Enemy {
    /// <summary>
    /// Initial value for maximum speed at which Bomb can approach the player
    /// </summary>
    /// <remarks>
    /// If you wish to tweak Bomb's max speed in the Inspector at runtime, alter max_speed variable from _awareBehaviour and _engagedBehaviour instead.
    /// </remarks>
    private const float _MAX_SPEED = 4;
    /// <summary>
    /// Initial value for magnitude of acceleration with which Bomb can approach the player
    /// </summary>
    /// <remarks>
    /// If you wish to tweak Bomb's acceleration rate in the Inspector at runtime, alter acceleration_rate variable from _awareBehaviour and _engagedBehaviour instead.
    /// </remarks>
    private const float _ACCELERATION_RATE = 1;

    private const int _MAX_HEALTH = 100;

    private const int _DAMAGE = 1;

    /// <summary>
    /// Player's Rigidbody2D
    /// </summary>
    private Rigidbody2D _player;

    /// <summary>
    /// Sets instance variables such that Bomb does nothing when idle and approaches player otherwise
    /// </summary>
    protected override void Start() {
        this._myRigidbody = this.GetComponent<Rigidbody2D>();
        this._mySprite = this.GetComponent<SpriteRenderer>();
        this._curr_health = this._max_health = _MAX_HEALTH;
        this._player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        this._behaviours.Add(ENEMY_STATE.idle, (new DoNothing(this, ENEMY_STATE.idle), AfterIdleAnimate));
        this._behaviours.Add(ENEMY_STATE.aware, (new ApproachTarget(this, this._player, Bomb._MAX_SPEED, Bomb._ACCELERATION_RATE, ENEMY_STATE.aware), AfterAwareAnimate));
        this._behaviours.Add(ENEMY_STATE.engaged, (new ApproachTarget(this, this._player, Bomb._MAX_SPEED, Bomb._ACCELERATION_RATE, ENEMY_STATE.engaged), AfterEngagedAnimate));
    }

    /// <summary>
    /// Sets Bomb's _state based on its distance from the player
    /// </summary>
    /// <remarks>
    /// Bomb is idle when at least 100 units from player, aware when at least 50 but less than 100 units from player, and engaged when less than 50 units from player.
    /// </remarks>
    protected override void UpdateState() {
        float dist_to_player = (this._myRigidbody.position - this._player.position).magnitude;
        ENEMY_STATE state = ENEMY_STATE.idle;
        if(dist_to_player < 50) {
            state = ENEMY_STATE.engaged;
        } else if(dist_to_player < 100) {
            state = ENEMY_STATE.aware;
        }
        this._state = state;

#if ENEMY_DEBUG
        Debug.Log("My distance to the player is " + dist_to_player + " so my current enemy state is " + this._state);
#endif
    }

    /// <summary>
    /// Does literally nothing
    /// </summary>
    /// <remarks>
    /// Bombs don't move when idle and have no animation updates to be performed.
    /// </remarks>
    protected override void AfterIdleAnimate() {
    }

    /// <summary>
    /// Flips Bomb's sprite to match horizontal acceleration
    /// </summary>
    protected override void AfterAwareAnimate() {
        this._mySprite.flipX = this._acceleration_dir.x <= 0;
    }

    /// <summary>
    /// Flips Bomb's sprite to match horizontal acceleration
    /// </summary>
    protected override void AfterEngagedAnimate() {
        this._mySprite.flipX = this._acceleration_dir.x <= 0;
    }

    protected void OnCollisionStay2D(Collision2D collision) {
        ContactPoint2D contact = collision.contacts[0];
        GameObject hit_object = contact.collider.gameObject;

        if(hit_object.tag == "Player"){
            hit_object.GetComponent<Robo>().TakeDamage(_DAMAGE);
        }
    }
}