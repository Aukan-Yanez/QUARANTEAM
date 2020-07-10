using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Settings;
    public GameObject MenuGame1;
    public GameObject MenuGame2;
    public GameObject MenuGame3;


    public static bool isMenu = false;
    public static bool isGame1 = false;
    public static bool isGame2 = false;
    public static bool isGame3 = false;
 

    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void exitGame()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    public void showSettingFromMenu()
    {
        if (!isMenu)
        {
            isMenu = true;
        }
    }

    public void showSettingFromGame1()
    {
        if(!isGame1)
        {
            isGame1 = true;
        }
        
    }

    public void showSettingFromGame2()
    {
        if (!isGame2)
        {
            isGame2 = true;
        }

    }
    public void showSettingFromGame3()
    {
        if (!isGame3)
        {
            isGame3 = true;
        }

    }

    public void Back()
    {
        Settings.SetActive(false);

        if (isMenu)
        {
            isMenu = false;
            Menu.SetActive(true);
        }
        if (isGame1)
        {
            isGame1 = false;
            MenuGame1.SetActive(true);
        }
        if (isGame2)
        {
            isGame2 = false;
            MenuGame2.SetActive(true);
        }
        if (isGame3)
        {
            isGame3 = false;
            MenuGame3.SetActive(true);
        }
    }
}
