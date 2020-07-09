using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenInteractives : MonoBehaviour
{

    public GameObject[] obj;
    public float timeMin = 1f;
    public float timeMax = 3f;

    void Start()
    {
        GeneratorObj();
    }

    void Update()
    {
        
    }

    void GeneratorObj()
    {
        Instantiate(obj[Random.Range(0, obj.Length)], transform.position, Quaternion.Euler(0,0,Random.Range(90,-90)) );
        Invoke("GeneratorObj", Random.Range(timeMin, timeMax));
    }
}
