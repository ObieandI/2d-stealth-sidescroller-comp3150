using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class PlayerMovementPhysics : MonoBehaviour
{
    public float speed = 0; 
    public float jump = 0; 
    private Rigidbody2D rb; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Input.GetAxis("Horizontal"); 
        rb.velocity = new Vector2(speed * direction, rb.velocity.y); 

        Debug.Log(rb.velocity); 

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.1)
        // when space has been hit and when player's vertical speed is 0
        {
            rb.AddForce(new Vector2(0, jump), ForceMode2D.Impulse); 
        }
    }
}
