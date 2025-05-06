using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject yarnPrefab;
    // public GameObject catPrefab;

    [Header("Unity Setup")]
    public float shootForce = 10f;
    public float spawnDistance = 1f;
    public TextMeshProUGUI ammoText;

    [Header("Materials")]
    public GameObject ammoCapsuleObj;
    public Material yarnModeMat;
    public Material catModeMat;

    private int ammo = 0;
    private List<GameObject> ammoPool = new List<GameObject>();

    private Transform cameraTransform;

    // input system
    private InputSystem_Actions actions;
    private InputAction shoot;
    private InputAction swap;

    private bool inYarnMode = true;

    void Awake()
    {
        actions = new InputSystem_Actions();
    }

    void Start()
    {
        cameraTransform = Camera.main.transform;
        ammoText.text = "Ammo: " + ammo;

        Yarn.OnCatCaught += OnCatCaught;
    }

    void OnCatCaught(GameObject cat)
    {
        ammo++;
        ammoText.text = "Ammo: " + ammo;

        ammoPool.Add(cat);
        Debug.Log("added " + ammoPool[ammoPool.Count-1]);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {        
        if(inYarnMode) {
            Vector3 spawnPos = cameraTransform.position + cameraTransform.forward * spawnDistance;

            GameObject yarn = Instantiate(yarnPrefab, spawnPos, cameraTransform.rotation);

            Rigidbody rb = yarn.GetComponent<Rigidbody>();
            rb.AddForce(cameraTransform.forward * shootForce, ForceMode.Impulse);
        }
        else {
            if(ammo > 0) {
                Vector3 spawnPos = cameraTransform.position + cameraTransform.forward * spawnDistance;

                GameObject cat = Instantiate(ammoPool[0], spawnPos, cameraTransform.rotation);
                ammoPool.Remove(ammoPool[0]);

                Rigidbody rb = cat.GetComponent<Rigidbody>();
                rb.AddForce(cameraTransform.forward * shootForce, ForceMode.Impulse);

                ammo--;
                ammoText.text = "Ammo: " + ammo;
            }
            else {
                Debug.Log("out of ammo");
            }
        }
    }

    private void OnSwap(InputAction.CallbackContext context)
    {
        inYarnMode = !inYarnMode;
        
        // change visual on gun
        Renderer ammoRenderer = ammoCapsuleObj.GetComponent<Renderer>();
        ammoRenderer.material = inYarnMode ? yarnModeMat : catModeMat;
    }

    // public void AddAmmo(GameObject cat)
    // {
    //     ammo++;
    //     ammoText.text = "Ammo: " + ammo;

    //     ammoPool.Add(cat);
    // }

    private void OnEnable()
    {
        // input system boilerplate
        shoot = actions.Player.Shoot;
        shoot.Enable();
        shoot.performed += OnShoot;

        swap = actions.Player.Swap;
        swap.Enable();
        swap.performed += OnSwap;
    }

    private void OnDisable()
    {
        shoot.Disable();
        swap.Disable();
    }

    void OnDestroy()
    {
        Yarn.OnCatCaught -= OnCatCaught;
    }
}