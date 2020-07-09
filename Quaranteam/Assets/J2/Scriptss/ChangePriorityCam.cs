using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangePriorityCam : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public int newPriority = 11;
    public float waitSeconds = 1.0f;

    IEnumerator Process()
    {
        //Wait 1 second
        yield return StartCoroutine(Wait(waitSeconds));
        //Do process stuff
        vcam.Priority = newPriority;
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    void wait()
    {
        StartCoroutine("Process");
    }

    // Start is called before the first frame update
    void Start()
    {
        wait();
    }
}
