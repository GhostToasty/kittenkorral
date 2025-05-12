//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GunSound : MonoBehaviour
{
    //https://www.youtube.com/watch?v=lqyzGntF5Hw

    public AudioClip[] audiosounds;
    public AudioSource audiosource;
    public GameObject wood_menu;

    private string sceneName;
    
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wood_menu.activeSelf == false)
        {
            if(Input.GetMouseButtonDown(1))
            {
                audiosource.clip = audiosounds[Random.Range(0, audiosounds.Length)];
                audiosource.PlayOneShot(audiosource.clip);
            }
        }
            
    }

}
