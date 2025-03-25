using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject yarnPrefab;
    public GameObject catPrefab;

    public float shootForce = 10f;
    public float spawnDistance = 1f;

    private int ammo = 0;

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

                GameObject cat = Instantiate(catPrefab, spawnPos, cameraTransform.rotation);

                Rigidbody rb = cat.GetComponent<Rigidbody>();
                rb.AddForce(cameraTransform.forward * shootForce, ForceMode.Impulse);

                ammo--;
            }
            else {
                Debug.Log("out of ammo");
            }
        }
    }

    private void OnSwap(InputAction.CallbackContext context)
    {
        inYarnMode = !inYarnMode;
    }

    public void AddAmmo()
    {
        ammo++;
    }

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
}