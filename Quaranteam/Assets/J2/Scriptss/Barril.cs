using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barril : MonoBehaviour
{
    private bool itsGrabbed = false;
    public Transform barrilTr;
    public Rigidbody2D barrilRb;
    public Collider2D barrilCc;
    [Range(0, 100)]
    public float torque = 0f;

    private void Update()
    {
        barrilRb.AddTorque(torque);
        if (itsGrabbed)
        {
            barrilRb.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Con esto la pelota sigue el movimiento del mouse.
        }
    }

    private void OnMouseDown()
    {
        itsGrabbed = true;
        barrilRb.isKinematic = true;
    }

    private void OnMouseUp()
    {
        itsGrabbed = false;
        barrilRb.isKinematic = false;
    }
}
