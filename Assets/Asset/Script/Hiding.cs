// this is the script for the bin 
// make sure the bin is on the hiding spot layer 
// make sure the bin has the correct sprite for its two different states 
// the trap bin requires a snake present in the scene. 
// the snake actaully goes in and hid in it, so the snake needs to be able to get to bin 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : MonoBehaviour
{
    public Sprite normalBin; 
    public Sprite trapBin; 

    private GameObject player; 
    private Collider2D playerCol;  
    private SpriteRenderer playerSR; 
    private Rigidbody2D playerRB; 
    private bool trap = false; 
    private bool occupied = false; 
    private SpriteRenderer selfSR; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("/Player"); 
        playerCol = player.GetComponent<Collider2D>(); 
        playerSR = player.GetComponent<SpriteRenderer>(); 
        playerRB = player.GetComponent<Rigidbody2D>(); 
        selfSR = GetComponent<SpriteRenderer>(); 
    }

    void Update()
    {
        if (trap)
        {
            selfSR.sprite = trapBin; 
        } 
        else 
        {
            selfSR.sprite = normalBin; 
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (playerRB.velocity.y < 0 && trap == false)
            {
                //playerCol.enabled = false; 
                playerSR.enabled = false; 
                player.layer = LayerMask.NameToLayer("Default"); 
                occupied = true; 
                //player.SetActive(false); 
            }
            else if (playerRB.velocity.y < 0 && trap)
            {
                GameManager.Instance.Respawn(); 
                AnalyticsManager.Instance.PlayerDeath(transform.position, gameObject.name); 
                Debug.Log("Caught"); 
            }
        } 

        if (col.transform.gameObject.layer == LayerMask.NameToLayer("EnemySnake"))
        {
            trap = true; 
            if (occupied)
            {
                GameManager.Instance.Respawn(); 
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            playerCol.enabled = true;
            playerSR.enabled = true; 
            player.layer = LayerMask.NameToLayer("Player"); 
            occupied = false; 
        }
            
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("EnemySnake"))
        {
            trap = false; 
        }

    }

}
