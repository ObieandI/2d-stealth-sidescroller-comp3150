/*
note for level designer: 
u need to change the name of the scene for the reset function to work correctly! 
     ^^^^^ very important ^^^^^^
scroll down to find the correct line, there are more instructuion down there  
this script is required for everything to function correctly. 
add this to an empty object
the checkpoints need to be added to this for them to function 
the order of the checkpoints matter! 
   ^^^^^ very important ^^^^^ 
ie: the first checkpoint that the player is going to should first on the list, the second one should be in the second place, etc 
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    static private GameManager instance; 
    static public GameManager Instance
    {
        get 
        {
            if (instance == null)
            {
                Debug.LogError("There is no GameManager present. "); 
            }
            return instance;  
        }
    } 

    public List<CheckPoint> listOfCheckPoints; 
    public int coinNeededForExtraLife; 
    private CheckPoint currentCP; 
    private CheckPoint nextCP; 
    private GameObject player; 
    private PlayerMovement playerMovement; 
    private int playerHealth; 
    private int coin = 0; 
    private float timeSinceStart; 
    private AnalyticsManager analyticsManager; 
    private Vector3 lastLocation; 
    private bool isPaused; 

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); 
        } 
        else 
        {
            instance = this; 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCP = listOfCheckPoints[0]; 
        player = GameObject.Find("/Player"); 
        playerMovement = player.GetComponent<PlayerMovement>(); 
        playerHealth = playerMovement.GetHealth; 
        //UIManager.Instance.UpdateHealth(); 
        analyticsManager = GameObject.Find("/AnalyticsManager").GetComponent<AnalyticsManager>(); 
    }

    void Update()
    {
        timeSinceStart += Time.deltaTime; 

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume(); 
            } 
            else 
            {
                Pause(); 
            }
        }
    }

    public CheckPoint CurrentCheckPoint 
    {
        get 
        {
            return currentCP; 
        }
    }
    
    public int IndexOfCurrentCP 
    {
        get 
        {
            return listOfCheckPoints.IndexOf(currentCP); 
        }
    }

    public void NextCheckPoint()
    {
        int i = listOfCheckPoints.IndexOf(currentCP); 
        if (i < listOfCheckPoints.Count)
        {
            if (i == (listOfCheckPoints.Count - 1)) 
            {
                Debug.Log("Won"); 
                UIManager.Instance.ShowVScreen(); 
                Time.timeScale = 0f; 
            }
            else 
            {
                currentCP = listOfCheckPoints[i + 1]; 
            }
        }
    }

    public CheckPoint CurrentSpwan
    {
        get
        {
            if (IndexOfCurrentCP == 0)
            {
                return currentCP; 
            }
            else 
            {
                return listOfCheckPoints[IndexOfCurrentCP - 1]; 
            }
            
        }
    }

    public void Respawn()
    {
        playerHealth --; 
        if (playerHealth > 0)
        {
            player.layer = LayerMask.NameToLayer("Player"); 
            player.GetComponent<SpriteRenderer>().enabled = true; 
            UIManager.Instance.UpdateHealth(); 
            playerMovement.Respawn(CurrentSpwan.GetComponent<Transform>().position); 
            player.GetComponent<Collider2D>().enabled = true; 
        }
        else 
        {
            Debug.Log("fooked"); 
            Time.timeScale = 0f; 
            UIManager.Instance.ShowLScreen(); 
            UIManager.Instance.UpdateHealth(); 
            lastLocation = player.transform.position; 
            analyticsManager.EndOfTheGame(lastLocation); 
        }
        
    }

    public void PickUp() 
    {
        coin ++;  
        UIManager.Instance.UpdateCoin(); 
        if (coin == coinNeededForExtraLife)
        {
            playerHealth ++; 
            UIManager.Instance.UpdateHealth(); 
            AudioManager.Instance.PlayLifeSound(); 
            coin = 0; 
            UIManager.Instance.UpdateCoin(); 
        }
    }

    public int CurrentHealth
    {
        get 
        {
            return playerHealth; 
        }
    }

    public int CurrentCoin
    {
        get 
        {
            return coin; 
        }
    }

// replace ScriptTestingScene with the name of whatever scene u want to reload! 
// unity doc on scenemanager.loadscene: https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
// double check to make sure u spell the name correctly, unity will NOT complain until this function is being called, which is when the button is clicked

    public void Reset() 
    {
        currentCP = listOfCheckPoints[0]; 
        Debug.Log(SceneManager.GetActiveScene().name); 
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single); 
    }

    public float GetTimeSinceStart
    {
        get 
        {
            return timeSinceStart; 
        }
    }

    public void Pause()
    {
        isPaused = true; 
        Time.timeScale = 0f; 
        UIManager.Instance.ShowPScreen(); 
    }

    public void Resume()
    {
        isPaused = false; 
        Time.timeScale = 1f; 
        UIManager.Instance.HidePScreen(); 
    }

    public void Quit()
    {
        Application.Quit(); 
    }
}
