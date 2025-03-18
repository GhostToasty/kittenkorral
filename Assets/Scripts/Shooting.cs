using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject yarnPrefab;
    public float shootForce = 10f;
    public float spawnDistance = 1f;

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            Vector3 spawnPos = cameraTransform.position + cameraTransform.forward * spawnDistance;

            GameObject yarn = Instantiate(yarnPrefab, spawnPos, cameraTransform.rotation);

            Rigidbody rb = yarn.GetComponent<Rigidbody>();
            rb.AddForce(cameraTransform.forward * shootForce, ForceMode.Impulse);
        }
    }
}