using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishMove : MonoBehaviour
{
    public float updateRate = 0.2f; // how often it checks for new target pos
    public float detectionRadius = 10f;
    public float fov = 60f;
    public LayerMask targetMask;
    public bool shootingType;

    private FishShoot fishShoot;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    private BrownianMotion motion;
    
    // Start is called before the first frame update
    void Start()
    {
        if(shootingType) {
            fishShoot = GetComponent<FishShoot>();
        }

        motion = GetComponent<BrownianMotion>();
        agent = GetComponent<NavMeshAgent>();
        timer = 0f;

        target = FindAnyObjectByType<PlayerMovement>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(IsPlayerInVision()) {
            if(shootingType) {
                FaceTarget();

                if(!fishShoot.enabled) {
                    fishShoot.enabled = true;
                }
            }

            if(timer >= updateRate && !shootingType) {
                agent.SetDestination(target.position);
            }

            if(motion.enabled) {
                motion.enabled = false; // disable brownian motion
            }
        }
        else {
            // turn shooting off if player not in vision
            if(shootingType && fishShoot.enabled) {
                fishShoot.enabled = false;
            }

            if(timer >= updateRate && !motion.enabled) {
                motion.enabled = true; // resume brownian motion
            }
        }

        // reset timer
        if(timer >= updateRate) {
            timer = 0f;
        }
    }

    bool IsPlayerInVision()
    {
        // if (target == null) return false;

        Vector3 directionToTarget = target.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        // Check distance
        if (distanceToTarget > detectionRadius) return false;

        // Check angle
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget.normalized);
        if (angleToTarget > fov / 2f) return false;

        // Check line of sight
        if (Physics.Raycast(transform.position, directionToTarget.normalized, out RaycastHit hit, detectionRadius, targetMask))
        {
            if (hit.transform == target)
            {
                return true;
            }
        }

        return false;
    }

    void FaceTarget()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0f; // keep rotation horizontal

        if(dir.magnitude > 0.01f) {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, -fov / 2f, 0) * transform.forward * detectionRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, fov / 2f, 0) * transform.forward * detectionRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}
