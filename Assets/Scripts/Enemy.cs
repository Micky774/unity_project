using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySprite;
    public Rigidbody2D player;
    // Start is called before the first frame update
    public float max_productive_speed = 4f;
    public float max_jitter_speed = 1f;
    public float acceleration_rate = 1;

    public float jitter_rate = 0.3f;
    // public float angle_tol = 90f*Mathf.Deg2Rad;

    // initialize productive velocity and jittery velocity
    private Vector2 productive_velocity = new Vector2(0, 0);
    private Vector2 jitter_velocity = new Vector2(0, 0);
    
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
        float time_step = Time.fixedDeltaTime;

        // Calculate the productive and jitter velocity changes
        Vector2 productive_velocity_change = player.position - myRigidbody.position;
        productive_velocity_change *= time_step * acceleration_rate / productive_velocity_change.magnitude;

        // calculate angle of travel
        // float productive_angle = Mathf.Atan2(productive_velocity.x, productive_velocity.y);
        // Vector2 jitter_velocity_change = GetConstrainedPointOnCircle(productive_angle, jitter_rate);
        Vector2 jitter_velocity_change = GetPointOnCircle(jitter_rate);

        // Update the productive and jitter velocity changes
        productive_velocity = Vector2.ClampMagnitude(productive_velocity + productive_velocity_change, max_productive_speed);
        jitter_velocity = Vector2.ClampMagnitude(jitter_velocity + jitter_velocity_change, max_jitter_speed);

        // update rigid body velocity
        myRigidbody.velocity = productive_velocity + jitter_velocity;

        // flip sprite if necessary
        mySprite.flipX = productive_velocity_change.x <= 0;
    }

    // Vector2 GetConstrainedPointOnCircle(float productive_angle, float radius = 1) {
    //     // float random_angle = Random.Range(productive_angle - angle_tol, productive_angle + angle_tol);
    //     float random_angle = Random.Range(0f, 2 * Mathf.PI - Mathf.Epsilon);
    //     // Debug.Log(random_angle);
    //     Vector2 point_on_circle = new Vector2(Mathf.Cos(random_angle), Mathf.Sin(random_angle)) * radius;
    //     return point_on_circle;
    // }   
    Vector2 GetPointOnCircle(float radius = 1) {
        float random_angle = Random.Range(0f, 2 * Mathf.PI - Mathf.Epsilon);
        Vector2 point_on_circle = new Vector2(Mathf.Cos(random_angle), Mathf.Sin(random_angle)) * radius;
        return point_on_circle;
    }   
}
