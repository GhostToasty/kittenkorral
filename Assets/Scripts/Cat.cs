using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public int damage = 1;
    public LayerMask targetLayer;

    private bool canAttack;

    private Rigidbody rb;
    private Fish fishComp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!canAttack) {
            return;
        }

        if(((1 << collision.gameObject.layer) & targetLayer) != 0) {
            Debug.Log("enemy hit");
            
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true; // prevent further physics interactions

            transform.parent = collision.transform; // if the fish is moving the cat needs to follow
            fishComp = collision.gameObject.GetComponent<Fish>();

            StartCoroutine(AttackCycle());
        }
    }

    IEnumerator AttackCycle()
    {
        for(int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(1f);
            fishComp.TakeDamage(damage);
        }

        StopAttacking();
    }

    void StopAttacking()
    {
        canAttack = false; // prevent it from re-colliding with enemy
        transform.parent = null; 

        // might need to re-add later for more of a jump?
        // rb.velocity = Vector3.zero;
        // rb.angularVelocity = Vector3.zero;

        rb.useGravity = true;
        rb.isKinematic = false;
    }
}
