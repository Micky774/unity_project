using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingBoi : MonoBehaviour
{

    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySprite;
    public Rigidbody2D player;
    // Start is called before the first frame update
    public float max_speed = 4;
    public float acceleration_rate = 1;
    public float pause_distance = 10f;
    public float pause_distance_tol = 2f;
    public float pause_time = 4;
    public float charge_speed = 8;
    private float charge_timer = 0;
    private bool charge_state = false;
    private Vector2 charge_dir = Vector2.zero;
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

        if (charge_state)
        {
            Charge(charge_dir);
        }
        else
        {
            // standard tracking of player movement
            PlayerTrack(dx, dt);

            // check to see if the charge state should be entered
            if ((dx.magnitude > (pause_distance - pause_distance_tol)) & (dx.magnitude < (pause_distance + pause_distance_tol)))
            {
                myRigidbody.velocity = 0 * myRigidbody.velocity;
                charge_state = true;
                charge_dir = dx;
            }
        }
    }
    void Charge(Vector2 charge_dir)
    {
        charge_timer += Time.deltaTime;

        if (charge_timer > pause_time)
            myRigidbody.velocity = (charge_dir / charge_dir.magnitude) * charge_speed;
    }

    void PlayerTrack(Vector2 dx, float dt)
    {
        mySprite.flipX = dx.x <= 0;
        Vector2 acceleration = (dx / dx.magnitude) * acceleration_rate * dt;
        myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity + acceleration, max_speed);
    }
}
