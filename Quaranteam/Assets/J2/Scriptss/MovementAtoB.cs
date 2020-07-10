using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MovementAtoB : MonoBehaviour
{
    [Header("Guide Points")]
    public GameObject[] points;
    [Header("Speed")]
    public float speed = 1f;
    [Header("Loop")]
    public bool activateLoop = false;
    [Header("Die when finished")]
    public bool dieInEnd = false;

    private float step;
    private bool inPoint;
    private GameObject nextPoint;
    private GameObject pointEnd;
    private int currentPositionInPionts;

    // Start is called before the first frame update
    void Start()
    {
        step  = speed * Time.deltaTime;
        inPoint = false;
        nextPoint = points[0];
        currentPositionInPionts = 0;
        pointEnd = points[points.Length-1];
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPoint) // aun no llega al punto
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPoint.transform.position, step);

            if(nextPoint.transform.position == transform.position)
            {
                inPoint = true;
            }
        }
        else // llego al punto
        {
            if(transform.position != pointEnd.transform.position) // si NO es el ultimo punto
            {
                nextPoint = points[currentPositionInPionts + 1];
                currentPositionInPionts++;
                inPoint = false;
            }
            else // si es el ultimo punto
            {
                if (activateLoop)
                {
                    nextPoint = points[0];
                    currentPositionInPionts = 0;
                    inPoint = false;
                }
                else
                {
                    if (dieInEnd)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
