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

    public GameObject Logo;
    public GameObject SettText;
    public GameObject back;
    public GameObject Sett1;
    public GameObject Sett2;

    private Animator Logo_anim;
    private Animator SettText_anim;
    private Animator Back_anim;
    private Animator Sett1_anim;
    private Animator Sett2_anim;
    

    public static bool isMenu = false;
    public static bool isGame1 = false;
    public static bool isGame2 = false;

    private bool isExit;
    private int tiempo;

    public int nivel;


    private void Awake()
    {
        Logo_anim = Logo.GetComponent<Animator>();
        SettText_anim = SettText.GetComponent<Animator>();
        Back_anim = back.GetComponent<Animator>();
        Sett1_anim = Sett1.GetComponent<Animator>();
        Sett2_anim = Sett2.GetComponent<Animator>();
        isExit = false;
        tiempo = 0;
    }

    void FixedUpdate()
    {
        if (isExit == true)
        {
            tiempo = tiempo + 1;
        }
    }

    void Update()
    {
        if(tiempo > 80)
        {
            Settings.SetActive(false);
            reset();
        }
    }



    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void saveCharacter(int index)
    {
        PlayerPrefs.SetInt("character", index);
    }

    public void exitGame()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    

    public void showSettings()
    {
        reset();
        Logo_anim.Play("PauseLogo");
        SettText_anim.Play("Text");
        Back_anim.Play("back");
        Sett1_anim.Play("sett1");
        Sett2_anim.Play("sett2");
        
    }

    public void exitSettings()
    {
        isExit = true;
        Logo_anim.Play("PauseLogo2");
        SettText_anim.Play("Text2");
        Back_anim.Play("back2");
        Sett1_anim.Play("sett12");
        Sett2_anim.Play("sett22");
        
    }

    public void reset()
    {
        //Debug.Log("RESET");
        //Debug.Log("TIMER: " + tiempo);
        isExit = false;
        tiempo = 0;
    }

    public void levels()
    {
        nivel = 4;
    }

    public void infinity()
    {
        nivel = 8;
    }

    public void starJ2()
    {
        if(nivel > 0)
        {
            SceneManager.LoadScene(nivel);
        }
    }
}
