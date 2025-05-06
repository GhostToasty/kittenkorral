using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string levelName;

    public void StartGame()
    {
        SceneManager.LoadScene(levelName);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
