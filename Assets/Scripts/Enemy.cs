using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySprite;
    public Rigidbody2D player;
    // Start is called before the first frame update
    public float max_speed = 4;
    public float acceleration_rate = 1;

    private void Awake(){
        myRigidbody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Used to normalize updates across various frame-rates
        float dt = Time.fixedDeltaTime;
        Vector2 dx = player.position - myRigidbody.position;

        // standard tracking of player movement
        PlayerTrack(dx, dt);
    }

    void PlayerTrack(Vector2 dx, float dt)
    {
        mySprite.flipX = dx.x <= 0;
        Vector2 acceleration = (dx / dx.magnitude) * acceleration_rate * dt;
        myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity + acceleration, max_speed);
    }
}
