// this script is required for everything to function correctly 
// audio sources needs to be created and assign to this script 
// add this to an empty object and leave it in the scene 
// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update

    static private AudioManager instance; 
    static public AudioManager Instance
    {
        get 
        {
            if (instance == null)
            {
                Debug.LogError("There is no AudioManager present. "); 
            }
            return instance; 
        }
    }

    public AudioSource pickUpSound; 
    public AudioSource jumpSound; 
    public AudioSource hitSound; 
    public AudioSource lifeSound; 

    public AudioSource gameAudioSound; 


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

    public void PlayPickUpSound()
    {
        pickUpSound.Play(); 
    }

    public void PlayHitSound()
    {
        hitSound.Play(); 
    }

    public void PlayJumpSound()
    {
        jumpSound.Play(); 
    }

    public void PlayLifeSound()
    {
        lifeSound.Play();
    }

    public void PlayGameAudioSound()
    {
        gameAudioSound.Play();
    }
}
