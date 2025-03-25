using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yarn : MonoBehaviour
{
    public float catchRadius = 3f;
    public float pullSpeed = 2f;
    public LayerMask catchLayer;

    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

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

    void OnCollisionEnter(Collision collision)
    {
        // bitwise operation to check if layer mask is the same
        if(((1 << collision.gameObject.layer) & catchLayer) != 0) {
            Debug.Log("cat caught");
            
            // get ammo
            Shooting shootingComp = FindAnyObjectByType<Shooting>();
            shootingComp.AddAmmo();

            Destroy(collision.gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, catchRadius);
    }
}