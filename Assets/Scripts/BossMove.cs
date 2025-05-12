using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMove : MonoBehaviour
{
    public float updateRate = 0.2f; // how often it checks for new target pos
    public GameObject projectile;
    public Transform shootPoint;
    public float shotSpeed = 8f;
    public float shootInterval = 2f; 

    private Transform target;
    private NavMeshAgent agent;
    private float lookTimer;
    private float shootTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lookTimer = 0f;
        shootTimer = 0f;

        target = FindAnyObjectByType<PlayerMovement>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        lookTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;

        if(lookTimer >= updateRate) {
            lookTimer = 0f;
            agent.SetDestination(target.position);
        }

        if(shootTimer >= shootInterval) {
            shootTimer = 0f; 

            Vector3 directionToTarget = target.position - shootPoint.position;

            // Convert to local space relative to shootPoint's parent
            Vector3 localDir = shootPoint.parent.InverseTransformDirection(directionToTarget.normalized);

            // Calculate pitch angle (rotation around x axis)
            float angleX = Mathf.Atan2(-localDir.y, localDir.z) * Mathf.Rad2Deg;

            // Apply only x-axis rotation (preserve y and z)
            Vector3 currentRotation = shootPoint.localEulerAngles;
            shootPoint.localEulerAngles = new Vector3(angleX, currentRotation.y, currentRotation.z);
            
            GameObject newProjectile = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
            FishProjectile projectileComp = newProjectile.GetComponent<FishProjectile>();
            projectileComp.Initialize(shootPoint.forward);
        }
    }
}
