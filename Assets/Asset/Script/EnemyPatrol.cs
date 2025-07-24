// behaviour for the dog 
// this script require a ground check and a wall check to function 
// create a empty object for both the ground check and wall check 
// the ground check needs to be under the enemy's feet and make sure it's low enough to hit the ground 
// the wall check should be in front of the dog
// make sure the enemy is on the enemy layer. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement Speed")]
    public float patrolSpeed = 0f;
    public float chaseSpeed = 0f; 
    private float chaseSpd = 0f; 

    [Header("Checks")]
    public GameObject groundCheck; 
    public GameObject wallCheck; 
    private GameObject player; 

    [Header("Timer")]
    public float coolDownTime; 
    private float coolDownTimer; 
    public float knockOutTime; 
    private float knockOutTimer; 

    [Header("Sprites")]
    public Sprite normalEnemy; 
    public Sprite alertedEnemy; 
    public Sprite stunnedEnemy; 
    public float alertTime; 
    private float alertTimer; 
    private SpriteRenderer sr; 

    private bool onGround; 
    private bool wallThere = false; 
    private bool movingRight = true; 
    private bool outOfSight = true; 

    private State state; 

    enum State 
    {
        kalm, 
        angy, 
        knockedOut
    }

    void Start()
    {
        state = State.kalm; 
        player = GameObject.Find("/Player"); 
        knockOutTimer = knockOutTime; 
        alertTimer = alertTime; 
        sr = GetComponent<SpriteRenderer>(); 
        chaseSpd = chaseSpeed; 
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.kalm)
        {
            KalmBehavior(); 
        } 
        else if (state == State.angy)
        {
            AngyBehavior(); 
            if (alertTimer > 0)
            {
                sr.sprite = alertedEnemy; 
            }
            else 
            {
                sr.sprite = normalEnemy; 
            }
            alertTimer -= Time.deltaTime; 

            if (outOfSight)
            {
                coolDownTimer -= Time.deltaTime; 
                if (coolDownTimer <= 0)
                {
                    state = State.kalm; 
                    chaseSpd = chaseSpeed; 
                }
            }
        }
        else 
        {
            knockOutTimer -= Time.deltaTime; 
            transform.Translate(0, 0, 0); 
            sr.sprite = stunnedEnemy; 
            
            if (knockOutTimer <= 0)
            {
                Debug.Log(0); 
                state = State.kalm; 
                gameObject.layer = LayerMask.NameToLayer("Enemy"); 
            }
        }
        
        //Debug.Log(knockOutTimer); 
        //Debug.Log(state); 
        //Debug.Log(coolDownTimer); 
        
    }

    private void KalmBehavior()
    {
        onGround = Physics2D.Raycast(groundCheck.transform.position, transform.TransformDirection(Vector3.down), 0.5f, 3 << LayerMask.NameToLayer("Ground")); 
        //Debug.DrawRay(groundCheck.transform.position, transform.TransformDirection(Vector3.down), Color.green); 
        wallThere = Physics2D.Raycast(wallCheck.transform.position, transform.TransformDirection(Vector3.right), 0.1f, 3 << LayerMask.NameToLayer("Ground")); 
        //Debug.DrawRay(wallCheck.transform.position, transform.TransformDirection(Vector3.right), Color.red); 

        //Debug.Log("onGround: " + onGround); 
        //Debug.Log("wallThere: " + wallThere); 

        if (onGround == false | wallThere == true) 
        {
            if (movingRight) {
                transform.eulerAngles = new Vector3(0, -180, 0); 
                movingRight = false; 
            } 
            else 
            {
                transform.eulerAngles = new Vector3(0, 0, 0); 
                movingRight = true; 
            }
        } 

        transform.Translate(patrolSpeed * Time.deltaTime, 0, 0); 
        sr.sprite = normalEnemy; 

        
    }

    private void AngyBehavior()
    {
        onGround = Physics2D.Raycast(groundCheck.transform.position, transform.TransformDirection(Vector3.down), 0.5f, 3 << LayerMask.NameToLayer("Ground")); 
        //Debug.DrawRay(groundCheck.transform.position, transform.TransformDirection(Vector3.down), Color.green); 
        wallThere = Physics2D.Raycast(wallCheck.transform.position, transform.TransformDirection(Vector3.right), 0.1f, 3 << LayerMask.NameToLayer("Ground")); 
        //Debug.DrawRay(wallCheck.transform.position, transform.TransformDirection(Vector3.right), Color.red); 

        //Debug.Log("onGround: " + onGround); 
        //Debug.Log("wallThere: " + wallThere); 

        if (onGround == false | wallThere == true) 
        {
            if (movingRight) {
                transform.eulerAngles = new Vector3(0, -180, 0); 
                movingRight = false; 
            } 
            else 
            {
                transform.eulerAngles = new Vector3(0, 0, 0); 
                movingRight = true; 
            }
        } 
        
        if (player.transform.position.x > transform.position.x && movingRight == false )
        {
            transform.eulerAngles = new Vector3(0, 0, 0); 
            movingRight = true; 
        } 
        else if (player.transform.position.x < transform.position.x && movingRight == true )
        {
            transform.eulerAngles = new Vector3(0, -180, 0); 
            movingRight = false; 
        }

        transform.Translate(chaseSpd * Time.deltaTime, 0, 0); 
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            state = State.angy; 
            alertTimer = alertTime; 
            outOfSight = false; 
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            outOfSight = true;  
            coolDownTimer = coolDownTime; 
            chaseSpd = 0;    
            Debug.Log("Lost the player"); 
        }
    } 

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.gameObject.layer == LayerMask.NameToLayer("Player") & state == State.angy)
        {
            Debug.Log("Caught"); 
            GameManager.Instance.Respawn(); 
            AnalyticsManager.Instance.PlayerDeath(transform.position, gameObject.name); 
            state = State.kalm; 
        }
    }

    public void KnockedOut()
    { 
        if (state == State.kalm)
        {
            state = State.knockedOut; 
            knockOutTimer = knockOutTime; 
            gameObject.layer = LayerMask.NameToLayer("NonActive"); 
        }   
    }

}
