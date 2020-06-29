using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Portal : MonoBehaviour
{

    public PortalComponents components;
    public PortalProperties properties;

    [Tooltip("Portal solo detectara objetos en este layer.")]
    public LayerMask layerMask;
    private LinkedList<Collider2D> arriving;


    // Start is called before the first frame update
    void Start()
    {
        arriving = new LinkedList<Collider2D>();

        if (components.pointATransform == null)
        {
            components.pointATransform = GameObject.Find(this.name).GetComponent<Transform>();
        }
        if (components.pointARigidbody == null)
        {
            components.pointARigidbody = GameObject.Find(this.name).GetComponent<Rigidbody2D>();
        }
        if (components.pointBCollider == null)
        {
            components.pointBCollider = GameObject.Find(this.name).GetComponent<Collider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        teleporting();
        checkIfPassengerLeftPortal();
    }

    private void teleporting()
    {
        //Se obtienen todos los colliders alrededor del punto A
        float boxSizeX = components.pointATransform.localScale.x + properties.teleportDetectionAreaX;
        float boxSizeY = components.pointATransform.localScale.y + properties.teleportDetectionAreaY;
        Collider2D[] teleporting = Physics2D.OverlapBoxAll(components.pointARigidbody.position, new Vector2(boxSizeX, boxSizeY), 0);

        //Se obtiene el punto B
        Portal pointB = GameObject.Find(components.pointBRigidbody.name).GetComponent<Portal>();
        
        //Por cada collider alrededor del punto A
        foreach (Collider2D passenger in teleporting)
        {
            bool isntMyself = passenger != components.pointACollider;
            bool isntOtherPortal = passenger != components.pointBCollider;
            if (isntMyself && isntOtherPortal)
            {
                //Si el collider no a llegado desde el punto B
                if (!this.arriving.Contains(passenger))
                {
                    //Si no lo he mandado al punto B la primera vez
                    if (!pointB.arriving.Contains(passenger))
                    {
                        //se manda al punto B(Es para no entrar en un ciclo de vaiven)
                        GameObject.Find(passenger.name).GetComponent<Rigidbody2D>().position = pointB.components.pointARigidbody.position;
                        pointB.arriving.AddLast(passenger);
                    }
                }
            }
        }
    }

    private void checkIfPassengerLeftPortal()
    {
        //Se obtienen los Colliders en este punto (Punto A)
        float boxSizeX = components.pointATransform.localScale.x + properties.teleportDetectionAreaX;
        float boxSizeY = components.pointATransform.localScale.y + properties.teleportDetectionAreaY;
        Collider2D[] passengers = Physics2D.OverlapBoxAll(components.pointARigidbody.position, new Vector2(boxSizeX, boxSizeY), 0);
        List<Collider2D> inPortal = passengers.ToList<Collider2D>();


        //Por cada Collider que fue transportado hasta el punto A (desde el punto B)
        
        LinkedListNode<Collider2D> passenger = null;
        if (arriving.Count>0)
        {
            passenger = arriving.Find(arriving.First());
        }

        while (passenger != null)
        {
            if (!inPortal.Contains(passenger.Value))
            {
                arriving.Remove(passenger.Value);
            }
            passenger = passenger.Next;
        }
    }


    




    private void OnDrawGizmos()
    {
        if (components.pointATransform)
        {
            float boxSizeX = components.pointATransform.localScale.x + properties.teleportDetectionAreaX;
            float boxSizeY = components.pointATransform.localScale.y + properties.teleportDetectionAreaY;
            Gizmos.DrawWireCube(components.pointATransform.position, new Vector3(boxSizeX, boxSizeY, 0));
        }
    }

}

[System.Serializable]
public class PortalComponents
{
    [Header("Current Portal Components")]
    [Tooltip("Componente Transform del Portal.")]
    public Transform pointATransform;
    [Tooltip("Componente Rigidbody2D del Portal.")]
    public Rigidbody2D pointARigidbody;
    [Tooltip("Componente BoxCollider2D del Portal.")]
    public Collider2D pointACollider;

    [Header("Arriving Portal Components")]
    [Tooltip("Componente Rigidbody2D del Portal.")]
    public Rigidbody2D pointBRigidbody;
    [Tooltip("Componente BoxCollider2D del Portal.")]
    public Collider2D pointBCollider;
}



[System.Serializable]
public class PortalProperties
{
    [Tooltip("Indica si el portal esta o no activado. True indica que el portal si está activado.")]
    public bool enable = true;

    [Range(0, 5)]
    [Tooltip("Rango de area de deteccion de Colliders en el eje 'X'")]
    public float teleportDetectionAreaX = 0.5f;
    [Range(0, 5)]
    [Tooltip("Rango de area de deteccion de Colliders en el eje 'Y'")]
    public float teleportDetectionAreaY = 0.5f;
    

}