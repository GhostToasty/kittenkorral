using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFloater : MonoBehaviour
{
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
    public float rotationSpeed = 50f;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // floating movement
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // rotating movement
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
