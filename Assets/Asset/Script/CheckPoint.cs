//this is the script for checkpoint 
// make sure the checkpoint is set on checkpoint layer 
// sprite will be added to it once simon finishes working on it 
// so make sure it is assigned correctly 
// checkpoints needs to be added into the game manager to fuction 
// the victory screen shows up once the player run through the last checkpoint
// put a checkpoint at the every beginning of the level, very close to the player 
// close enough so they cant miss it 
// so u need to have at least 2 checkpoints  


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class CheckPoint : MonoBehaviour
{
    public Sprite unclaimed; 
    public Sprite claimed; 

    private GameObject player; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("/Player"); 
        GetComponent<SpriteRenderer>().sprite = unclaimed; 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(0); 
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (GameManager.Instance.CurrentCheckPoint.GetComponent<Transform>().position == transform.position)
            {
                GameManager.Instance.NextCheckPoint();
                GetComponent<SpriteRenderer>().sprite = claimed;  
                Debug.Log(GameManager.Instance.IndexOfCurrentCP); 
            }
        }
    }

}
