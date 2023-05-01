using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float max_speed = 4;
    public float acceleration_rate = 1;
    public Rigidbody2D player;

    private Rigidbody2D _myRigidbody;
    private SpriteRenderer _mySprite;
    
    protected void Awake(){
        this._myRigidbody = this.GetComponent<Rigidbody2D>();
        this._mySprite = this.GetComponent<SpriteRenderer>();
    }
    protected void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        // Used to normalize updates across various frame-rates
        float time_step = Time.fixedDeltaTime;

        // Constructs a unit-vector along one of the eight digital directions
        Vector2 acceleration = this.player.position - this._myRigidbody.position;
        this._mySprite.flipX = acceleration.x <= 0;
        acceleration *= time_step * this.acceleration_rate / acceleration.magnitude;
        this._myRigidbody.velocity = Vector2.ClampMagnitude(this._myRigidbody.velocity + acceleration, this.max_speed);
    }
}
