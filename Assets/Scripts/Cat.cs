using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour
{
    public int damage = 1;
    public LayerMask targetLayer;

    private bool canAttack;

    private Rigidbody rb;
    private Fish fishComp;

    // for spawner functionality
    private CatSpawner spawner;

    private NavMeshAgent agent;
    private BrownianMotion motion;
    private float moveDelay = 1f;
    private float timer = 0f;
    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        motion = GetComponent<BrownianMotion>();

        rb = GetComponent<Rigidbody>();
        canAttack = true;
        isAttacking = false;

        spawner = FindAnyObjectByType<CatSpawner>();
    }

    void Update()
    {
        if(canAttack) {
            if(motion.enabled) {
                motion.enabled = false;
                agent.enabled = false;
            }
        }
        else {
            if(!motion.enabled) {
                motion.enabled = true;
                agent.enabled = true;
            }
        }

        timer += Time.deltaTime;
        if(timer >= moveDelay && !isAttacking) {
            canAttack = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!canAttack) {
            return;
        }

        isAttacking = true;

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
            // in case the fish dies
            if(fishComp == null) {
                StopAttacking();
                yield break;
            }

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

        isAttacking = false;
    }

    void OnDestroy()
    {
        if(spawner != null) {
            spawner.RemoveCat(gameObject);
        }
    }

    public void RemoveFishComponent()
    {
        fishComp = null;
    }
}
