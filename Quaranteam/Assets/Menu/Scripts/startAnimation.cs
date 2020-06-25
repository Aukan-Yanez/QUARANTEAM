using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class startAnimation : MonoBehaviour
{
    public Button ButtonJ1;
    public Button ButtonJ2;
    public Button ButtonJ3;
    public Button ButtonStart;

    bool activateJ1 = false;
    bool activateJ2 = false;
    bool activateJ3 = false;

    void Update()
    {
        string J1 = ButtonJ1.gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string J2 = ButtonJ2.gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string J3 = ButtonJ3.gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (J1 == "Highlighted" && !activateJ1)
        {
            ButtonStart.gameObject.GetComponent<Animator>().SetTrigger("StartJ1");
            activateJ1 = true;
            activateJ2 = false;
            activateJ3 = false;

        }
        if (J2 == "Highlighted" && !activateJ2)
        {
            ButtonStart.gameObject.GetComponent<Animator>().SetTrigger("StartJ2");
            activateJ1 = false;
            activateJ2 = true;
            activateJ3 = false;
        }
        if (J3 == "Highlighted" && !activateJ3)
        {
            ButtonStart.gameObject.GetComponent<Animator>().SetTrigger("StartJ3");
            activateJ1 = false;
            activateJ2 = false;
            activateJ3 = true;
        }
        if (J1 == "Normal" && J2 == "Normal" && J3 == "Normal")
        {
            ButtonStart.gameObject.GetComponent<Animator>().SetTrigger("Normal");
            activateJ1 = false;
            activateJ2 = false;
            activateJ3 = false;
        }
    }
}
