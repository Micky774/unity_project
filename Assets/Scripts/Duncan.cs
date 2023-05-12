using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectile for use with basic weapon
/// </summary>
public class Duncan : MonoBehaviour {
    /// <summary>
    /// Speed at which Duncan moves
    /// </summary>
    public float speed = 4;

    /// <summary>
    /// Initializes a Duncan with the correct velocity
    /// </summary>
    /// <param name="displacement"> 2D displacement vector from player to target (e.g. mouse cursor position)</param>
    /// <returns> The initialized Duncan </returns>
    public Duncan Init(Vector2 displacement) {
        this.GetComponent<Rigidbody2D>().velocity = displacement * (this.speed / displacement.magnitude);
        return this;
    }

    /// <summary>
    /// Handles collisions between Duncan and other entities
    /// </summary>
    /// <param name="collision"> Details of collision </param>
    protected void OnCollisionEnter2D(Collision2D collision) {
        ContactPoint2D contact = collision.contacts[0];
        if(contact.collider.gameObject.tag == "Wall") {
            Object.Destroy(this.gameObject);
        }
    }
}
