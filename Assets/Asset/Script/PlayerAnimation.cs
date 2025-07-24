//a script to flip the player when the player changes direction 
//probably could get away with leave this in the main player movement script, but what's done is done 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Rigidbody2D rb;  
    private float playerSpeed; 
    private float currentDirection; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        currentDirection = transform.localScale.x; 
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = rb.velocity.x; 
        if (playerSpeed < 0)
        {
            currentDirection = -1; 
        } 
        else if (playerSpeed > 0)
        {
            currentDirection = 1; 
        }
        transform.localScale = new Vector2 (currentDirection, transform.localScale.y); 
    }
}
