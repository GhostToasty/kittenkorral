using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
   public AudioSource audiosource;
   private bool audioPlay;
   Vector3 newPosition;

    
    // Start is called before the first frame update
    void Start()
    {
       Vector3 newPosition = transform.position;

       if (transform.position.y <= 1.09)
        {
            if ((transform.position.x != newPosition.x) || (transform.position.z != newPosition.z))
            audiosource.Play();
            audioPlay = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= 1.09)
        {
            if ((transform.position.x != newPosition.x) || (transform.position.z != newPosition.z))
            audiosource.UnPause();
            audioPlay = true;

            if ((transform.position.x == newPosition.x) && (transform.position.z == newPosition.z))
            audiosource.Pause();
            audioPlay = false;

        }

        else
        {
            audiosource.Pause();
            audioPlay = false;
        }

         newPosition = transform.position;

    }
}
