using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAtoB : MonoBehaviour
{
    [Header("Start object")]
    public GameObject start;
    [Header("End object")]
    public GameObject end;
    [Header("Speed")]
    public float speed = 1f;

    private float step;
    private bool inEnd;



    // Start is called before the first frame update
    void Start()
    {
        step  = speed * Time.deltaTime;
        inEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inEnd)
        {
            transform.position = Vector3.MoveTowards(start.transform.position, end.transform.position, step);
        }
        else
        {
            inEnd = true;
            transform.position = end.transform.position;
        }
    }
}
