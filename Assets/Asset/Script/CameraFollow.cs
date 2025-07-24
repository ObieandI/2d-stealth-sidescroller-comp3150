// this is the camera following script 
// make sure the target is set on the player 
// be careful changing the smooth factor
// setting the smooth factor too low might be uncomfortable for some player 
// try play around with it, but dont go crazy 

using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset; 
    public float smoothFactor;
    public float cameraRange;  
    private Vector3 currentPos; 
    private RaycastHit2D hit; 
    private float whereGround; 
    
    void Update()
    {
        Follow(); 

        currentPos = target.position; 
        if (Input.GetKey("down"))
        {
            hit = Physics2D.Raycast(target.position, -Vector2.up, Mathf.Infinity, 3 << LayerMask.NameToLayer("Ground")); 
            Debug.Log(hit.distance); 
            if (hit.distance > cameraRange)
            {
                whereGround = target.position.y - cameraRange; 
            }
            else 
            {
                whereGround = target.position.y - hit.distance; 
            }
    
            transform.position = new Vector3(currentPos.x, whereGround, -10); 
        }
        else if (Input.GetKeyUp("down"))
        {
            transform.position = currentPos; 
        }

    }

    void Follow() 
    {
        Vector3 targetPosition = target.position + offset; 
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime); 
        transform.position = smoothedPosition; 
    }
}
