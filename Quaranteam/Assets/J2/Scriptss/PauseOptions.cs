using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseOptions : MonoBehaviour
{
    public GameObject menu;

    public void TouchPause()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }

    public void resumeButton()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void menuButton()
    {
        SceneManager.LoadScene(0);
    }
}
