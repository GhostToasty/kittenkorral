using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAttack : MonoBehaviour
{
    public Transform attackOrigin;
    public float attackRange = 1f; 
    public float attackCooldown = 2f;
    public LayerMask playerLayer;
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
        
        Collider[] hits = Physics.OverlapSphere(attackOrigin.position, attackRange, playerLayer);
        foreach(Collider hit in hits) {
            if(timer >= attackCooldown) {
                timer = 0f;

                PlayerStats player = hit.GetComponent<PlayerStats>();
                if(player != null) {
                    player.TakeDamage();
                }
                
                // animation/sound would go here

                break;
            }
        }
    }

    void OnEnable()
    {
        timer = 0f; // reset timer when script is enabled
    }

    void OnDrawGizmosSelected()
    {
        if (attackOrigin == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin.position, attackRange);
    }
}
