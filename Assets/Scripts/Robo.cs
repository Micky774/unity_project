using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ContextEvent : UnityEvent<InputAction.CallbackContext>{}

/*
 * Player character
 */
public class Robo : MonoBehaviour
{
    public GameObject duncan;
    public PlayerInputs playerInputs;
    public Vector2 min_momentum = new Vector2(5, 5);
    public float move_strength = 40;
    public float max_speed = 12;
    public float decay_rate = 25;

    // Player health system variables
	public float invincibility_duration_seconds = 1.5f;
	public int curr_health;
	public int max_health = 5;
	public HealthBar health_bar;
    public GameOverScreen game_over;

    private InputAction _move_action;
    private InputAction _fire_action;
    private Vector2 _movement;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidbody;
    private float _projectile_spawn_dist = 3;
    private Vector3 _cursor;
    private Quaternion _orientation;

    // If true, makes player take no damage when hit. Currently used only for invincibility frames.
	private bool _invincible = false;

    private Animator _animator;
	
    // Start is called before the first frame update
    [SerializeField]
    private GameObject projectileParent;

    void Start()
    {        
        gameObject.name = "His Robotness";
        _sprite = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        // Sets player health to maximum possible on instantiation.
		curr_health = max_health;
    }
    private void Awake(){
        if (playerInputs == null){
            playerInputs = new PlayerInputs();
        }
    }

    private void OnEnable(){

        // Enables WASD movement
        _move_action = playerInputs.Player.movement;
        _move_action.Enable();

        // Enables clicking to fire Duncan
        _fire_action = playerInputs.Player.fire;
        _fire_action.performed += FireDuncan;
        _fire_action.Enable();
    }

    // Update is called once per tick, and hence is independent of framerate.
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
    void Update(){
        _cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _orientation = Utilities.GetGlobalRotation((Vector2) (_cursor - transform.position));
    }
	
	// Gives player invincibility frames
	private IEnumerator Iframes()
	{
		_invincible = true;
		
		yield return new WaitForSeconds(invincibility_duration_seconds);
		
		_invincible = false;
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
	
    // Processes damage taken by player
	public void TakeDamage(int damage){
		// Player takes no damage if currently invincible
		if(_invincible) {
			return;
		}

        // Updates health
		curr_health = Mathf.Clamp(curr_health - damage, 0, max_health);
        _animator.SetTrigger("IsDamaged");
		health_bar.UpdateHealthBar();

        // Disables player control and displays game over screen on player death
        if(curr_health == 0) {
            _move_action.Disable();
            _fire_action.performed -= FireDuncan;
            _fire_action.Disable();
            game_over.Display();
        }

        // Gives player invincibility frames after being hit
		StartCoroutine(Iframes());
	}

    public void DamageAnimation(){
        _sprite.color = Color.red;
    }
}
