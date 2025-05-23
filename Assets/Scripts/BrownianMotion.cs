using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrownianMotion : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float jitter = 1f; // lower to make less random
    public float frequency = 1f; // increase to make more energetic
    public int octaves = 3; // lower to make less random
    public float moveRadius = 10f;
    public float noiseScale = 1f;

    private NavMeshAgent agent;
    private float timeOffsetX;
    private float timeOffsetZ;
    private float elapsedTime = 0f; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        timeOffsetX = Random.Range(-1000f, 1000f); // prevent all objects from moving identically
        timeOffsetZ = Random.Range(-1000f, 1000f);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime * frequency;
    }

    void FixedUpdate()
    {
        Vector3 moveDir = GetBrownianDirection(elapsedTime);
        Vector3 destination = transform.position + moveDir;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(destination, out hit, moveRadius, NavMesh.AllAreas)) {
            agent.SetDestination(hit.position);
        }

        if(moveDir != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    Vector3 GetBrownianDirection(float t)
    {
        // float x = Fbm(timeOffsetX, t, octaves);
        // float z = Fbm(timeOffsetZ, t, octaves);

        float x = (Mathf.PerlinNoise(timeOffsetX + t, timeOffsetZ) - 0.5f) * 2f;
        float z = (Mathf.PerlinNoise(timeOffsetZ + t, timeOffsetX) - 0.5f) * 2f;

        Vector3 dir = new Vector3(x, 0f, z).normalized * jitter;
        return dir;
    }

    // fractal Brownian motion
    float Fbm(float x, float t, int octaves)
    {
        float result = 0f;
        float amplitude = 0.5f;
        float frequency = 1f; // noiseScale;

        for(int i = 0; i < octaves; i++) {
            result += amplitude * Mathf.PerlinNoise(x * frequency, t * frequency);
            frequency *= 2f;
            amplitude *= 0.5f;
        }

        return result - 0.5f; // center around 0
    }


    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawWireSphere(transform.position, moveRadius);
    // }
}
