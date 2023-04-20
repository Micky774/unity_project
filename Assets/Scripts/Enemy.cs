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
    public float max_speed = 4;
    public float acceleration_rate = 1;

    // Variables for health and damage management
    public int max_health = 100;
    public int curr_health;
    public int damage = 1;

    private Animator _animator;
        
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
    }

    // Update is called once per frame
    void Update()
    {
        // Used to normalize updates across various frame-rates
        float time_step = Time.fixedDeltaTime;

        // Constructs a unit-vector along one of the eight digital directions
        Vector2 acceleration = player.position - myRigidbody.position;
        mySprite.flipX = acceleration.x <= 0;
        acceleration *= time_step * acceleration_rate / acceleration.magnitude;
        myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity + acceleration, max_speed);
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
}
