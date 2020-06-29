using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpringJoint2D), typeof(LineRenderer))]
public class PlayermovementDef : MonoBehaviour
{
    #region Variables
    #region Público
    public PmComponents components;
    public PmProperties properties;
    public LayerMask layers;
    #endregion

    #region Privado
    private bool pressing = false;
    private string state = "inicio";
    #endregion
    #endregion


    #region Métodos
    void Start()
    {
        components.line.material.color = gameObject.GetComponent<SpriteRenderer>().color;
        components.line.SetPosition(0, components.hookRigidBody2D.position);
        components.line.startWidth = 0.05f;
        components.line.endWidth = 0.05f;

        if (components.playerRigidBody2D==null)
        {
            components.playerRigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        }
        if (components.elasticCord == null)
        {
            components.elasticCord = gameObject.GetComponent<SpringJoint2D>();
            if (components.hookRigidBody2D != null)
            {
                components.elasticCord.connectedBody = components.hookRigidBody2D;
                components.elasticCord.frequency = 2;
            }
        }
        if (components.line == null)
        {
            components.line = gameObject.GetComponent<LineRenderer>();
        }
    }

    
    void Update()
    {
        if (pressing)
        {
            components.playerRigidBody2D.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Con esto la pelota sigue el movimiento del mouse.
        }

        if (pressing) { onHoldDownMouse(); }
        
        cutTheRope();

        if (components.elasticCord.enabled == true)
        {
            draw();
        }
    }

    private void cutTheRope()
    {
        if (state == "volviendo" && isInRadius())
        {
            //Debug.Log("Cortando");
            components.elasticCord.enabled = false;
        }
    }

    private bool isInRadius()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(components.hookRigidBody2D.gameObject.transform.position, properties.cuttingRadius, layers);
        foreach (Collider2D collider in colliders)
        {
            if (collider.Equals(components.playerRigidBody2D.gameObject.GetComponent<CircleCollider2D>()))
            {
                return true;
            }
        }
        return false;
    }

    private void onHoldDownMouse()
    {
        if (!isInRadius())
        {
            if (state == "inicio")
            {
                state = "arrastrando"; //Debug.Log("Arrastrando");
            }
        }
        else
        {
            //Debug.Log("Agarrando");
        }
    }

    private void OnMouseDown()
    {
        pressing = true;
        components.playerRigidBody2D.isKinematic = true;
    }

    private void OnMouseUp()
    {
        pressing = false;
        components.playerRigidBody2D.isKinematic = false;

        if(state == "arrastrando")
        {
            state = "volviendo";//Debug.Log("Volviendo");
        }
    }


    private void draw()
    {
        if (components.line != null)
        {
            Vector3 inicio = components.hookRigidBody2D.position;
            Vector3 distance = components.hookRigidBody2D.position - components.playerRigidBody2D.position;
            Vector3 fin = inicio + distance;
            components.line.SetPosition(1, fin);
        }
    }


    private void OnDrawGizmos()
    {
        if (components.hookRigidBody2D)
        {
            Gizmos.DrawWireSphere(components.hookRigidBody2D.gameObject.transform.position, properties.cuttingRadius);
        }
    }

    #endregion
}


[System.Serializable]
public class PmComponents
{
    public Rigidbody2D playerRigidBody2D;
    public Rigidbody2D hookRigidBody2D;
    public SpringJoint2D elasticCord;
    public LineRenderer line;
}

[System.Serializable]
public class PmProperties
{
    [Header("Color del elástico")]
    [Range(0, 255)]
    public int R = 0;
    [Range(0, 255)]
    public int G = 0;
    [Range(0, 255)]
    public int B = 0;
    [Range(0, 255)]
    public int A = 0;

    [Range(0, 10)]
    public float cuttingRadius = 1;

    
}
