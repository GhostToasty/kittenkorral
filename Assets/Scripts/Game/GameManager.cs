using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI enemiesText;
    public GameObject winImage;
    public GameObject returnButton;

    [Header("For Boss")]
    public GameObject bossPrefab;
    public Transform bossSpawnPos;

    private int totalFish;

    private AudioSource audioSource; // plays the sound for winning

    // Start is called before the first frame update
    void Start()
    {
        winImage.SetActive(false);
        returnButton.SetActive(false); // just to make sure they are off

        audioSource = GetComponent<AudioSource>();

        Fish[] fishInScene = FindObjectsByType<Fish>(FindObjectsSortMode.None);
        totalFish = fishInScene.Length;
        enemiesText.text = "Enemies: " + totalFish;

        Fish.OnFishDied += OnFishDied;
    }

    void OnFishDied()
    {
        totalFish--;
        enemiesText.text = "Enemies: " + totalFish;

        if(totalFish == 0) {
            // all normal fish dead -- spawn boss
            Instantiate(bossPrefab, bossSpawnPos.position, Quaternion.identity);
        }
        else if(totalFish < 0) {
            // boss dead -- end game
            Debug.Log("boss defeated!");
            audioSource.Play();
            winImage.SetActive(true);
            StartCoroutine(PauseOnEnd());
        }
    }

    IEnumerator PauseOnEnd()
    {
        yield return new WaitForSeconds(1f);
        returnButton.SetActive(true);

        // disable player
        FindAnyObjectByType<PlayerMovement>().enabled = false;
        FindAnyObjectByType<PlayerLook>().enabled = false;
        FindAnyObjectByType<Shooting>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnDestroy()
    {
        Fish.OnFishDied -= OnFishDied;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
