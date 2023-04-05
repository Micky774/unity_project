using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robo : MonoBehaviour
{
    public GameObject duncan;
    public SpriteRenderer mySprite;
    public Rigidbody2D myRigidbody;
    public TimeManager my_time;
    public Vector2 min_momentum = new Vector2(5, 5);
    public float move_strength = 40;
    public float max_speed = 12;
    public float decay_rate = 25;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "His Robotness";
        my_time = GameObject.FindGameObjectWithTag("Time").GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Used to normalize updates across various frame-rates
        float time_step = my_time.GetTimeStep();

        // Constructs a unit-vector along one of the eight digital directions
        Vector2 acceleration = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        acceleration = acceleration.normalized;
        mySprite.flipX = acceleration.x == 0 ? mySprite.flipX : acceleration.x < 0;

        // Updates speed based on move_strength, independant of decay_rate
        acceleration *= (move_strength + decay_rate) * time_step;
        
        // Calculates decay vector and adjusts acceleration accordingly
        acceleration += CalcDelayVector();

        // Scale back the speed of the adjusted velocity if needed
        myRigidbody.velocity = Vector2.ClampMagnitude(
            myRigidbody.velocity + acceleration,
            max_speed
        );
        if(Input.GetMouseButtonDown(0)){
            FireDuncan((Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
    Vector2 CalcDelayVector(){
        Vector2 my_vel = myRigidbody.velocity;
        float decay_val = decay_rate * my_time.GetTimeStep();
        float final_speed = Mathf.Max(my_vel.magnitude - decay_val, 0);
        return Vector2.ClampMagnitude(my_vel, final_speed) - my_vel;
    }
    void FireDuncan(Vector2 target){
        Vector2 displacement = target - myRigidbody.position;
        GameObject created_duncan = Instantiate(duncan, myRigidbody.position, GetDuncanRotation(displacement));
        created_duncan.GetComponent<Rigidbody2D>().velocity = displacement * (created_duncan.GetComponent<Duncan>().speed / displacement.magnitude);
        return;
    }

    Quaternion GetDuncanRotation(Vector2 displacement){
        float angle = Mathf.Atan(displacement.y / displacement.x);
        float offset = displacement.x > 0 ? 0 : 1;
        return Quaternion.Euler(new Vector3(0, 0, 180 * offset + 90 + angle * 180 / Mathf.PI));       
    }
}
