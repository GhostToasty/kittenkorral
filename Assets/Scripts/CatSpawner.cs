using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    public List<GameObject> catPrefabs;
    // public GameObject catPrefab;
    public int spawnLimit = 5;
    public float spawnInterval = 1f;

    private List<GameObject> catsInTrigger = new List<GameObject>();
    private Collider spawnTrigger;
    private bool canSpawn = true;
    private int totalCats = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnTrigger = GetComponent<Collider>();
        if(!spawnTrigger.isTrigger) {
            Debug.LogError("Spawner object needs to be a trigger collider.");
        }

        StartCoroutine(SpawnCatsContinuously());
    }

    IEnumerator SpawnCatsContinuously()
    {
        while(true) {
            yield return new WaitForSeconds(spawnInterval);

            if(catsInTrigger.Count < spawnLimit && canSpawn) {
                SpawnCat();
            }
        }
    }

    void SpawnCat()
    {
        Bounds bounds = spawnTrigger.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.min.y;

        Vector3 spawnPos = new Vector3(x,y,z);

        int index = Random.Range(0, catPrefabs.Count);

        Instantiate(catPrefabs[index], spawnPos, Quaternion.identity);

        totalCats++;
        if(totalCats == 20) {
            canSpawn = false; // just to prevent too many cats from existing
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("cat") && !catsInTrigger.Contains(other.gameObject)) {
            catsInTrigger.Add(other.gameObject);
        }
    }

    public void RemoveCat(GameObject cat)
    {
        if(catsInTrigger.Contains(cat)) {
            catsInTrigger.Remove(cat);
        }
    }
}
