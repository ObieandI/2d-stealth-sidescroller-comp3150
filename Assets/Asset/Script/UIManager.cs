// note for level designer: 
// this is script is required so everything will function correctly 
// add this to an empty object 
// make sure u have ui elements, otherwise the enemies cant kill the player 
   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIManager : MonoBehaviour
{
    static private UIManager instance; 
    static public UIManager Instance
    {
        get 
        {
            if (instance == null)
            {
                Debug.LogError("There is no UIManager present. "); 
            }
            return instance; 
        }
    }

    
    public Text health; 
    public Text coin; 
    private GameManager gameManager; 
    public GameObject VScreen; 
    public GameObject LScreen; 
    public GameObject PScreen; 

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
        health.text = gameManager.CurrentHealth.ToString();  
        coin.text = gameManager.CurrentCoin.ToString(); 
    }

    public void UpdateHealth()
    {
        health.text = gameManager.CurrentHealth.ToString(); 
    }

    public void UpdateCoin()
    {
        coin.text = gameManager.CurrentCoin.ToString(); 
    }

    public void ShowVScreen()
    {
        VScreen.SetActive(true); 
    }

    public void ShowLScreen()
    {
        LScreen.SetActive(true); 
    }

    public void ShowPScreen()
    {
        PScreen.SetActive(true); 
    }

    public void HidePScreen()
    {
        PScreen.SetActive(false); 
    }
}



