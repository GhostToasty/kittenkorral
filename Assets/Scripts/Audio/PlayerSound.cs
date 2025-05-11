using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
   public AudioSource audiosourcePlayer;
   private bool firstMove;
   Vector3 newPosition;

    
    // Start is called before the first frame update
    void Start()
    {
       Vector3 newPosition = transform.position;
       firstMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= 1.09)
        {
            if ((transform.position.x != newPosition.x) || (transform.position.z != newPosition.z))
            {
                    audiosourcePlayer.UnPause();
                    //Debug.Log("audio unpaused");
            }

            if ((transform.position.x == newPosition.x) && (transform.position.z == newPosition.z))
            {
                audiosourcePlayer.Pause();
                //Debug.Log("audio paused");
            }

        }

        else
        {
            audiosourcePlayer.Pause();
            //Debug.Log("audio paused");
        }

        newPosition = transform.position;
        firstPlay();

    }

    public void firstPlay()
    {
        if (firstMove == true)
               {
                    audiosourcePlayer.Play();
                    firstMove = false; 
                    Debug.Log("audio played");
               }
    }
}
