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

    private Animator J1_anim;
    private Animator J2_anim;
    private Animator Start_Anim;

    private void Awake()
    {
        J1_anim = ButtonGame1.GetComponent<Animator>();
        J2_anim = ButtonGame2.GetComponent<Animator>();
        Start_Anim = ButtonStart.GetComponent<Animator>();
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

        if (J1 == "Highlighted")
        {
            Start_Anim.Play("StartJ1");
            //J2_anim.Play("Normal");
        }

        if (J2 == "Highlighted")
        {
            Start_Anim.Play("StartJ2");
            //J1_anim.Play("Normal");
        }
        /*
        if (J1 == "Normal")
        {
            Start_Anim.Play("StartJ11");
        }

        if(J2 == "Normal")
        {
            Start_Anim.Play("StartJ22");
        }*/
    }
}
