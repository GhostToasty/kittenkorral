using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int health = 9;
    private int currentHealth;
    public Slider healthBar;

    public float iFramesDuration = 1f;
    private bool isInvincible = false; 

    public GameObject deathUI;

    // Start is called before the first frame update
    void Start()
    {
        deathUI.SetActive(false);
        currentHealth = health;

        healthBar.maxValue = health;
        healthBar.value = currentHealth;
    }

    public void TakeDamage()
    {
        if(isInvincible) {
            return; 
        }

        currentHealth--;
        healthBar.value = currentHealth;

        if(currentHealth <= 0) {
            Debug.Log("you died");
            Die();
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
        currentHealth++;
        if(currentHealth > health) {
            currentHealth = health; // prevent player from healing more than max health
        }
        healthBar.value = currentHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("hit " + other.name);
        if(other.tag == "pickup") {
            Heal();
            Destroy(other.gameObject); // destroy the pickup
        }
        else if(other.tag == "Respawn") {
            Die();
        }
    }

    void Die()
    {
        deathUI.SetActive(true);

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerLook>().enabled = false;
        GetComponent<Shooting>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
