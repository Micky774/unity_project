using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    /// <summary>
    /// Maximum speed at which Enemy can move
    /// </summary>
    public float max_speed = 4;
    /// <summary>
    /// Magnitude of Enemy acceleration
    /// </summary>
    public float acceleration_rate = 1;
    /// <summary>
    /// Player's Rigidbody2D
    /// </summary>
    public Rigidbody2D player;

    /// <summary>
    /// Enemy's Rigidbody2D
    /// </summary>
    private Rigidbody2D _myRigidbody;
    /// <summary>
    /// Enemy's SpriteRenderer
    /// </summary>
    private SpriteRenderer _mySprite;

    /// <summary>
    /// Sets _myRigidBody and _mySprite
    /// </summary>
    protected void Awake() {
        this._myRigidbody = this.GetComponent<Rigidbody2D>();
        this._mySprite = this.GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// Sets player
    /// </summary>
    protected void Start() {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    // TODO: Move FindGameObjectWithTag call to a script tied to Enemies parent asset so individual enemies can access the player Rigidbody2D without performing redundant searches

    /// <summary>
    /// Updates Enemy velocity for current frame to make Enemy approach player
    /// </summary>
    protected void Update() {
        // Used to normalize updates across various frame-rates
        float time_step = Time.fixedDeltaTime;

        // Constructs a unit-vector along one of the eight digital directions
        Vector2 acceleration = this.player.position - this._myRigidbody.position;
        this._mySprite.flipX = acceleration.x <= 0;
        acceleration *= time_step * this.acceleration_rate / acceleration.magnitude;
        this._myRigidbody.velocity = Vector2.ClampMagnitude(this._myRigidbody.velocity + acceleration, this.max_speed);
    }
}
