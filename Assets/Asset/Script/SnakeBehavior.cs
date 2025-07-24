// this is the snake behaviour script 
// this script requiers a ground check and wall check to function 
// make sure the ground check is actually below the snake and low enough to hit the ground 
// make sure the wall check is in front of the snake 
// make sure all the variables are correctly assigned 
// ie: left sprite actually has the left sprite instead of the right one 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBehavior : MonoBehaviour
{
    public float speed; 
    public float timeInBin; 
    public GameObject groundCheck; 
    public GameObject wallCheck; 
    public Sprite rightSprite; 
    public Sprite leftSprite; 
    
    private bool onGround = true; 
    private bool wallThere = false; 
    private bool movingRight = true; 
    private float timerInBin; 
    private bool inBin = false; 
    private float privateSpeed; 
    private SpriteRenderer sr; 

    // Start is called before the first frame update
    void Start()
    {     
        privateSpeed = speed; 
        timerInBin = timeInBin; 
        sr = GetComponent<SpriteRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.Raycast(groundCheck.transform.position, transform.TransformDirection(Vector3.down), 0.5f, 3 << LayerMask.NameToLayer("Ground")); 
        //Debug.DrawRay(groundCheck.transform.position, transform.TransformDirection(Vector3.down), Color.green); 
        wallThere = Physics2D.Raycast(wallCheck.transform.position, transform.TransformDirection(Vector3.right), 0.1f, 3 << LayerMask.NameToLayer("Ground")); 
        //Debug.DrawRay(wallCheck.transform.position, transform.TransformDirection(Vector3.right), Color.red); 
        
        if (onGround == false | wallThere == true) 
        {
            if (movingRight) {
                transform.eulerAngles = new Vector3(0, -180, 0); 
                //speed *= -1; 
                movingRight = false; 
                //sr.sprite = leftSprite; 
                //Debug.Log("turn left"); 
                //Debug.Log("movingRight: " + movingRight); 
            }
            else 
            {
                transform.eulerAngles = new Vector3(0, 0, 0); 
                //speed *= -1;                 
                movingRight = true; 
                //sr.sprite = rightSprite; 
                //Debug.Log("movingright: " + movingRight); 
            }
        } 

        if (inBin)
        {
            timerInBin -= Time.deltaTime; 
            if (timerInBin <= 0)
            {
                inBin = false; 
                speed = privateSpeed; 
            }
        }

        transform.Translate(speed * Time.deltaTime, 0, 0);
         
        //Debug.Log("onGround: " + onGround); 
        //Debug.Log("wallThere: " + wallThere); 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
       //Debug.Log("Hit bin"); 
       //Debug.Log(col.transform.gameObject.name); 
       inBin = true; 
       speed = 0; 
       sr.enabled = false; 
    }

    void OnTriggerExit2D(Collider2D col)
    {
        sr.enabled = true; 
        timerInBin = timeInBin; 
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Caught"); 
            col.transform.gameObject.GetComponent<Collider2D>().enabled = false; 
            GameManager.Instance.Respawn(); 
            AnalyticsManager.Instance.PlayerDeath(transform.position, gameObject.name); 
        }
    }
}
