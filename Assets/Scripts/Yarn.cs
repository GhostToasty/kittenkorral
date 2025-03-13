using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yarn : MonoBehaviour
{
    public float catchRadius = 3f;
    public float pullSpeed = 2f;
    public LayerMask catchLayer;

    // Update is called once per frame
    void Update()
    {
        DetectCats();
    }

    void DetectCats()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, catchRadius, catchLayer);
        foreach(Collider hitCollider in hitColliders) {
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
            Vector3 pullDir = (transform.position - rb.position).normalized;
            rb.AddForce(pullDir * pullSpeed, ForceMode.Acceleration);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, catchRadius);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Cat")) {
            Debug.Log("cat caught");
            Destroy(collision.gameObject);
        }
    }
}
