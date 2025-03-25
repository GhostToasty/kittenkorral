using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public int health = 10;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0) {
            foreach(Transform child in transform) {
                child.GetComponent<Cat>().RemoveFishComponent();
                child.SetParent(null);
            }

            Destroy(gameObject);
        }
    }
}
