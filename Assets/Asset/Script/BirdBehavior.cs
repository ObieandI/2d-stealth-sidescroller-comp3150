// behaviour script for the bird 
// this script requires two game object with a collider acting as a turning point 
// make sure said objects' colliders are set to isTrigger 
//
// make sure the bird's collider is actually colliding with the turn points! 
//          ^^^^ very important ^^^^^
// make sure the bird is on the enenmy layer 
// also make sure the turning points are on the turning point layer 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehavior : MonoBehaviour
{
    public float speed; 
    private bool movingRight = true; 
    private bool turning = false; 

    // Update is called once per frame
    void Update()
    {
        if (turning)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0); 
                movingRight = false; 
                turning = false; 
            }
            else 
            {
                transform.eulerAngles = new Vector3(0, 0, 0); 
                movingRight = true; 
                turning = false; 
            }
        }

        transform.Translate(speed * Time.deltaTime, 0, 0);

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(0); 
        turning = true; 
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Caught"); 
            GameManager.Instance.Respawn(); 
            AnalyticsManager.Instance.PlayerDeath(transform.position, gameObject.name); 
        }
    }
}

