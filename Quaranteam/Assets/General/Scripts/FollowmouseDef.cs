using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class FollowmouseDef : MonoBehaviour
{//Variables
    private bool itsGrabbed = false;
    public Rigidbody2D playerRigidBody2D;
    public CircleCollider2D playerCircleCollider2D;
    public BoxCollider2D playerBoxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        if (playerRigidBody2D==null)
        {
            playerRigidBody2D = GameObject.Find(this.name).GetComponent<Rigidbody2D>();
        }
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (itsGrabbed)
        {
            playerRigidBody2D.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Con esto la pelota sigue el movimiento del mouse.
        }
    }

    private void OnMouseDown()
    {
        itsGrabbed = true;
        playerRigidBody2D.isKinematic = true;
    }

    private void OnMouseUp()
    {
        itsGrabbed = false;
        playerRigidBody2D.isKinematic = false;
    }
}
