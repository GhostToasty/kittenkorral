using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishMove : MonoBehaviour
{
    public float updateRate = 0.2f; // how often it checks for new target pos

    private Transform target;
    private NavMeshAgent agent;
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = 0f;

        target = FindAnyObjectByType<PlayerMovement>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(target != null && timer >= updateRate) {
            agent.SetDestination(target.position);
            timer = 0f;
        }
    }
}
