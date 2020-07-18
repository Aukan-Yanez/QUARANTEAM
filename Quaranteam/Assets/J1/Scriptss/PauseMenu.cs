using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject exitMenuUI;

    public GameObject Logo;
    public GameObject Gral_Resume;
    public GameObject Gral_Settings;
    public GameObject Gral_Exit;

    private Animator Logo_Anim;
    private Animator Gral_Resume_Anim;
    private Animator Gral_Sett_Anim;
    private Animator Gral_Exit_Anim;

    void Update()
    {
        string resumen = Gral_Resume_Anim.gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string settings = Gral_Resume_Anim.gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string exit = Gral_Resume_Anim.gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (resumen == "Highlighted" || settings == "Highlighted" || exit == "Highlighted")
        {
            Gral_Resume_Anim.SetBool("Close", true); /**/
            Gral_Sett_Anim.SetBool("Open", true);
            Gral_Exit_Anim.SetBool("Open", true);
        }
    }


    private void Awake()
    {
        Gral_Resume_Anim = Gral_Resume.GetComponent<Animator>();
        Gral_Sett_Anim = Gral_Settings.GetComponent<Animator>();
        Gral_Exit_Anim = Gral_Exit.GetComponent<Animator>();
        Logo_Anim = Logo.GetComponent<Animator>();
    }

    public void Pause()
    {
        //Debug.Log("PAUSE");
        pauseMenuUI.SetActive(true);
        //Time.timeScale = 0f;

        Logo_Anim.Play("PauseLogo");
        Gral_Resume_Anim.Play("Resume");
        Gral_Resume_Anim.SetBool("Open", true);/**/
        Gral_Sett_Anim.Play("Settings");
        Gral_Exit_Anim.Play("Exit");

        
    }

    public void Resume()
    {
        //Debug.Log("RESUMEN");
        //pauseMenuUI.SetActive(false);

        Logo_Anim.Play("PauseLogo2");
        Gral_Resume_Anim.Play("Resume2");
        Gral_Resume_Anim.SetBool("Open", false); /**/
        Gral_Sett_Anim.Play("Settings2");
        Gral_Exit_Anim.Play("Exit2");
        Time.timeScale = 1f;
    }

    public void openAnim()
    {
        Gral_Resume_Anim.SetBool("Close", true);/**/
        Gral_Resume_Anim.SetBool("Open", true);
        Gral_Sett_Anim.SetBool("Open", true);
        Gral_Exit_Anim.SetBool("Open", true);
    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
    

}