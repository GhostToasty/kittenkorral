using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CatSound : MonoBehaviour
{
    //https://www.youtube.com/watch?v=lqyzGntF5Hw

    public AudioClip[] audiosounds;
    public AudioSource audiosource;
    public GameObject wood_menu;


    private bool yarnMode;
    private string sceneName;
    public int catCount;
    
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        yarnMode = true;
        catCount = 0;
        Yarn.OnCatCaught += catCaught;
    }

    // Update is called once per frame
    void Update()
    {
        if (wood_menu.activeSelf == false)
        {
            if(Input.GetMouseButtonDown(1))
            {
                yarnMode =! yarnMode;
            }

            if((yarnMode == false && Input.GetMouseButtonDown(0)) && (catCount > 0))
            {
                audiosource.clip = audiosounds[Random.Range(0, audiosounds.Length)];
                audiosource.PlayOneShot(audiosource.clip);
                catCount--;
            }
        }
            
    }

    private void catCaught(GameObject cat)
    {
        catCount++;
        Debug.Log(catCount);
    }

}
