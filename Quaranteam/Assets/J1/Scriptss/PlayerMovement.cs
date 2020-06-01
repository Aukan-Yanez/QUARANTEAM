using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    private bool itsGrabbed = false;
    private bool itsDragged = false;
    private bool itsThrown = false;
    private Vector3 initPos;
    //private bool wasNotThrown = true;
    public Rigidbody2D playerRigidBody2D;
    public Rigidbody2D hookRigidBody2D;
    public SpringJoint2D elasticCord;

    public LineRenderer line;

    [Header("Color del elástico")]
    [Range(0,255)]
    public int R = 0;
    [Range(0, 255)]
    public int G = 0;
    [Range(0, 255)]
    public int B = 0;
    [Range(0, 255)]
    public int A = 0;


    // Start is called before the first frame update
    void Start()
    {
        line.SetPosition(0, hookRigidBody2D.position);
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        initPos = playerRigidBody2D.position;
        //line.startColor = new Color(R,G,B);
        //line.endColor = new Color(R, G, B);
    }

    // Update is called once per frame
    void Update()
    {
        if (itsGrabbed)
        {
            playerRigidBody2D.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Con esto la pelota sigue el movimiento del mouse.
        }

        //if (wasNotThrown) //En caso de necesitar controlar que se tire la pelota solo una vez. Por defecto solo se puede tirar una vez.
        if (passedOriginPoint())
        {
            elasticCord.enabled = false;
            line.enabled = false;
            
        }

        if (elasticCord.enabled == true)
        {
            draw();
        }


    }

    private void OnMouseDown()
    {
        itsGrabbed = true;
        playerRigidBody2D.isKinematic = true;
        
        if (!itsThrown)
        {
            itsDragged = true;
        }
    }

    private void OnMouseUp()
    {
        itsGrabbed = false;
        playerRigidBody2D.isKinematic = false;

        if (itsDragged)
        {
            itsThrown = true;
        }
    }

    private bool passedOriginPoint()
    {
        if (itsThrown)
        {
            double playersPosition = playerRigidBody2D.position.x;
            double positiveRange = hookRigidBody2D.position.x + 0.5;
            double negativeRange = hookRigidBody2D.position.x - 0.5;
            if (playersPosition > negativeRange && playersPosition < positiveRange)
            {
                //wasNotThrown = false;
                return true;
            }
            return false;
        }
        return false;
    }



    private void OnDrawGizmos()
    {
        Vector3 inicio;
        Vector3 distance;
        Vector3 fin;
        if (elasticCord.enabled == true)
        {
            inicio = hookRigidBody2D.position;
            distance = hookRigidBody2D.position - playerRigidBody2D.position;
            fin =inicio + distance;
        }
        else
        {
            inicio = Vector3.zero;
            fin = Vector3.zero;
        }

        Gizmos.color = new Color(R,G,B);
        Gizmos.DrawLine(fin, inicio);
    }

    private void draw()
    {
        Vector3 inicio = hookRigidBody2D.position;
        Vector3 distance = hookRigidBody2D.position - playerRigidBody2D.position;
        Vector3 fin = inicio + distance;
        line.SetPosition(1, fin);
    }

    public void reset(Vector3 d)
    {
        playerRigidBody2D.position=d;
        hookRigidBody2D.position=d;
        line.SetPosition(0, d);
    }

    public Vector3 getInitPos()
    {
        return initPos;
    }

    //PRINTS PARA REVISAR VALORES:
    //
    //Dentro de la funcion passedOrigin():       //Debug.Log("Player: " + posicionPlayer); Debug.Log("Hook range: (" + rangoPositivo + ", " + rangoNegativo + ")");




}
