using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseUI;

    private bool isPaused;

    // input system
    private InputSystem_Actions actions;
    private InputAction pause;

    void Awake()
    {
        actions = new InputSystem_Actions();
        isPaused = false;
    }

    public bool GetPauseState()
    {
        return isPaused;
    }

    public void ResumeGame()
    {
        if(isPaused) {
            isPaused = false;
            pauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if(isPaused) {
            pauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; // not exactly necessary but here for consistency
            Time.timeScale = 0f;
        }
        else {
            pauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    private void OnEnable()
    {
        // input system boilerplate
        pause = actions.Player.Pause;
        pause.Enable();
        pause.performed += OnPause;
    }

    private void OnDisable()
    {
        pause.Disable();
    }
}
