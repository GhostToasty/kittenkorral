using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int health = 9;
    private int currentHealth; 

    public int healingAmount = 1;

    public float iFramesDuration = 1f;
    private bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    public void TakeDamage()
    {
        if(isInvincible) {
            return; 
        }

        currentHealth--;
        // TODO: update health bar (when it gets added)
        Debug.Log("health: " + currentHealth);

        if(currentHealth <= 0) {
            Debug.Log("you died");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            // TODO: replace w/ actual death logic instead of reloading the scene instantly
        }

        StartCoroutine(IFrames());
    }

    IEnumerator IFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(iFramesDuration);
        isInvincible = false;
    }

    void Heal()
    {
        currentHealth += healingAmount;
        if(currentHealth > health) {
            currentHealth = health; // prevent player from healing more than max health
        }
        Debug.Log("health: " + currentHealth);
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("hit " + other.name);
        if(other.tag == "pickup") {
            Heal();
            Destroy(other.gameObject); // destroy the pickup
        }
    }
}
