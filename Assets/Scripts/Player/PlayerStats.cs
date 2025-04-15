using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int health = 9;
    private int currentHealth; 

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0) {
            Debug.Log("you died");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            // TODO: replace w/ actual death logic
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        // TODO: update health bar (when it gets added)
        Debug.Log("health: " + currentHealth);
    }
}
