using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySprite;
    public Rigidbody2D player;
    // Start is called before the first frame update
    public TimeManager my_time;
    public float max_speed = 4;
    public float acceleration_rate = 1;
    private void Awake(){
        myRigidbody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        my_time = GameObject.FindGameObjectWithTag("Time").GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Used to normalize updates across various frame-rates
        float time_step = my_time.GetTimeStep();

        // Constructs a unit-vector along one of the eight digital directions
        Vector2 acceleration = player.position - myRigidbody.position;
        mySprite.flipX = acceleration.x <= 0;
        acceleration *= time_step * acceleration_rate / acceleration.magnitude;
        myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity + acceleration, max_speed);
    }
}
