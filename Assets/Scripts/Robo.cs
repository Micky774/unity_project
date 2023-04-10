using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ContextEvent : UnityEvent<InputAction.CallbackContext>{}

public class Robo : MonoBehaviour
{
    public GameObject duncan;
    public PlayerInputs playerInputs;
    public Vector2 min_momentum = new Vector2(5, 5);
    public float move_strength = 40;
    public float max_speed = 12;
    public float decay_rate = 25;

    private InputAction _move_action;
    private Vector2 _movement;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidbody;
    private float _projectile_spawn_dist = 3;
    // Start is called before the first frame update
    void Start()
    {        
        gameObject.name = "His Robotness";
        _sprite = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();


    }
    private void Awake(){
        if (playerInputs == null){
            playerInputs = new PlayerInputs();
        }
    }
    private void OnEnable(){
        _move_action = playerInputs.Player.movement;
        _move_action.Enable();

        playerInputs.Player.fire.performed += FireDuncan;
        playerInputs.Player.fire.Enable();
    }

    // Update is called once per tick, and hence is independant on framerate.
    // We primarily use this to mediate physics.
    void FixedUpdate()
    {
        // Used to normalize updates across various frame-rates
        float time_step = Time.fixedDeltaTime;

        // Constructs a unit-vector along one of the eight digital directions
        Vector2 acceleration = _move_action.ReadValue<Vector2>();
        acceleration = acceleration.normalized;
        _sprite.flipX = acceleration.x == 0 ? _sprite.flipX : acceleration.x < 0;

        // Updates speed based on move_strength, independant of decay_rate
        acceleration *= (move_strength + decay_rate) * time_step;
        
        // Calculates decay vector and adjusts acceleration accordingly
        acceleration += CalcDelayVector();

        // Scale back the speed of the adjusted velocity if needed
        _rigidbody.velocity = Vector2.ClampMagnitude(
            _rigidbody.velocity + acceleration,
            max_speed
        );
    }
    Vector2 CalcDelayVector(){
        Vector2 my_vel = _rigidbody.velocity;

        // Reduce the speed by a flat amount (constant acceleration)
        float decay_val = decay_rate * Time.fixedDeltaTime;
        float final_speed = Mathf.Max(my_vel.magnitude - decay_val, 0);
        return Vector2.ClampMagnitude(my_vel, final_speed) - my_vel;
    }
    void FireDuncan(InputAction.CallbackContext ctx){
        // Grab point of cursor at this exact time
        Vector2 target = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 displacement = target - _rigidbody.position;
        
        // Spawn projetile in the direction of the target
        GameObject created_duncan = Instantiate(
            duncan,
            _rigidbody.position + displacement.normalized * _projectile_spawn_dist,
            GetDuncanRotation(displacement)
        );

        // Set velocity based on projectiles' prescribed speed
        created_duncan.GetComponent<Rigidbody2D>().velocity = displacement * (created_duncan.GetComponent<Duncan>().speed / displacement.magnitude);
        return;
    }
    Quaternion GetDuncanRotation(Vector2 displacement){
        float angle = Mathf.Atan(displacement.x / displacement.y);
        float offset = displacement.y > 0 ? Mathf.PI : 0;
        float world_angle = (offset - angle) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, world_angle));       
    }
}
