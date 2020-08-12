using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class startAnimation : MonoBehaviour, IPointerClickHandler
{

    public GameObject ButtonGame1;
    public GameObject ButtonGame2;
    public GameObject ButtonStart;

    public GameObject Mainmenu;
    public GameObject MenuGame1;
    public GameObject MenuGame2;

    private Animator J1_anim;
    private Animator J2_anim;
    private Animator Start_Anim;

    bool toquesJ1;
    bool toquesJ2;
    bool ocupadoJ1;
    bool ocupadoJ2;

    int countJ1;
    int countJ2;

    int tap;
    PointerEventData eventData;

    private void Awake()
    {
        J1_anim = ButtonGame1.GetComponent<Animator>();
        J2_anim = ButtonGame2.GetComponent<Animator>();
        Start_Anim = ButtonStart.GetComponent<Animator>();
        toquesJ1 = false;
        toquesJ2 = false;
        ocupadoJ1 = false;
        ocupadoJ2 = false;
    }

    void FixedUpdate()
    {
        if (toquesJ1 == true)
        {
            countJ1 = countJ1 + 1;
        }
        if(toquesJ2 == true)
        {
            countJ2 = countJ2 + 1;
        }
    }


    void Update()
    {
        string J1 = null;
        string J2 = null;

        if (ButtonGame1.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0) != null)
        {
            if(ButtonGame1.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length != 0)
            {
                J1 = ButtonGame1.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name;
            }
        }
        if (ButtonGame2.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0) != null)
        {
            if (ButtonGame2.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length != 0)
            {
                J2 = ButtonGame2.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name;
            }
        }
        
        /*if (J1 == "Highlighted")
        {
            Start_Anim.Play("StartJ1");
            //J2_anim.Play("Normal");
        }

        if (J2 == "Highlighted")
        {
            Start_Anim.Play("StartJ2");
            //J1_anim.Play("Normal");
        }
        if (toquesJ1 == true)
        {
            Mainmenu.SetActive(false);
            MenuGame1.SetActive(true);
        }*/
    }

    public void primerToqueJ1()
    {
        if(toquesJ1 == false)
        {
            if (ocupadoJ2 == true)
            {
                J2_anim.Play("Normal");
                Start_Anim.Play("StarJ22");
                ocupadoJ2 = false;
                toquesJ2 = false;
                countJ2 = 0;
            }
            J1_anim.Play("Highlighted");
            Start_Anim.Play("StarJ1");
            ocupadoJ1 = true;
            toquesJ1 = true;
        }
    }

    public void segundoToqueJ1()
    {
        Debug.Log("contador J1: " + countJ1);
        if (toquesJ1 == true && countJ1 > 30)
        {
            toquesJ1 = false;
            J1_anim.Play("Normal");
            Mainmenu.SetActive(false);
            MenuGame1.SetActive(true);
        }
    }

    public void primerToqueJ2()
    {
        if(toquesJ2 == false)
        {
            if(ocupadoJ1 == true)
            {
                J1_anim.Play("Normal");
                Start_Anim.Play("StarJ12");
                ocupadoJ1 = false;
                toquesJ1 = false;
                countJ1 = 0;
            }
            J2_anim.Play("Highlighted");
            Start_Anim.Play("StarJ2");
            ocupadoJ2 = true;
            toquesJ2 = true;
        }
    }

    public void segundoToqueJ2()
    {
        Debug.Log("contador J2: " + countJ2);
        if (toquesJ2 == true && countJ2 > 30)
        {
            toquesJ2 = false;
            J2_anim.Play("Normal");
            Mainmenu.SetActive(false);
            MenuGame2.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tap = eventData.clickCount;

        if (tap == 1)
        {
            Debug.Log("Primer Click");
        }
        if (tap == 2)
        {
            Debug.Log("Segundo Click");
        }

    }
    public void backJ1()
    {
        Start_Anim.Play("StarJ11");
        J1_anim.Play("Normal");
        countJ1 = 0;
        countJ2 = 0;
    }
    public void backJ2()
    {
        Start_Anim.Play("StarJ22");
        J2_anim.Play("Normal");
        countJ1 = 0;
        countJ2 = 0;
    }
}
