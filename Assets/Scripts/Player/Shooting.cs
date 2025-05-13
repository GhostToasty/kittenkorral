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
    private InputAction shoot; // now refers to specifically shooting cats
    private InputAction swap; // now refers to shooting yarn just too lazy to change the name rn -- Matthew

    private bool inYarnMode = true;
    private Pause pause;

    // sfx
    private GunSound yarnSound;
    private CatSound catSound;

    void Awake()
    {
        actions = new InputSystem_Actions();
        pause = GetComponentInChildren<Pause>();
    }

    void Start()
    {
        yarnSound = GetComponentInChildren<GunSound>();
        catSound = GetComponentInChildren<CatSound>();

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

    private void OnShootCat(InputAction.CallbackContext context)
    {     
        // prevent shooting inputs while game is paused
        if(pause.GetPauseState()) {
            return;
        }

        if(ammo > 0) {
            Vector3 spawnPos = cameraTransform.position + cameraTransform.forward * spawnDistance;

            GameObject cat = Instantiate(ammoPool[0], spawnPos, cameraTransform.rotation);
            ammoPool.Remove(ammoPool[0]);

            Rigidbody rb = cat.GetComponent<Rigidbody>();
            rb.AddForce(cameraTransform.forward * shootForce, ForceMode.Impulse);

            ammo--;
            ammoText.text = "Ammo: " + ammo;

            catSound.PlayCatSound();
        }
        else {
            // Debug.Log("out of ammo");
            StartCoroutine(AmmoTextFeedback());
        }
    }

    // to make your ammo counter more noticeable when trying to shoot while you're out
    IEnumerator AmmoTextFeedback()
    {
        ammoText.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        ammoText.color = Color.white;
    }

    private void OnShootYarn(InputAction.CallbackContext context)
    {
        // prevent shooting inputs while game is paused
        if(pause.GetPauseState()) {
            return;
        }

        Vector3 spawnPos = cameraTransform.position + cameraTransform.forward * spawnDistance;

        GameObject yarn = Instantiate(yarnPrefab, spawnPos, cameraTransform.rotation);

        Rigidbody rb = yarn.GetComponent<Rigidbody>();
        rb.AddForce(cameraTransform.forward * shootForce, ForceMode.Impulse);

        yarnSound.PlayYarnSound();
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
        shoot.performed += OnShootCat;

        swap = actions.Player.Swap;
        swap.Enable();
        swap.performed += OnShootYarn;
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