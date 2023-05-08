using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duncan : MonoBehaviour {
    public float speed = 4;

    public Duncan Init(Vector2 displacement) {
        this.GetComponent<Rigidbody2D>().velocity = displacement * (this.speed / displacement.magnitude);
        return this;
    }
    protected void OnCollisionEnter2D(Collision2D collision) {
        ContactPoint2D contact = collision.contacts[0];
        if (contact.collider.gameObject.tag == "Wall") {
            Object.Destroy(this.gameObject);
        }
    }
}
