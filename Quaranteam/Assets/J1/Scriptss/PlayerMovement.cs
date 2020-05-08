using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    private bool itsGrabbed = false;
    private bool itsDragged = false;
    private bool itsThrown = false;
    //private bool wasNotThrown = true;
    public Rigidbody2D playerRigidBody2D;
    public Rigidbody2D hookRigidBody2D;
    public SpringJoint2D elasticCord;


    // Start is called before the first frame update
    void Start()
    {
        
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




    //PRINTS PARA REVISAR VALORES:
    //
    //Dentro de la funcion passedOrigin():       //Debug.Log("Player: " + posicionPlayer); Debug.Log("Hook range: (" + rangoPositivo + ", " + rangoNegativo + ")");




}
