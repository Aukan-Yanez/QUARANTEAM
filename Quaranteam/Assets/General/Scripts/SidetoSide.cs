using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidetoSide : MonoBehaviour
{
    public Rigidbody2D objectToVaiven;
    [Range(0, 100)]
    public float speedX = 0f;
    [Range(0, 100)]
    public float speedY = 0f;
    [Range(0, 500)]
    public float totalTime = 0f;


    private float initX = 0f;
    private float initY = 0f;
    [SerializeField]
    private float currentTime = 0f;
    private float sentido = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        initX = objectToVaiven.position.x;
        initY = objectToVaiven.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        vaiven();
    }

    private void vaiven()
    {
        // pos = posinit + vel*t
        objectToVaiven.position = new Vector2(initX + speedX * currentTime, initY + speedY * currentTime);
        currentTime += sentido;

        if (currentTime >= totalTime)
        {
            currentTime = totalTime;
            sentido = -0.1f;
        }
        if (currentTime <= 0)
        {
            currentTime = 0;
            sentido = 0.1f;
        }
    }
}
