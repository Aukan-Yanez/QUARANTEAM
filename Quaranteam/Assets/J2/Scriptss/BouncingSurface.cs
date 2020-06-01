using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingSurface : MonoBehaviour
{
    [Header("BouncingSurface Components")]
    [Tooltip("Componente Rigidbody2D del trampolin.")]
    public Rigidbody2D objectRigidbody2D;
    [Tooltip("Componente Transform del trampolin.")]
    public Transform objectTransform;
    [Tooltip("Componente Collider2D del trampolin.")]
    public Collider2D objectCollider2D;

    [Header("BouncingSurface Properties")]
    [Range(-10, 10)]
    [Tooltip("Indica el radio de detección del trampolin.")]
    public float detectionRadius=0.5f;
    [Range(0, 9000)]
    [Tooltip("Indica la fuerza con la que impulsa trampolin.")]
    public float jumpForce = 10f;
    [Header("BouncingSurface Directions")]
    [Tooltip("Indica si el trampolin impulsa o no hacia arriba. (True indica que impulsará hacia arriba)")]
    public bool up = true;
    [Tooltip("Indica si el trampolin impulsa o no hacia abajo. (True indica que impulsará hacia abajo)")]
    public bool down = false;
    [Tooltip("Indica si el trampolin impulsa o no hacia derecha. (True indica que impulsará hacia derecha)")]
    public bool right = false;
    [Tooltip("Indica si el trampolin impulsa o no hacia izquierda. (True indica que impulsará hacia izquierda)")]
    public bool left = false;

    [Header("Mask of items to check")]
    [Tooltip("El trampolín solo detectará objetos asociados a este Layer.")]
    public LayerMask layerMask;


    private void FixedUpdate()
    {
        checkArround();
    }

    private void checkArround()
    {
        if (objectTransform == null) { return; }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(objectTransform.position, detectionRadius, layerMask);
        foreach(Collider2D collider in colliders)
        {
            if (GameObject.Find(collider.name).GetComponent<Rigidbody2D>() != null)
            {
                if(collider != objectCollider2D)
                {
                    applyJumpForce(GameObject.Find(collider.name).GetComponent<Rigidbody2D>());
                }
            }
            
        }
    }


    private void applyJumpForce(Rigidbody2D objectRb)
    {
        Vector3 force = new Vector3(0,0,0);

        if (up)
        {
            force.y += jumpForce;
            objectRb.AddForce(force);
        }
        if (down)
        {
            force.y += -jumpForce;
            objectRb.AddForce(force);
        }
        if (left)
        {
            force.x += -jumpForce;
            objectRb.AddForce(force);
        }
        if (right)
        {
            force.x += jumpForce;
            objectRb.AddForce(force);
        }
        
    }


    private void OnDrawGizmosSelected()
    {
        if (objectTransform == null) { return; }
        Gizmos.DrawWireSphere(objectTransform.position, detectionRadius);
    }

}
