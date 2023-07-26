using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ContextEvent : UnityEvent<InputAction.CallbackContext> { }

public class Robo : MonoBehaviour {
    public GameObject duncan;
    public PlayerInputs playerInputs;
    public Vector2 min_momentum = new Vector2(5, 5);
    public float move_strength = 40;
    public float max_speed = 12;
    public float decay_rate = 25;

    /// <summary>
    /// Seconds of invincibility frames after being hit
    /// </summary>
    [SerializeField]
    private float _invincibility_duration = 1.5f;

    /// <summary>
    /// Whether player is currently invincible
    /// </summary>
    private bool _invincible = false;

    /// <summary>
    /// Player's current health
    /// </summary>
    [SerializeField]
    private int _curr_health;

    /// <summary>
    /// Player's maximum health
    /// </summary>
    [SerializeField]
    private int _max_health = 5;

    /// <summary>
    /// Player HealthBar
    /// </summary>
    /// <remarks>
    /// Assigned in Inspector
    /// </remarks>
    [SerializeField]
    private HealthBar _healthBar;

    /// <summary>
    /// GameOverScreen displayed on player death
    /// </summary>
    /// <remarks>
    /// Assigned in Inspector
    /// </remarks>
    [SerializeField]
    private GameOverScreen _gameOver;

    /// <summary>
    /// Player movement InputAction
    /// </summary>
    private InputAction _moveAction;

    /// <summary>
    /// Projectile firing InputAction
    /// </summary>
    private InputAction _fireAction;
    private Vector2 _movement;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidbody;
    private float _projectile_spawn_dist = 3;
    private Vector3 _cursor;
    private Quaternion _orientation;

    // Start is called before the first frame update
    [SerializeField]
    private GameObject projectileParent;
    void Start() {
        gameObject.name = "His Robotness";
        _sprite = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _curr_health = _max_health;
    }
    private void Awake() {
        if(playerInputs == null) {
            playerInputs = new PlayerInputs();
        }
    }
    private void OnEnable() {
        this._moveAction = this.playerInputs.Player.movement;
        this._moveAction.Enable();

        this._fireAction = this.playerInputs.Player.fire;
        this._fireAction.performed += this.FireDuncan;
        this._fireAction.Enable();
    }

    // Update is called once per tick, and hence is independant on framerate.
    // We primarily use this to mediate physics.
    void FixedUpdate() {
        // Used to normalize updates across various frame-rates
        float time_step = Time.fixedDeltaTime;

        // Constructs a unit-vector along one of the eight digital directions
        Vector2 acceleration = _moveAction.ReadValue<Vector2>();
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
    void Update() {
        _cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _orientation = Utilities.GetGlobalRotation((Vector2)(_cursor - transform.position));
    }

    /// <summary>
    /// Gives player invincibility frames for _invincibility_duration seconds
    /// </summary>
    /// <returns> IEnumerator corresponding to Iframes coroutine </returns>
    private IEnumerator Iframes() {
        this._invincible = true;

        yield return new WaitForSeconds(this._invincibility_duration);

        this._invincible = false;
    }

    Vector2 CalcDelayVector() {
        Vector2 my_vel = _rigidbody.velocity;

        // Reduce the speed by a flat amount (constant acceleration)
        float decay_val = decay_rate * Time.fixedDeltaTime;
        float final_speed = Mathf.Max(my_vel.magnitude - decay_val, 0);
        return Vector2.ClampMagnitude(my_vel, final_speed) - my_vel;
    }
    void FireDuncan(InputAction.CallbackContext ctx) {
        // Grab point of cursor at this exact time
        Vector2 displacement = _cursor - transform.position;

        // Spawn projetile in the direction of the target
        GameObject created_duncan = Instantiate(
            duncan,
            _rigidbody.position + displacement.normalized * _projectile_spawn_dist,
            _orientation,
            projectileParent.transform
        );
        // Set velocity based on projectiles' prescribed speed
        created_duncan.GetComponent<Rigidbody2D>().velocity = displacement * (created_duncan.GetComponent<Duncan>().speed / displacement.magnitude);
        return;
    }

    /// <summary>
    /// Updates player health after an attack occurs
    /// </summary>
    /// <param name="damage"> Damage caused by attack </param>
    public void TakeDamage(int damage) {
        // Player takes no damage if currently invincible
        if(this._invincible) {
            return;
        }

        // Updates health
        this._curr_health = Mathf.Clamp(this._curr_health - damage, 0, this._max_health);
        this._curr_health = this._healthBar.UpdateHealth(this._curr_health);

        if(this._curr_health == 0) {
            this.OnDeath();
        } else {
            this.StartCoroutine(this.Iframes());
        }
    }

    /// <summary>
    /// Debug coroutine that kills the player after 10 seconds
    /// </summary>
    /// <returns> IEnumerator corresponding to GameOverDebug coroutine </returns>
    private IEnumerator GameOverDebug() {
        yield return new WaitForSeconds(10);
        this.OnDeath();
    }

    /// <summary>
    /// Handles player death
    /// </summary>
    private void OnDeath() {
        this._invincible = true;
        this._moveAction.Disable();
        this._fireAction.Disable();
        this.StartCoroutine(this._gameOver.Display());
    }
}
