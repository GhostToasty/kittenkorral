using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public delegate void FishDied(); // tell the other scripts when it dies
    public static event FishDied OnFishDied;

    public int health = 10;
    private int currentHealth;

    public GameObject healthPickup;
    public float dropChance = 0.5f;

    public AudioSource audiosourceFish; //added by Alyssa

    // Start is called before the first frame update
    void Start()
    {
        if(audiosourceFish == null) {
            GameObject srcObj = GameObject.FindGameObjectWithTag("FishSound");
            audiosourceFish = srcObj.GetComponent<AudioSource>();
        }
        
        currentHealth = health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0) {
            OnFishDied.Invoke();
            audiosourceFish.Play(); //addded by Alyssa
            CheckForPickup();
            Destroy(gameObject);
        }
    }

    void CheckForPickup()
    {
        float rand = Random.Range(0f, 1f);
        if(rand <= dropChance) {
            Instantiate(healthPickup, transform.position, Quaternion.identity);
        }
    }
}
