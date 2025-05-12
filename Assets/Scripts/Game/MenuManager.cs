using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject tutorialUI;
    public string levelName;

    void Start()
    {
        tutorialUI.SetActive(false); // just to make sure it's off
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelName);
    }

    public void ToggleTutorial()
    {
        tutorialUI.SetActive(!tutorialUI.activeSelf);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
