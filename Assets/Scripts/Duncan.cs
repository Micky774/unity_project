using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duncan : MonoBehaviour
{
    public float speed = 4;
	public float damage = 20;
    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
		GameObject hit_object = contact.collider.gameObject;
        if(hit_object.tag == "Wall"){
            Destroy(gameObject);
        }
		else if(hit_object.tag == "Enemy"){
			hit_object.GetComponent<Enemy>().TakeDamage(damage);
			Destroy(gameObject);
		}
    }
}
