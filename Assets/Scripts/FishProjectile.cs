using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishProjectile : MonoBehaviour
{
    private Vector3 moveDir;
    public float speed = 5f;
    public float lifetime = 10f;

    public void Initialize(Vector3 dir)
    {
        moveDir = dir.normalized;
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime); // destroy projectile eventually
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        // Debug.Log("hit " + hit.name);

        if(hit.tag == "Player") {
            // deal damage to player
            PlayerStats player = hit.GetComponent<PlayerStats>();
            player.TakeDamage();
        }

        Destroy(gameObject);
    }
}
