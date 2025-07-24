// player movement script
// make sure the player is on the player layer 
// the player needs a empty game object to act as ground check 
// make sure the ground check is under player's feet and low enough so it will detect the ground correctly 
// the player also needs an empty game object for attack 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Move and Jump")]
    public float speed = 0; 
    private float playerSpd = 0; 
    public float speedOnBranch = 0; 
    public float jumpHeight = 0;
    public GameObject groundCheck; 
    public float maxFallingSpeed; 
    private float currentSpeed; 
    private bool onGround = true; 
    private bool onBranch = false; 
    private float jumpForce = 0f; 

    [Header("Climbing")]
    public float climbingSpeed = 5f; 
    private float vertical; 
    private bool isLadder = false; 
    private bool isClimbing = false; 

    [Header("Health")]
    public int playerHealth; 

    [Header("PlayerAttack")]
    public Sprite normalPlayer; 
    public Sprite attackPlayer; 
    public float timeBetweenAttack; 
    private float timerBetweenAttack; 
    public Transform attackPosition; 
    public float attackRange; 
    public LayerMask enemyLayer;  

    private Rigidbody2D rb; 
    private SpriteRenderer sr; 
    private GameManager gameManager; 
    private float t; 

    
    void Start()
    {
        playerSpd = speed; 
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); 
        gameManager = GameObject.Find("/GameManager").GetComponent<GameManager>(); 
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));  
        timerBetweenAttack = timeBetweenAttack;   
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(onGround);
        onBranch = Physics2D.Linecast(transform.position, groundCheck.transform.position, 3 << LayerMask.NameToLayer("Branch"));
        float direction = Input.GetAxis("Horizontal"); 
        if (onBranch)
        {
            rb.velocity = new Vector2(speedOnBranch * direction, rb.velocity.y); 
        }
        else 
        {
            rb.velocity = new Vector2(speed * direction, rb.velocity.y); 
        }
        

        //Debug.Log(rb.velocity); 

        if (Input.GetButtonDown("Jump") && (onGround || onBranch))
        {
            onGround = false; 
            AudioManager.Instance.PlayJumpSound(); 
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse); 
        }

        onGround = Physics2D.Linecast(transform.position, groundCheck.transform.position, 3 << LayerMask.NameToLayer("Ground"));

        //Debug.Log(onGround); 

        vertical = Input.GetAxis("Vertical"); 

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true; 
        }

        if (timerBetweenAttack <=0)
        { 
            if (Input.GetButtonDown("Fire1"))
            {
                Collider2D enemyHit = Physics2D.OverlapCircle(attackPosition.position, attackRange, enemyLayer); 
                try 
                {
                    enemyHit.GetComponent<EnemyPatrol>().KnockedOut();
                    timerBetweenAttack = timeBetweenAttack;
                    AudioManager.Instance.PlayHitSound(); 
                    sr.sprite = attackPlayer; 
                }
                catch (Exception e)
                {
                    sr.sprite = attackPlayer; 
                }
                t = 0.2f; 
                //Debug.Log(enemyHit);  
            }
            else if (Input.GetButtonUp("Fire1") || t < 0)
            {
                sr.sprite = normalPlayer; 
            }
        }
        else 
        {
            timerBetweenAttack -= Time.deltaTime;  
        }
        t -= Time.deltaTime; 
        //Debug.Log(timerBetweenAttack); 

    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f; 
            rb.velocity = new Vector2(rb.velocity.x, vertical * climbingSpeed); 
        }
        else 
        {
            rb.gravityScale = 4f; 
        }

        if (rb.velocity.y < 0)
        {
            currentSpeed = Vector3.Magnitude(rb.velocity); 

            if (maxFallingSpeed < currentSpeed)
            {
                float brakeSpeed = currentSpeed - maxFallingSpeed; 

                Vector3 normalVelocity = rb.velocity.normalized; 
                Vector3 brakeVelocity = normalVelocity * brakeSpeed;
                rb.AddForce(-brakeVelocity);  
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true; 
        }

        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("HidingSpot"))
        {
            speed = 0; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false; 
            isClimbing = false; 
        }

        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("HidingSpot"))
        {
            speed = playerSpd; 
        }
    }

    public void Respawn(Vector2 location)
    {
        rb.velocity = Vector3.zero; 
        transform.position = location;  
    }

    public int GetHealth
    {
        get 
        {
            return playerHealth;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(attackPosition.position, attackRange); 
    }
}
