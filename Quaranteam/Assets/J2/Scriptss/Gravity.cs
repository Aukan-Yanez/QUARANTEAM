using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Gravity : MonoBehaviour
{

    [TextArea(4,4)]
    public string ConsiderecionesDeUso = "Los objetos a ser atraidos por la gravedad deben contar con el componente Rigidbody2D. Para ser detectados dentro de un rango deben contar a demás con un componente Collider2D.";

    [Header("Gravity Components")]
    [Tooltip("Componente Rigidbody2D del objeto con gravedad.")]
    public Rigidbody2D objectRigidbody2D;
    [Tooltip("Componente Transform del objeto con gravedad.")]
    public Transform objectTransform;
    [Tooltip("Componente Transform del halo del objeto con gravedad.")]
    public Transform haloTransform;

    [Header("Gravity Properties")]
    [Range(-500, 500)]
    [Tooltip("Indica la fuerza de gravedad del objeto.")]
    public float gravity = 9.8f;
    [Range(0, 5000)]
    [Tooltip("Indica la masa del objeto.")]
    public float mass = 1f;

    [Space]
    [Range(-500, 500)]
    [Tooltip("Indica la proporcion del halo del objeto. (Este rango visible no indica exactamente que se esten detectando elementos dentro de él a menos que este en concordancia con 'objectDetectionRange')")]
    public int haloProportion = 20;

    [Header("Specific Object Properties")]
    [Tooltip("Indica si se detecta o no un objeto específico para aplicar gravedad. (True indica que se va a detectar un objeto específico)")]
    public bool checkSpecificObject = true;
    [Tooltip("Nombre del objeto a detectar.")]
    public string specificObjectName = "player";

    [Header("Specific Objects Properties (List)")]
    [Tooltip("Indica si se detecta o no una lista de objetos para aplicar gravedad. (True indica que se va a detectar una lista de objetos)")]
    public bool checkObjectList = false;
    [Tooltip("Lista del objetos a detectar. (Objetos del tipo GameObject)")]
    public GameObject[] objectList;

    [Header("Specific Range Properties")]
    [Tooltip("Indica si detecta o no objetos dentro de un rango especificado para aplicar gravedad. (True indica que detecta objetos dentro de un rango)")]
    public bool checkInRange = false;
    [Range(-10, 10)]
    [Tooltip("Rango de detección de los objetos. (Este es el rango real de deteccion de elementos)")]
    public float objectDetectionRange = 0f;

    [Header("To check all")]
    [Tooltip("Indica si se detecta o no a todos los objetos. (True indica que se van a detectar todos los objetos)")]
    public bool checkAll = false;

    [Header("Mask of items to check")]
    [Tooltip("El blackhole solo detectara objetos asociados a este Layer.")]
    public LayerMask layers;



    private void Start()
    {
        
    }


    private void FixedUpdate()
    {
        applyGravity();
    }


    private void applyGravity()
    {
        if (checkSpecificObject)
        {
            attractSpecific();
        }
        if (checkObjectList)
        {
            attractObjectList();
        }
        if (checkInRange)
        {
            attractInrange();
        }
        if (checkAll)
        {
            attractAll();
        }
    }

    private void attractSpecific()
    {
        Rigidbody2D objectRigibody = GameObject.Find(specificObjectName).GetComponent<Rigidbody2D>();
        if (objectRigibody != null)
        {
            objectRigibody.gravityScale = 0;
            Attract(objectRigibody);
        }
    }

    private void attractObjectList()
    {
        for (int i = 0; i < objectList.Length; i++)
        {
            Rigidbody2D objectRigidbody = objectList[i].GetComponent<Rigidbody2D>();
            if (objectRigidbody != null)
            {
                objectRigidbody.gravityScale = 0;
                Attract(objectRigidbody);
            }
        }
    }

    private void attractInrange()
    {
        Collider2D[] closeCollider = Physics2D.OverlapCircleAll(objectTransform.position, objectDetectionRange, layers);
        GameObject[] all = GameObject.FindObjectsOfType<GameObject>();

        //Para cada Collider2D
        foreach (Collider2D collider in closeCollider)
        {
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i].GetComponent<Collider2D>() != null)
                {
                    if (collider.Equals(all[i].GetComponent<Collider2D>()))
                    {
                        if (all[i].GetComponent<Rigidbody2D>() != null)
                        {
                            if (!all[i].GetComponent<Rigidbody2D>().Equals(objectRigidbody2D))
                            {
                                all[i].GetComponent<Rigidbody2D>().gravityScale = 0;
                                Attract(all[i].GetComponent<Rigidbody2D>());
                            }
                        }
                    }
                }

            }
        }
    }

    private void attractAll()
    {
        Rigidbody2D[] all = GameObject.FindObjectsOfType<Rigidbody2D>();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i] != null)
            {
                if (!all[i].Equals(objectRigidbody2D))
                {
                    all[i].gravityScale = 0;
                    Attract(all[i]);
                }
            }
        }
    }



    void Attract(Rigidbody2D objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract;

        Vector3 direction = objectRigidbody2D.position - rbToAttract.position;

        float distance = direction.magnitude;

        float forceMagnitude = gravity * (mass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }



    private void OnDrawGizmosSelected()
    {
        if (objectTransform == null)
            return;

        Gizmos.DrawWireSphere(objectTransform.position, objectDetectionRange);
        if (haloTransform != null)
        {
            float haloProportion = objectTransform.localScale.x* this.haloProportion/100;

            haloTransform.position = objectTransform.position;
            haloTransform.localScale = new Vector2(haloProportion + objectDetectionRange, haloProportion + objectDetectionRange);
        }
    }


    /*
    public enum Opciones
    {
        UnaOpcion,
        Otra,
        LaUltima
    }
    public Opciones opcion = Opciones.Otra;
    */
}
