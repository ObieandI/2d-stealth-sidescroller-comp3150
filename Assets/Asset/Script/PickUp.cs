// this is the script for the pick up 
// make sure the object is on the PickUp layer 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {   
            GameManager.Instance.PickUp(); 
            AudioManager.Instance.PlayPickUpSound(); 
            Destroy(gameObject); 
        }
    }
}
