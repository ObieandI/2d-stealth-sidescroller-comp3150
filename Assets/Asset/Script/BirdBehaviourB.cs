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

public class BirdBehaviourB : MonoBehaviour
{
    public float speed; 
    
    // Update is called once per frame
    void Update()
    {

        transform.Translate(0,speed * Time.deltaTime, 0);

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(0); 
        speed *= -1; 
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

