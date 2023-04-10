using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duncan : MonoBehaviour
{
    public float speed = 4;
    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        if(contact.collider.gameObject.tag == "Wall"){
            Destroy(gameObject);
        }
    }
}
