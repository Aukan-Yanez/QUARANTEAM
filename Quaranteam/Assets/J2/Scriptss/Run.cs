using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    public GameObject follow;

    public int rango;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x + rango < follow.transform.position.x)
        {
            transform.position = new Vector3(follow.transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
