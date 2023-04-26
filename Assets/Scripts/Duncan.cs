using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duncan : MonoBehaviour
{
    public float speed = 4;

    public void Init(Vector2 displacement){
        GetComponent<Rigidbody2D>().velocity = displacement * (this.speed / displacement.magnitude);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        if(contact.collider.gameObject.tag == "Wall"){
            Destroy(this.gameObject);
        }
    }
}
