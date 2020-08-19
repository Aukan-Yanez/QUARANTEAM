using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenInteractives : MonoBehaviour
{

    public GameObject[] obj;
    public float timeMin = 1f;
    public float timeMax = 3f;
    public float rotationMin = 90;
    public float rotationMax = -90;

    public void GeneratorObj()
    {
        Invoke("InitiateObject", Random.Range(timeMin, timeMax));
    }

    void InitiateObject()
    {
        Instantiate(obj[Random.Range(0, obj.Length)], transform.position, Quaternion.Euler(0,0,Random.Range(rotationMin,rotationMax)) );
    }
}
