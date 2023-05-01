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

    private InputAction _moveAction;
    private Vector2 _movement;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidbody;
    private float _projectile_spawn_dist = 3;
    private Vector3 _cursor;
    private Quaternion _orientation;

    [SerializeField]
    private GameObject _projectileParent;

    protected void Start()
    {        
        this.gameObject.name = "His Robotness";
        this._sprite = this.GetComponent<SpriteRenderer>();
        this._rigidbody = this.GetComponent<Rigidbody2D>();
    }
    protected void Awake(){
        if (this.playerInputs == null){
            this.playerInputs = new PlayerInputs();
        }
    }
    protected void OnEnable(){
        this._moveAction = this.playerInputs.Player.movement;
        this._moveAction.Enable();

        this.playerInputs.Player.fire.performed += this._FireDuncan;
        this.playerInputs.Player.fire.Enable();
    }

    // Update is called once per tick, and hence is independent of framerate.
    // We primarily use this to mediate physics.
    protected void FixedUpdate()
    {
        // Used to normalize updates across various frame-rates
        float time_step = Time.fixedDeltaTime;

        // Constructs a unit-vector along one of the eight digital directions
        Vector2 acceleration = this._moveAction.ReadValue<Vector2>();
        acceleration = acceleration.normalized;
        this._sprite.flipX = acceleration.x == 0 ? this._sprite.flipX : acceleration.x < 0;

        // Updates speed based on move_strength, independent of decay_rate
        acceleration *= (this.move_strength + this.decay_rate) * time_step;
        
        // Calculates decay vector and adjusts acceleration accordingly
        acceleration += this._CalcDelayVector();

        // Scale back the speed of the adjusted velocity if needed
        this._rigidbody.velocity = Vector2.ClampMagnitude(
            this._rigidbody.velocity + acceleration,
            this.max_speed
        );
    }
    protected void Update(){
        this._cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this._orientation = Utilities.GetGlobalRotation((Vector2) (this._cursor - this.transform.position));
    }
    
    private Vector2 _CalcDelayVector(){
        Vector2 my_vel = this._rigidbody.velocity;

        // Reduce the speed by a flat amount (constant acceleration)
        float decay_val = this.decay_rate * Time.fixedDeltaTime;
        float final_speed = Mathf.Max(my_vel.magnitude - decay_val, 0);
        return Vector2.ClampMagnitude(my_vel, final_speed) - my_vel;
    }
    
    private void _FireDuncan(InputAction.CallbackContext ctx){
        // Grab point of cursor at this exact time
        Vector2 displacement = this._cursor - this.transform.position;
        
        // Spawn projectile in the direction of the target
        GameObject created_duncan = Object.Instantiate(
            this.duncan,
            this._rigidbody.position + displacement.normalized * this._projectile_spawn_dist,
            this._orientation,
            this._projectileParent.transform
        );
        // Set velocity based on projectiles' prescribed speed
        created_duncan.GetComponent<Duncan>().Init(displacement);
        return;
    }
}
