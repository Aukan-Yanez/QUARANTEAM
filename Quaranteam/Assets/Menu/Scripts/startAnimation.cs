using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class startAnimation : MonoBehaviour
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

    public GameObject charizard;
    public GameObject iceClimber;
    public GameObject pikachu;

    private Animator charizard_anim;
    private Animator iceClimber_anim;
    private Animator pikachu_anim; 

    bool toquesJ1;
    bool toquesJ2;
    bool ocupadoJ1;
    bool ocupadoJ2;

    bool isCharizard;
    bool isIceClimber;
    bool isPikachu;
    bool charizardOcupado;
    bool iceClimberOcupado;
    bool pikachuOcupado;

    int countJ1;
    int countJ2;

    private void Awake()
    {
        J1_anim = ButtonGame1.GetComponent<Animator>();
        J2_anim = ButtonGame2.GetComponent<Animator>();
        Start_Anim = ButtonStart.GetComponent<Animator>();

        charizard_anim = charizard.GetComponent<Animator>();
        iceClimber_anim = iceClimber.GetComponent<Animator>();
        pikachu_anim = pikachu.GetComponent<Animator>();


        toquesJ1 = false;
        toquesJ2 = false;

        ocupadoJ1 = false;
        ocupadoJ2 = false;

        isCharizard = false;
        isIceClimber = false;
        isPikachu = false;

        charizardOcupado = false;
        iceClimberOcupado = false;
        pikachuOcupado = false;

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
        //Debug.Log("contador J1: " + countJ1);
        if (toquesJ1 == true && countJ1 > 30)
        {
            toquesJ1 = false;
            //J1_anim.Play("Normal");
            //Mainmenu.SetActive(false);
            MenuGame1.SetActive(true);
            charizard_anim.Play("First");
            iceClimber_anim.Play("First1");
            pikachu_anim.Play("First2");

            isCharizard = false;
            isIceClimber = false;
            isPikachu = false;
            charizardOcupado = false;
            iceClimberOcupado = false;
            pikachuOcupado = false;

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
        //Debug.Log("contador J2: " + countJ2);
        if (toquesJ2 == true && countJ2 > 30)
        {
            toquesJ2 = false;
            //J2_anim.Play("Normal");
            //Mainmenu.SetActive(false);
            MenuGame2.SetActive(true);
        }
    }
    public void backJ1()
    {
        Start_Anim.Play("StarJ11");
        J1_anim.Play("Normal");
        countJ1 = 0;
        countJ2 = 0;
        charizard_anim.Play("Normal");
        iceClimber_anim.Play("Normal");
        pikachu_anim.Play("Normal");
    }
    public void backJ2()
    {
        Start_Anim.Play("StarJ22");
        J2_anim.Play("Normal");
        countJ1 = 0;
        countJ2 = 0;
    }

    public void primerToqueCharizard()
    {   
        if(isCharizard == false)
        {
            if(iceClimberOcupado == true)
            {
                iceClimber_anim.Play("Normal");
                iceClimberOcupado = false;
                isIceClimber = false;
            }
            if(pikachuOcupado == true)
            {
                pikachu_anim.Play("Normal");
                pikachuOcupado = false;
                isPikachu = false;
            }
            charizard_anim.Play("Highlighted");
            charizardOcupado = true;
            isCharizard = true;
        }
        
    }
    public void primerToqueIceClimber()
    {
        if(isIceClimber == false)
        {
            iceClimber_anim.Play("Highlighted");
            if (charizardOcupado == true)
            {
                charizard_anim.Play("Normal");
                charizardOcupado = false;
                isCharizard = false;
                
            }
            if (pikachuOcupado == true)
            {
                pikachu_anim.Play("Normal");
                pikachuOcupado = false;
                isPikachu = false;
            }
            iceClimber_anim.Play("Highlighted");
            iceClimberOcupado = true;
            isIceClimber = true;
        }
    }
    public void primerToquePikachu()
    {
        if(isPikachu == false)
        {
            if (charizardOcupado == true)
            {
                charizard_anim.Play("Normal");
                charizardOcupado = false;
                isCharizard = false;
            }
            if (iceClimberOcupado == true)
            {
                iceClimber_anim.Play("Normal");
                iceClimberOcupado = false;
                isIceClimber = false;
            }
            pikachu_anim.Play("Highlighted");
            pikachuOcupado = true;
            isPikachu = true;
        }
    }
}