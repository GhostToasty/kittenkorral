using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishShoot : MonoBehaviour
{
    public GameObject projectile;
    public Transform shootPoint;
    public float shotSpeed = 5f;
    public float shootInterval = 1f; 
    private float timer = 0f;

    void Awake()
    {
        // make sure script disabled from beginning (bc it won't have anything to shoot at)
        if(enabled) {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= shootInterval) {
            timer = 0f; 
            
            GameObject newProjectile = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
            FishProjectile projectileComp = newProjectile.GetComponent<FishProjectile>();
            projectileComp.Initialize(shootPoint.forward);


            // shooting method using rigidbody
            // Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            // rb.velocity = shootPoint.forward * shotSpeed;
        }
    }
}
