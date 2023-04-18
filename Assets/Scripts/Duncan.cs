using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Basic projectile. Fires once per click in direction of mouse cursor.
 */
public class Duncan : MonoBehaviour
{
    public float speed = 4;

    // Damage done by Duncan to enemy on hit
	public int damage = 20;

    // Runs on the first frame on which a collision occurs
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Gets the object hit by Duncan
        ContactPoint2D contact = collision.contacts[0];
		GameObject hit_object = contact.collider.gameObject;

        // Destroys Duncan if he hits a wall
        if(hit_object.tag == "Wall"){
            Destroy(gameObject);
        }
		else if(hit_object.tag == "Enemy"){
            // Damages enemy hit by Duncan and destroys Duncan
			hit_object.GetComponent<Enemy>().TakeDamage(damage);
			Destroy(gameObject);
		}
    }
}
