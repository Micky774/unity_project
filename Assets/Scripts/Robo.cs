using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ContextEvent : UnityEvent<InputAction.CallbackContext> { }

/// <summary>
/// Player character
/// </summary>
public class Robo : MonoBehaviour {
    /// <summary>
    /// Duncan prefab
    /// </summary>
    public GameObject duncan;
    /// <summary>
    /// Player controls handler
    /// </summary>
    public PlayerInputs playerInputs;
    /// <summary>
    /// Unused momentum vector
    /// </summary>
    public Vector2 min_momentum = new Vector2(5, 5);
    /// <summary>
    /// Magnitude of Robo acceleration based on player inputs
    /// </summary>
    public float move_strength = 40;
    /// <summary>
    /// Maximum speed at which Robo can move
    /// </summary>
    public float max_speed = 12;
    /// <summary>
    /// Magnitude of automatic deceleration
    /// </summary>
    /// <remarks>
    /// The higher this value, the more Robo slows down when the player isn't providing input.
    /// </remarks>
    public float decay_rate = 25;

    /// <summary>
    /// InputAction corresponding to player movement controls
    /// </summary>
    private InputAction _moveAction;
    /// <summary>
    /// Unused movement vector
    /// </summary>
    private Vector2 _movement;
    /// <summary>
    /// Robo SpriteRenderer
    /// </summary>
    private SpriteRenderer _sprite;
    /// <summary>
    /// Robo Rigidbody2D
    /// </summary>
    private Rigidbody2D _rigidbody;
    /// <summary>
    /// Distance from Robo that projectiles should be spawned on creation
    /// </summary>
    private float _projectile_spawn_dist = 3;
    /// <summary>
    /// In-scene position vector corresponding to location of mouse cursor on screen
    /// </summary>
    private Vector3 _cursor;
    /// <summary>
    /// Rotation a newly instantiated projectile should have based on _cursor and Robo's position
    /// </summary>
    private Quaternion _orientation;

    /// <summary>
    /// Parent object under which projectiles are created
    /// </summary>
    [SerializeField]
    private GameObject _projectileParent;

    /// <summary>
    /// Sets Robo's name, _sprite, and _rigidbody
    /// </summary>
    protected void Start() {
        this.gameObject.name = "His Robotness";
        this._sprite = this.GetComponent<SpriteRenderer>();
        this._rigidbody = this.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Initializes playerInputs if not provided in Inspector
    /// </summary>
    protected void Awake() {
        if(this.playerInputs == null) {
            this.playerInputs = new PlayerInputs();
        }
    }

    /// <summary>
    /// Enables player movement and fire controls
    /// </summary>
    protected void OnEnable() {
        this._moveAction = this.playerInputs.Player.movement;
        this._moveAction.Enable();

        this.playerInputs.Player.fire.performed += this._FireDuncan;
        this.playerInputs.Player.fire.Enable();
    }

    // Update is called once per tick, and hence is independent of framerate.
    // We primarily use this to mediate physics.
    /// <summary>
    /// Updates Robo's velocity based on player input for current physics tick
    /// </summary>
    protected void FixedUpdate() {
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

    /// <summary>
    /// Updates last known cursor position and corresponding rotation of newly instantiated projectiles each frame
    /// </summary>
    protected void Update() {
        this._cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this._orientation = Utilities.GetGlobalRotation((Vector2)(this._cursor - this.transform.position));
    }

    /// <summary>
    /// Constructs a vector in the direction of Robo's velocity with magnitude reduced based on decay_val
    /// </summary>
    /// <returns> New velocity vector for Robo to use </returns>
    private Vector2 _CalcDelayVector() {
        Vector2 my_vel = this._rigidbody.velocity;

        // Reduce the speed by a flat amount (constant acceleration)
        float decay_val = this.decay_rate * Time.fixedDeltaTime;
        float final_speed = Mathf.Max(my_vel.magnitude - decay_val, 0);
        return Vector2.ClampMagnitude(my_vel, final_speed) - my_vel;
    }

    /// <summary>
    /// Fires a new Duncan based on player input
    /// </summary>
    /// <param name="ctx"> Information about player input that triggered method call </param>
    private void _FireDuncan(InputAction.CallbackContext ctx) {
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
