using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenuUI;
    public GameObject pauseButton;

    public void Pause()
    {
        Debug.Log("PAUSE");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Debug.Log("RESUMEN");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    
    public void LoadMenu(string pScene)
    {
        Debug.Log("Loading menu...");
        SceneManager.LoadScene(pScene);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
    }

}
