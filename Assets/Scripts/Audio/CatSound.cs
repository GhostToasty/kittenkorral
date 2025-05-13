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


    public int catCount;
    [Range(0.1f, 0.5f)]
    private float pitchChange = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        catCount = 0;
        Yarn.OnCatCaught += catCaught;
    }

    // Update is called once per frame
    void Update()
    {
        if (wood_menu.activeSelf == false)
        {
            if(Input.GetMouseButtonDown(0) && (catCount > 0))
            {
                audiosource.clip = audiosounds[Random.Range(0, audiosounds.Length)];
                audiosource.pitch = Random.Range(1 - pitchChange, 1 + pitchChange);
                audiosource.PlayOneShot(audiosource.clip);
                catCount--;
                Debug.Log("meow");
            }
        }
            
    }

    private void catCaught(GameObject cat)
    {
        catCount++;
        Debug.Log(catCount);
    }

}
