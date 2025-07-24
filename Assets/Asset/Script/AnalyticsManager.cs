// not really working, still trying to figure out 
// still required for everything to work 
// same as all the other managers, add to an empty, drop in the scene 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics; 

public class AnalyticsManager : MonoBehaviour
{
    static private AnalyticsManager instance; 
    static public AnalyticsManager Instance 
    {
        get 
        {
            if (instance == null)
            {
                Debug.LogError("There is no Analytics Manager present. "); 
            }
            return instance; 
        }
    }

    private GameManager gameManager; 

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

    void Start()
    {
        gameManager = GameObject.Find("/GameManager").GetComponent<GameManager>(); 
    }

    public void StartOfTheGame()
    {
        Analytics.CustomEvent("victory", new Dictionary<string, object>
        {
            {"time: ", gameManager.GetTimeSinceStart}
        }); 
        AnalyticsEvent.GameStart(); 
    }

    public void EndOfTheGame(Vector3 lastLocation)
    {
        Analytics.CustomEvent("defeat", new Dictionary<string, object>
        {
            {"time: ", gameManager.GetTimeSinceStart}, 
            {"player position: ", lastLocation}, 
        }); 
        AnalyticsEvent.GameOver(); 
    }

    public void PlayerDeath(Vector3 location, string killedBy)
    {
        Analytics.CustomEvent("death", new Dictionary<string, object>
        {
            {"location: ", location}, 
            {"killed by: ", killedBy}
        }); 
    }


}
