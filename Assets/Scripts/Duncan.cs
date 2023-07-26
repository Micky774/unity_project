using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duncan : MonoBehaviour {
    public float speed = 4;

    /// <summary>
    /// Damage Duncan does when he hits an Enemy
    /// </summary>
    [SerializeField]
    private int damage = 20;

    // Runs on the first frame on which a collision occurs
    void OnCollisionEnter2D(Collision2D collision) {
        ContactPoint2D contact = collision.contacts[0];
        GameObject hit_object = contact.collider.gameObject;

        if(hit_object.tag == "Wall") {
            Destroy(gameObject);
        } else if(hit_object.tag == "Enemy") {
            hit_object.GetComponent<Enemy>().TakeDamage(this.damage);
            Destroy(this.gameObject);
        }
    }
}
