using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Basic enemy. Follows player directly.
 */
public class Enemy : MonoBehaviour
{

    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySprite;
    public Rigidbody2D player;
    public GameOverScreen game_over;
    public float max_speed = 4;
    public float acceleration_rate = 1;

    // Variables for health and damage management
    public int max_health = 100;
    public int curr_health;
    public int damage = 1;

    private Animator _animator;

    private delegate void TimeBasedAction(float time_step);

    // Allows changing of how enemy moves without changing method calls
    private TimeBasedAction Move;
        
    private void Awake(){
        myRigidbody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

    }

    // Start is called before the first frame update
    void Start()
    {
        curr_health = max_health;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        // Current initial behavior is to follow the player unrelentingly until player death
        Move = FollowPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        // Used to normalize updates across various frame-rates
        float time_step = Time.fixedDeltaTime;

        Move(time_step);
    }
    
    public void TakeDamage(int damage) {
        curr_health -= damage;

        // Performs hit animation
        _animator.SetTrigger("IsDamaged");

        if(curr_health <= 0){
            Destroy(gameObject);
        }
    }    
    
    // Runs once every frame during which a collision is occurring
    void OnCollisionStay2D(Collision2D collision) {
        ContactPoint2D contact = collision.contacts[0];
        GameObject hit_object = contact.collider.gameObject;

        if(hit_object.tag == "Player"){
            hit_object.GetComponent<Robo>().TakeDamage(damage);
        }
    }

    // Enemy moves in direction of player up to max speed
    private void FollowPlayer(float time_step) {
        // Constructs a unit-vector along one of the eight digital directions
        Vector2 acceleration = player.position - myRigidbody.position;
        mySprite.flipX = acceleration.x <= 0;
        acceleration *= time_step * acceleration_rate / acceleration.magnitude;
        myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity + acceleration, max_speed);
    }

    // Enemy stops moving entirely
    private void StandStill(float time_step) {
        myRigidbody.velocity = Vector2.zero;
    }

    // Enemy moves away from player up to 1/3 max speed
    private void WanderOffSlowly(float time_step){
        Vector2 acceleration = myRigidbody.position - player.position;
        mySprite.flipX = acceleration.x <= 0;
        acceleration *= time_step * acceleration_rate / acceleration.magnitude;
        myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity + acceleration, max_speed/3f);
    }

    // Coroutine for enemy behavior on player death
    private IEnumerator GetBored(){
        Move = StandStill;
        yield return new WaitForSeconds(GameOverScreen.seconds_til_game_over / 3f);
        Move = WanderOffSlowly;
    }

    public void OnPlayerDeath(){
        StartCoroutine(GetBored());
    }
}
