using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour
{
    public GameObject catPrefab;
    public int damage = 1;
    public LayerMask targetLayer;
    public LayerMask walkLayer;

    private bool canAttack;

    private NavMeshAgent agent;
    private BrownianMotion motion;
    private Rigidbody rb;

    // for spawner functionality
    private CatSpawner spawner;

    private Coroutine attackCoroutine;

    void Awake()
    {
        // call in awake so the check gets done first
        agent = GetComponent<NavMeshAgent>();
        motion = GetComponent<BrownianMotion>();
        if(motion.enabled) {
            motion.enabled = false;
            agent.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canAttack = true;

        spawner = FindAnyObjectByType<CatSpawner>();

        Fish.OnFishDied += OnFishDied;
    }

    void OnFishDied()
    {
        InterruptAttack();
    }

    void OnCollisionEnter(Collision collision)
    {
        // check for collision w/ ground
        if(((1 << collision.gameObject.layer) & walkLayer) != 0) {
            canAttack = false;

            if(!motion.enabled) {
                motion.enabled = true;
                agent.enabled = true;
            }
        }

        // prevent it from re-colliding with enemy
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
            Fish fishComp = collision.gameObject.GetComponent<Fish>();

            attackCoroutine = StartCoroutine(AttackCycle(fishComp));
        }
    }

    IEnumerator AttackCycle(Fish fishComp)
    {
        for(int i = 0; i < 3; i++) {
            // in case the fish dies
            if(fishComp == null) {
                StopAttacking();
                yield break;
            }
            else {
                yield return new WaitForSeconds(1f);
                fishComp.TakeDamage(damage);
            }
        }

        StopAttacking();
        attackCoroutine = null;
    }

    public void InterruptAttack()
    {
        // make sure attack cycle is stopped
        if(attackCoroutine != null) {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        StopAttacking();
    }

    void StopAttacking()
    {
        canAttack = false; 
        transform.parent = null; 

        // might need to re-add later for more of a jump?
        // rb.velocity = Vector3.zero;
        // rb.angularVelocity = Vector3.zero;

        rb.useGravity = true;
        rb.isKinematic = false;
    }

    void OnDestroy()
    {
        Fish.OnFishDied -= OnFishDied;

        // used for spawner logic -- may not be necessary later
        if(spawner != null) {
            spawner.RemoveCat(gameObject);
        }
    }
}
